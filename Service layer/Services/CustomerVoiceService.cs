using System;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Domain_layer.Interfaces;
using Domain_layer.Models;
using Microsoft.Extensions.Configuration;
using Service_layer.DTOS.Chat;
using Service_layer.DTOS.Notification;
using Service_layer.DTOS.Voice;
using Service_layer.Services_Interfaces;

namespace Service_layer.Services
{
    public class CustomerVoiceService : ICustomerVoiceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAiVoiceJoinService _aiVoiceJoin;
        private readonly IAuditLogService? _auditLogService;
        private readonly INotificationService? _notificationService;
        private readonly CustomerInteractionBusinessLogic _businessLogic;
        private readonly string _meetingUrl;

        public CustomerVoiceService(
            IUnitOfWork unitOfWork,
            IAiVoiceJoinService aiVoiceJoin,
            IConfiguration configuration,
            IAuditLogService? auditLogService = null,
            INotificationService? notificationService = null)
        {
            _unitOfWork      = unitOfWork;
            _aiVoiceJoin     = aiVoiceJoin;
            _auditLogService = auditLogService;
            _notificationService = notificationService;
            _businessLogic   = new CustomerInteractionBusinessLogic(unitOfWork, auditLogService);
            _meetingUrl      = configuration["AiVoiceApi:MeetingUrl"] ?? string.Empty;
        }

        // ── 1. Start Call ──────────────────────────────────────────────────────

        public async Task<StartVoiceCallResponseDTO> StartCallAsync(StartVoiceCallRequestDTO request)
        {
            // Validate business exists
            var business = await _unitOfWork.Businesses.GetByIdAsync(request.BusinessId)
                ?? throw new ArgumentException($"Business '{request.BusinessId}' not found.");

            // Validate customer exists
            var customer = await _unitOfWork.Customers.GetByIdAsync(request.CustomerId)
                ?? throw new ArgumentException($"Customer '{request.CustomerId}' not found.");

            if (string.IsNullOrWhiteSpace(_meetingUrl))
                throw new InvalidOperationException("MeetingUrl is not configured in appsettings.");

            // Create Interaction
            var interaction = new Interaction
            {
                InteractionId = Guid.NewGuid().ToString(),
                BusinessId    = request.BusinessId,
                CustomerId    = request.CustomerId,
                Channel       = "Voice",
                Status        = "Open",
                StartedAt     = DateTime.UtcNow
            };

            await _unitOfWork.Interactions.AddAsync(interaction);
            await _unitOfWork.CompleteAsync();

            // Notify AI to join (non-fatal)
            await _aiVoiceJoin.JoinCallAsync(interaction.InteractionId, request.BusinessId, request.CustomerId);

            if (_auditLogService != null)
            {
                try
                {
                    await _auditLogService.LogInteractionActionAsync(
                        businessId:    request.BusinessId,
                        action:        "StartVoiceCall",
                        interactionId: interaction.InteractionId,
                        userId:        null);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"[CustomerVoiceService] Audit log failed: {ex.Message}");
                }
            }

