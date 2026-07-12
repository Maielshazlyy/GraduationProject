namespace Service_layer.DTOS.Dashboard
{
    /// <summary>
    /// Per-section period filters. Each accepts: "today" | "7d" | "30d" | "all" | "custom".
    /// Null falls back to the global default ("30d"). "all" = all time (no date filter).
    /// "custom" uses <see cref="CustomFrom"/> / <see cref="CustomTo"/> (required when any section is "custom").
    /// </summary>
    public class DashboardFilterDTO
    {
        public string? InsightsPeriod { get; set; }
        public string? ChannelsPeriod { get; set; }
        public string? SentimentPeriod { get; set; }
        public string? RevenuePeriod { get; set; }
        public string? ProductsPeriod { get; set; }
        public string? LeadsPeriod { get; set; }
        public string? ChatAnalysisPeriod { get; set; }
        public string? FeedbacksPeriod { get; set; }

        /// <summary>Start of the custom window (inclusive, UTC). Used by any section set to "custom".</summary>
        public DateTime? CustomFrom { get; set; }
        /// <summary>End of the custom window (inclusive, UTC). Used by any section set to "custom".</summary>
        public DateTime? CustomTo { get; set; }
    }

    public class FullDashboardDTO
    {
        /// <summary>Echoes the period applied to each section so the frontend can label the data.</summary>
        public AppliedPeriodsDTO AppliedPeriods { get; set; } = new();
        public GeneralInsightsDTO GeneralInsights { get; set; } = new();
        public ChannelDistributionDTO ChannelDistribution { get; set; } = new();
        public SentimentAnalysisDTO SentimentAnalysis { get; set; } = new();
        public List<RevenueTrendPointDTO> RevenueTrend { get; set; } = new();
        public List<TopProductDashboardDTO> TopProducts { get; set; } = new();
        public List<AlertInsightDTO> RecentAlerts { get; set; } = new();
        public List<CustomerLeadDTO> CustomerLeads { get; set; } = new();

        /// <summary>AI post-session analysis results: intents, topics, sentiment trends.</summary>
        public ChatAnalysisDTO ChatAnalysis { get; set; } = new();

        /// <summary>Customer feedback ratings summary.</summary>
        public FeedbackSummaryDTO FeedbackSummary { get; set; } = new();
    }

    // ── Chat Analysis Section ─────────────────────────────────────────────────

    public class ChatAnalysisDTO
    {
        /// <summary>Total number of sessions that have been analyzed by the AI.</summary>
        public int TotalAnalyzedSessions { get; set; }

        /// <summary>Average AI sentiment score across all analyzed sessions (-1.0 to +1.0).</summary>
        public double AvgSentimentScore { get; set; }

        /// <summary>Overall sentiment label derived from the average score.</summary>
        public string OverallSentimentLabel { get; set; } = "Neutral";

        /// <summary>Intent distribution across all analyzed sessions, sorted by count.</summary>
        public List<IntentDistributionDTO> TopIntents { get; set; } = new();

        /// <summary>Most frequently mentioned topics across all analyzed sessions.</summary>
        public List<TopicCountDTO> TopTopics { get; set; } = new();

        /// <summary>Aggregated key moments collected across all analyzed sessions (max 10).</summary>
        public List<string> TopKeyMoments { get; set; } = new();

        /// <summary>Most recent analyzed sessions with their summaries.</summary>
        public List<RecentSessionInsightDTO> RecentSessions { get; set; } = new();
    }

    public class IntentDistributionDTO
    {
        public string Intent { get; set; } = string.Empty;
        public int Count { get; set; }
        public double Percentage { get; set; }
    }

    public class TopicCountDTO
    {
        public string Topic { get; set; } = string.Empty;
        public int Count { get; set; }
    }

    public class RecentSessionInsightDTO
    {
        public string InteractionId { get; set; } = string.Empty;
        public string? CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public string MainIntent { get; set; } = "Unknown";
        public string? Summary { get; set; }
        public string? SummaryAr { get; set; }
        public string SentimentLabel { get; set; } = "Neutral";
        public double SentimentScore { get; set; }
        public DateTime AnalyzedAt { get; set; }
    }

    public class AppliedPeriodsDTO
    {
        public string Insights { get; set; } = "30d";
        public string Channels { get; set; } = "30d";
        public string Sentiment { get; set; } = "30d";
        public string Revenue { get; set; } = "30d";
        public string Products { get; set; } = "30d";
        public string Leads { get; set; } = "30d";
        public string ChatAnalysis { get; set; } = "30d";
        public string Feedbacks { get; set; } = "30d";

        /// <summary>Echoes the custom window when any section uses "custom" (UTC); null otherwise.</summary>
        public DateTime? CustomFrom { get; set; }
        public DateTime? CustomTo { get; set; }
    }

    // ── Feedback Summary Section ──────────────────────────────────────────────

    public class FeedbackSummaryDTO
    {
        public int Total { get; set; }
        /// <summary>Average rating on a 1–5 scale.</summary>
        public double AverageRating { get; set; }
        /// <summary>Rating >= 4</summary>
        public int PositiveCount { get; set; }
        /// <summary>Rating == 3</summary>
        public int NeutralCount { get; set; }
        /// <summary>Rating <= 2</summary>
        public int NegativeCount { get; set; }
        public double PositivePercentage { get; set; }
        public double NeutralPercentage { get; set; }
        public double NegativePercentage { get; set; }
    }

    public class GeneralInsightsDTO
    {
        public MetricWithChangeDTO TotalRevenue { get; set; } = new();
        public MetricWithChangeDTO AvgResolutionTimeHours { get; set; } = new();
        public MetricWithChangeDTO TotalInteractions { get; set; } = new();
        public MetricWithChangeDTO AvgOrderValue { get; set; } = new();
        public MetricWithChangeDTO NewCustomers { get; set; } = new();
        public MetricWithChangeDTO OpenTickets { get; set; } = new();
    }

    public class MetricWithChangeDTO
    {
        public double Value { get; set; }
        public double PreviousValue { get; set; }
        /// <summary>Positive = increase, negative = decrease. Null if no previous data.</summary>
        public double? ChangePercentage { get; set; }
        /// <summary>"up", "down", or "neutral"</summary>
        public string Trend { get; set; } = "neutral";
    }

    public class ChannelDistributionDTO
    {
        public int Total { get; set; }
        public List<ChannelCountDTO> Channels { get; set; } = new();
    }

    public class ChannelCountDTO
    {
        public string Channel { get; set; } = string.Empty;
        public int Count { get; set; }
        public double Percentage { get; set; }
    }

    public class SentimentAnalysisDTO
    {
        public int Satisfied { get; set; }
        public int Neutral { get; set; }
        public int Angry { get; set; }
        public int Total { get; set; }
        public double SatisfiedPercentage { get; set; }
        public double NeutralPercentage { get; set; }
        public double AngryPercentage { get; set; }
    }

    public class RevenueTrendPointDTO
    {
        public string Label { get; set; } = string.Empty;
        public decimal Revenue { get; set; }
        public int OrderCount { get; set; }
    }

    public class TopProductDashboardDTO
    {
        public string MenuItemId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public int TotalUnitsSold { get; set; }
        public decimal Revenue { get; set; }
        public int OrdersCount { get; set; }
    }

    public class AlertInsightDTO
    {
        public string Id { get; set; } = string.Empty;
        /// <summary>e.g. LateDelivery, WrongOrder, Escalation, HighPriority</summary>
        public string Type { get; set; } = string.Empty;
        /// <summary>PriorityLevel: Low, Normal, High, Critical</summary>
        public string Category { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string Severity { get; set; } = "Normal";
    }

    public class CustomerLeadDTO
    {
        public string CustomerId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        /// <summary>VIP (10+ orders), Regular (3–9), New (1–2), Potential (0)</summary>
        public string Tag { get; set; } = string.Empty;
        public decimal TotalSpend { get; set; }
        public int TotalOrders { get; set; }
        public DateTime LastActivity { get; set; }
    }
}
