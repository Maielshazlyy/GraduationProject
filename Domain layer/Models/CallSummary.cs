namespace Domain_layer.Models
{
    public class CallSummary
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        // --- Call Data ---
        public string CallId { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public double DurationSeconds { get; set; }
        public int MessagesCount { get; set; }
        public string FullTranscript { get; set; } = string.Empty;

        // Stored as JSON
        public string MessagesJson { get; set; } = "[]";
        public string AudioFilesJson { get; set; } = "{}";
        public string AudioInfoJson { get; set; } = "{}";

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
        public string ModelsUsedJson { get; set; } = "[]";

        public bool EscalationRequired { get; set; }
        public string? EscalationReason { get; set; }

        public DateTime AnalyzedAt { get; set; }
        public DateTime QueuedAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
