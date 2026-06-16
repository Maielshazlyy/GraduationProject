using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Domain_layer.Interfaces;
using Domain_layer.Models;
using Service_layer.DTOS.AiAnalysis;
using Service_layer.Services_Interfaces;

namespace Service_layer.Services
{
    /// <summary>
    /// Fetches a completed interaction's messages from the DB,
    /// sends them to the AI analysis pipeline,
    /// and returns the structured analysis result.
    ///
    /// Called post-session only — never during a live conversation.
    /// </summary>
    public class AiAnalysisService : IAiAnalysisService
    {
        private readonly HttpClient _httpClient;
        private readonly IUnitOfWork _unitOfWork;

        public AiAnalysisService(HttpClient httpClient, IUnitOfWork unitOfWork)
        {
            _httpClient = httpClient;
            _unitOfWork = unitOfWork;
        }

        public async Task<AiSessionAnalysisResultDTO?> AnalyzeSessionAsync(string interactionId)
        {
            // ── 1) Load interaction ───────────────────────────────────────────
            var interaction = await _unitOfWork.Interactions.GetByIdAsync(interactionId);
            if (interaction == null) return null;

            // ── 2) Load messages ──────────────────────────────────────────────
            var messages = (await _unitOfWork.Messages.GetByInteractionIdAsync(interactionId))
                .OrderBy(m => m.SentAt)
                .ToList();

            if (!messages.Any()) return null;

            // ── 3) Map SenderType → role ──────────────────────────────────────
            // DB values : "Customer" | "AI" | "Agent"
            // Contract  : "customer" | "assistant"
            // Skip blank messages — AI rejects empty text fields.
            var mappedMessages = messages
                .Where(m => !string.IsNullOrWhiteSpace(m.Content))
                .Select(m => new AiAnalysisMessageDTO
                {
                    Role = m.SenderType.Equals("Customer", StringComparison.OrdinalIgnoreCase)
                               ? "customer"
                               : "assistant",
                    Text = m.Content
                })
                .ToList();

            if (!mappedMessages.Any()) return null;

            // ── 4) Build request ──────────────────────────────────────────────
            var request = new AiAnalysisRequestDTO
            {
                BusinessId = interaction.BusinessId,
                Sessions   = new()
                {
                    new AiAnalysisSessionDTO
                    {
                        SessionId = interactionId,
                        Messages  = mappedMessages
                    }
                }
            };

            // ── 5) Call AI ────────────────────────────────────────────────────
            var response = await _httpClient.PostAsJsonAsync("/api/v1/analysis/chat-batch", request);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<AiAnalysisResponseDTO>();
            var sessionResult = result?.Results.FirstOrDefault(r => r.SessionId == interactionId);

            if (sessionResult != null)
                await StoreAnalysisAsync(interaction, sessionResult);

            return sessionResult;
        }

        // ── Storage ───────────────────────────────────────────────────────────

        private async Task StoreAnalysisAsync(
            Domain_layer.Models.Interaction interaction,
            AiSessionAnalysisResultDTO result)
        {
            // Guard: skip if this interaction was already analyzed (e.g. retry or duplicate trigger).
            var existing = await _unitOfWork.InteractionAnalyses
                                            .GetByInteractionIdAsync(interaction.InteractionId);
            if (existing != null) return;

            var analysis = new InteractionAnalysis
            {
                InteractionAnalysisId = Guid.NewGuid().ToString(),
                InteractionId         = interaction.InteractionId,
                BusinessId            = interaction.BusinessId,
                Summary               = result.Summary,
                SummaryAr             = result.SummaryAr,
                SentimentScore        = result.OverallSentiment?.Score ?? 0.0,
                SentimentLabel        = result.OverallSentiment?.Label ?? "Neutral",
                MainIntent            = result.MainIntent,
                IntentsDetectedJson   = JsonSerializer.Serialize(result.IntentsDetected),
                MainTopicsJson        = JsonSerializer.Serialize(result.MainTopics),
                KeyMomentsJson        = JsonSerializer.Serialize(result.KeyMoments),
                AnalyzedAt            = DateTime.UtcNow
            };

            await _unitOfWork.InteractionAnalyses.AddAsync(analysis);
            await _unitOfWork.CompleteAsync();
        }
    }
}
