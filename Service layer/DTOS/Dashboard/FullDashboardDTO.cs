namespace Service_layer.DTOS.Dashboard
{
    /// <summary>
    /// Per-section period filters. Each accepts: "today" | "7d" | "30d" | "all".
    /// Null falls back to the global default ("30d"). "all" = all time (no date filter).
    /// </summary>
    public class DashboardFilterDTO
    {
        public string? InsightsPeriod { get; set; }
        public string? ChannelsPeriod { get; set; }
        public string? SentimentPeriod { get; set; }
        public string? RevenuePeriod { get; set; }
        public string? ProductsPeriod { get; set; }
        public string? LeadsPeriod { get; set; }
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
    }

    public class AppliedPeriodsDTO
    {
        public string Insights { get; set; } = "30d";
        public string Channels { get; set; } = "30d";
        public string Sentiment { get; set; } = "30d";
        public string Revenue { get; set; } = "30d";
        public string Products { get; set; } = "30d";
        public string Leads { get; set; } = "30d";
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
