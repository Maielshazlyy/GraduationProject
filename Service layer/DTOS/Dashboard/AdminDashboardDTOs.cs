namespace Service_layer.DTOS.Dashboard
{
    public class AdminDashboardSummaryDTO
    {
        // Platform overview
        public int TotalBusinesses { get; set; }
        public int ActiveBusinesses { get; set; }
        public int NewBusinessesLast30Days { get; set; }
        public int TotalUsers { get; set; }
        public int TotalCalls { get; set; }

        // Customer satisfaction: percentage of analyzed messages with a "Positive" sentiment label.
        public double CustomerSatisfactionRate { get; set; }
        public List<AdminCategoryCountDTO> TopBusinessTypes { get; set; } = new();
        public List<AdminCategoryCountDTO> TopCuisineTypes { get; set; } = new();

        // Core business activity
        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public int PendingOrders { get; set; }
        public int CompletedOrders { get; set; }
        public int CancelledOrders { get; set; }

        // Support and engagement
        public int TotalTickets { get; set; }
        public int OpenTickets { get; set; }
        public int EscalatedTickets { get; set; }
        public int TotalInteractions { get; set; }
        public int ActiveInteractions { get; set; }

        // Experience signals
        public int TotalFeedbacks { get; set; }
        public double AverageRating { get; set; }
        public int PositiveSentiments { get; set; }
        public int NegativeSentiments { get; set; }
        public int NeutralSentiments { get; set; }
        public double AverageSentimentScore { get; set; }

        // Audit activity
        public int TotalAuditLogs { get; set; }
        public int AuditLogsLast24Hours { get; set; }
        public DateTime? LastAuditLogDate { get; set; }

        // Recency indicators
        public DateTime? LastOrderDate { get; set; }
        public DateTime? LastTicketDate { get; set; }
        public DateTime? LastFeedbackDate { get; set; }
    }

    public class AdminCategoryCountDTO
    {
        public string Category { get; set; } = string.Empty;
        public int Count { get; set; }
    }

    public class AdminBusinessSnapshotDTO
    {
        public string BusinessId { get; set; } = string.Empty;
        public string BusinessName { get; set; } = string.Empty;
        public bool IsActive { get; set; }

        public int OrdersCount { get; set; }
        public decimal Revenue { get; set; }
        public int OpenTicketsCount { get; set; }
        public int CustomersCount { get; set; }
    }

    public class AdminDashboardFullDTO
    {
        public AdminDashboardSummaryDTO Summary { get; set; } = new();
        public List<AdminBusinessSnapshotDTO> TopBusinessesByRevenue { get; set; } = new();
    }

    public class AdminAlertDTO
    {
        public string Id { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Severity { get; set; } = string.Empty; // Critical, High, Medium, Low
        public string BusinessId { get; set; } = string.Empty;
        public string BusinessName { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }

    public class AdminRevenueTrendPointDTO
    {
        public string Period { get; set; } = string.Empty; // yyyy-MM
        public decimal Revenue { get; set; }
        public int OrdersCount { get; set; }
    }

    public class AdminStatusCountDTO
    {
        public string Status { get; set; } = string.Empty;
        public int Count { get; set; }
    }

    public class AdminPriorityCountDTO
    {
        public string Priority { get; set; } = string.Empty;
        public int Count { get; set; }
    }

    public class AdminBusinessHealthDTO
    {
        public string BusinessId { get; set; } = string.Empty;
        public string BusinessName { get; set; } = string.Empty;
        public int HealthScore { get; set; } // 0 - 100

        public double AverageRating { get; set; }
        public double NegativeSentimentRatio { get; set; } // 0 - 1
        public double CancellationRate { get; set; } // 0 - 1
        public int OpenTicketsCount { get; set; }
        public int EscalatedTicketsCount { get; set; }
    }

    public class AdminSentimentTrendPointDTO
    {
        public string Date { get; set; } = string.Empty; // yyyy-MM-dd
        public int Positive { get; set; }
        public int Negative { get; set; }
        public int Neutral { get; set; }
    }
}