            return new StartVoiceCallResponseDTO
            {
                InteractionId = interaction.InteractionId,
                MeetingUrl    = _meetingUrl,
                Status        = "connecting"
            };
        }

        // ── 2. Handle Call Completed (called by AI after the call ends) ────────

        public async Task HandleCallCompletedAsync(VoiceCallCompletedDTO payload)
        {
            var callData = payload.CallData;
            var analysis = payload.Analysis;

            // call_id == interaction_id sent in the join request
            var interaction = await _unitOfWork.Interactions.GetByIdAsync(callData.CallId);

            if (interaction == null)
                Debug.WriteLine($"[CustomerVoiceService] call-completed: interaction '{callData.CallId}' not found — storing orphan summary.");

            // Resolve + validate the owning business (BusinessId is a required FK).
            var businessId = interaction?.BusinessId ?? payload.BusinessId;
            if (string.IsNullOrWhiteSpace(businessId) ||
                await _unitOfWork.Businesses.GetByIdAsync(businessId) == null)
                throw new ArgumentException($"Business '{businessId}' not found.");

            // Build + persist CallSummary
            var summary = new CallSummary
            {
                Id             = Guid.NewGuid().ToString(),
                InteractionId  = interaction?.InteractionId,
                BusinessId     = businessId,
                CallId         = callData.CallId,
                StartTime      = callData.StartTime,
                EndTime        = callData.EndTime,
                DurationSeconds = callData.DurationSeconds,
                MessagesCount  = callData.MessagesCount,
                FullTranscript = callData.FullTranscript ?? string.Empty,
                MessagesJson   = JsonSerializer.Serialize(callData.Messages),
                AudioInfoJson  = JsonSerializer.Serialize(callData.AudioInfo),
                AudioUrlsJson  = JsonSerializer.Serialize(payload.AudioUrls ?? new()),

                Summary          = analysis.Summary ?? string.Empty,
                SummaryAr        = analysis.SummaryAr ?? string.Empty,
                SentimentScore   = analysis.OverallSentiment?.Score ?? 0.0,
                SentimentLabel   = analysis.OverallSentiment?.Label ?? "Neutral",
                MainTopicsJson   = JsonSerializer.Serialize(analysis.MainTopics),
                IntentsDetectedJson = JsonSerializer.Serialize(analysis.IntentsDetected),
                ActionsPerformedJson = JsonSerializer.Serialize(analysis.ActionsPerformed),
                KeyMomentsJson   = JsonSerializer.Serialize(analysis.KeyMoments),
                EscalationRequired = analysis.EscalationRequired,
                EscalationReason = analysis.EscalationReason,
                AnalyzedAt       = analysis.AnalyzedAt ?? DateTime.UtcNow,
                QueuedAt         = payload.QueuedAt ?? DateTime.UtcNow,
                CreatedAt        = DateTime.UtcNow
            };

            await _unitOfWork.CallSummaries.AddAsync(summary);

            // Turn any "OrderCreated" action the AI reported into a real Order record --
            // previously this JSON was only archived on the CallSummary and never became
            // an actual order (invisible to Dashboard, customer order counts, Owner Chat, etc).
            if (interaction != null)
            {
                var orderAction = analysis.ActionsPerformed?
                    .FirstOrDefault(a => string.Equals(a.Type, "OrderCreated", StringComparison.OrdinalIgnoreCase)
                                       || string.Equals(a.Type, "CreateOrder", StringComparison.OrdinalIgnoreCase));

                if (orderAction?.Details?.Items?.Any() == true)
                {
                    await _businessLogic.HandleVoiceOrderAsync(interaction, orderAction.Details.Items);
                }
            }

            // Turn an AI-flagged escalation into a real Ticket — previously this only
            // flipped the Interaction's status to "Escalated" and never created a Ticket,
            // so escalated calls never showed up anywhere in the Tickets list.
            string? ticketId = null;
            if (interaction != null && analysis.EscalationRequired)
            {
                var intentResult = new DetectedIntentResultDTO
                {
                    Intent             = "RequestHumanAgent",
                    RequiresEscalation = true,
                    EscalationReason   = analysis.EscalationReason
                };

                var ticket = await _businessLogic.HandleTicketAsync(
                    interaction, intentResult,
                    aiTicketDetails: null,
                    isEscalation: true);

                ticketId = ticket.TicketId;
                interaction.InteractionType = "Ticket";
                interaction.RelatedTicketId = ticketId;

                if (_notificationService != null)
                {
                    try
                    {
                        await _notificationService.CreateAsync(new NotificationCreateDTO
                        {
                            Title = "Customer needs a human agent",
                            Message = $"Ticket #{ticket.TicketId.Substring(0, 8)} — {ticket.Subject}",
                            BusinessId = interaction.BusinessId,
                            UserId = null
                        });
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"[CustomerVoiceService] Escalation notification failed: {ex.Message}");
                    }
                }
            }

            // Close the Interaction
            if (interaction != null)
            {
                interaction.Status  = analysis.EscalationRequired ? "Escalated" : "Closed";
                interaction.IsEnded = true;
                interaction.EndedAt = callData.EndTime;
                _unitOfWork.Interactions.Update(interaction);
            }

            await _unitOfWork.CompleteAsync();
        }

    }
}
