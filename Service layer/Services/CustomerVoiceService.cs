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
            // Validate
            if (string.IsNullOrWhiteSpace(request.MeetingUrl))
                throw new ArgumentException("meetingUrl is required.");

            var business = await _unitOfWork.Businesses.GetByIdAsync(request.BusinessId)
                ?? throw new ArgumentException($"Business '{request.BusinessId}' not found.");

            var meetingUrl = request.MeetingUrl;

            // Ensure customer record exists
            var customerId = await EnsureCustomerAsync(request.BusinessId, request.CustomerId);

            // Create Interaction
            var interaction = new Interaction
            {
                InteractionId = Guid.NewGuid().ToString(),
                BusinessId    = request.BusinessId,
                CustomerId    = customerId,
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
                Status        = "connecting"
            };
        }

        // ── 2. Handle Call Completed (called by AI after the call ends) ────────

        public async Task HandleCallCompletedAsync(VoiceCallCompletedDTO payload)
        {
            // Find the Interaction (must exist — created at call start)
            Interaction? interaction = null;

            if (!string.IsNullOrWhiteSpace(payload.InteractionId))
                interaction = await _unitOfWork.Interactions.GetByIdAsync(payload.InteractionId);

            if (interaction == null)
            {
                Debug.WriteLine($"[CustomerVoiceService] call-completed: interaction '{payload.InteractionId}' not found — storing orphan summary.");
            }

            var callData = payload.CallData;
            var analysis = payload.Analysis;

            // Build + persist CallSummary
            var summary = new CallSummary
            {
                Id             = Guid.NewGuid().ToString(),
                InteractionId  = interaction?.InteractionId,
                BusinessId     = payload.BusinessId ?? interaction?.BusinessId ?? string.Empty,
                CallId         = callData.CallId,
                StartTime      = callData.StartTime,
                EndTime        = callData.EndTime,
                DurationSeconds = callData.DurationSeconds,
                MessagesCount  = callData.MessagesCount,
                FullTranscript = callData.FullTranscript ?? string.Empty,
                MessagesJson   = JsonSerializer.Serialize(callData.Messages),
                AudioFilesJson = JsonSerializer.Serialize(callData.AudioFiles),
                AudioInfoJson  = JsonSerializer.Serialize(callData.AudioInfo),

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

        // ── Helpers ───────────────────────────────────────────────────────────

        private async Task<string> EnsureCustomerAsync(string businessId, string? customerId)
        {
            if (!string.IsNullOrWhiteSpace(customerId))
            {
                var existing = await _unitOfWork.Customers.GetByIdAsync(customerId);
                if (existing != null) return customerId;
            }

            var guest = new Customer
            {
                CustomerId = Guid.NewGuid().ToString(),
                BusinessId = businessId,
                FullName   = "Guest",
                Email      = string.Empty,
                Phone      = string.Empty,
                CreatedAt  = DateTime.UtcNow
            };
            await _unitOfWork.Customers.AddAsync(guest);
            await _unitOfWork.CompleteAsync();
            return guest.CustomerId;
        }

    }
}
