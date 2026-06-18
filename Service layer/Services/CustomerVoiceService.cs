using System;
using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;
using Domain_layer.Interfaces;
using Domain_layer.Models;
using Service_layer.DTOS.Voice;
using Service_layer.Services_Interfaces;

namespace Service_layer.Services
{
    public class CustomerVoiceService : ICustomerVoiceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAiVoiceJoinService _aiVoiceJoin;
        private readonly IAuditLogService? _auditLogService;

        public CustomerVoiceService(
            IUnitOfWork unitOfWork,
            IAiVoiceJoinService aiVoiceJoin,
            IAuditLogService? auditLogService = null)
        {
            _unitOfWork      = unitOfWork;
            _aiVoiceJoin     = aiVoiceJoin;
            _auditLogService = auditLogService;
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

            // Read MeetingUrl from Settings
            var settings = await _unitOfWork.Settings.GetByBusinessIdAsync(request.BusinessId);
            var meetingUrl = settings?.MeetingUrl;

            if (string.IsNullOrWhiteSpace(meetingUrl))
                throw new ArgumentException("MeetingUrl is not configured for this business. Please set it in Settings.");

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
            await _aiVoiceJoin.JoinCallAsync(interaction.InteractionId, request.BusinessId, meetingUrl);

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
                MeetingUrl    = meetingUrl,
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
