namespace Domain_layer.Models
{
    public class CallSummary
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        // --- Links ---
        /// <summary>The Interaction this call belongs to.</summary>
        public string? InteractionId { get; set; }
        public Interaction? Interaction { get; set; }

        /// <summary>The Business this call belongs to.</summary>
        public string BusinessId { get; set; } = string.Empty;
        public Business? Business { get; set; }

        // --- Call Data ---
        public string CallId { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public double DurationSeconds { get; set; }
        public int MessagesCount { get; set; }
        public string FullTranscript { get; set; } = string.Empty;

        // Stored as JSON
        public string MessagesJson { get; set; } = "[]";
        public string AudioInfoJson { get; set; } = "{}";

        /// <summary>Download URLs returned by /api/CallRecording/upload (customer, agent, stereo).</summary>
        public string AudioUrlsJson { get; set; } = "{}";

        // --- Analysis ---
        public string Summary { get; set; } = string.Empty;
        public string SummaryAr { get; set; } = string.Empty;
        public double SentimentScore { get; set; }
        public string SentimentLabel { get; set; } = string.Empty;

        // Stored as JSON arrays / objects
        public string MainTopicsJson { get; set; } = "[]";
        public string IntentsDetectedJson { get; set; } = "[]";
        public string ActionsPerformedJson { get; set; } = "[]";
        public string KeyMomentsJson { get; set; } = "[]";

        public bool EscalationRequired { get; set; }
        public string? EscalationReason { get; set; }

        public DateTime AnalyzedAt { get; set; }
        public DateTime QueuedAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
