namespace Service_layer.DTOS.CallSummary
{
    public class CallSummaryResponseDTO
    {
        public string Id { get; set; } = string.Empty;
        public string? InteractionId { get; set; }
        public string BusinessId { get; set; } = string.Empty;
        public string CallId { get; set; } = string.Empty;

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public double DurationSeconds { get; set; }
        public int MessagesCount { get; set; }
        public string FullTranscript { get; set; } = string.Empty;

        public string Summary { get; set; } = string.Empty;
        public string SummaryAr { get; set; } = string.Empty;
        public double SentimentScore { get; set; }
        public string SentimentLabel { get; set; } = string.Empty;

        public bool EscalationRequired { get; set; }
        public string? EscalationReason { get; set; }

        public DateTime AnalyzedAt { get; set; }
        public DateTime CreatedAt { get; set; }

        /// <summary>Download URLs for the call recordings.</summary>
        public AudioUrlsResponseDTO? AudioUrls { get; set; }
    }

    public class AudioUrlsResponseDTO
    {
        public string? Customer { get; set; }
        public string? Agent { get; set; }
        public string? Stereo { get; set; }
    }
}
