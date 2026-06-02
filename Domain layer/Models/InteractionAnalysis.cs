using System;

namespace Domain_layer.Models
{
    /// <summary>
    /// Stores the AI post-session analysis result for a completed Interaction.
    /// Populated by AiAnalysisService after the interaction is closed.
    /// </summary>
    public class InteractionAnalysis
    {
        public string InteractionAnalysisId { get; set; } = Guid.NewGuid().ToString();

        // ── Foreign keys ──────────────────────────────────────────────────────
        public string InteractionId { get; set; } = string.Empty;
        public Interaction Interaction { get; set; } = null!;

        public string BusinessId { get; set; } = string.Empty;
        public Business Business { get; set; } = null!;

        // ── Summaries ─────────────────────────────────────────────────────────
        public string? Summary { get; set; }
        public string? SummaryAr { get; set; }

        // ── Sentiment ─────────────────────────────────────────────────────────
        /// <summary>Range: -1.0 to +1.0</summary>
        public double SentimentScore { get; set; } = 0.0;

        /// <summary>Positive | Neutral | Negative</summary>
        public string SentimentLabel { get; set; } = "Neutral";

        // ── Intents / Topics / Key moments (stored as JSON) ───────────────────
        /// <summary>Dominant intent for the session (always equals IntentsDetected[0].Name). Never null.</summary>
        public string MainIntent { get; set; } = "Unknown";

        /// <summary>JSON: [{ "name": "CreateOrder", "count": 1 }, ...]</summary>
        public string? IntentsDetectedJson { get; set; }

        /// <summary>JSON: ["Burger", "Food Order"]</summary>
        public string? MainTopicsJson { get; set; }

        /// <summary>JSON: ["Customer asked about burger options", ...]</summary>
        public string? KeyMomentsJson { get; set; }

        // ── Metadata ──────────────────────────────────────────────────────────
        public DateTime AnalyzedAt { get; set; } = DateTime.UtcNow;
    }
}
