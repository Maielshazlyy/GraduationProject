namespace Service_layer.DTOS.Chatbot
{
    public class RushBucketDTO
    {
        /// <summary>
        /// Bucket key (e.g., "13" for hour 1PM, "Mon", "Week2", "Q2-M04").
        /// </summary>
        public string Key { get; set; } = string.Empty;

        /// <summary>
        /// Human-readable label (e.g., "13:00–13:59", "Monday").
        /// </summary>
        public string Label { get; set; } = string.Empty;

        public int OrdersCount { get; set; }
        public int CallsCount { get; set; }

        /// <summary>
        /// Combined load score (orders + calls) used for ranking.
        /// </summary>
        public double LoadScore { get; set; }
    }

    public class RushInsightsDTO
    {
        /// <summary>
        /// Date range used to compute rush buckets (UTC).
        /// </summary>
        public DateTime FromUtc { get; set; }
        public DateTime ToUtc { get; set; }

        /// <summary>
        /// Top peak hours across the selected range.
        /// </summary>
        public List<RushBucketDTO> PeakHoursOfDay { get; set; } = new();

        /// <summary>
        /// Top peak days across the selected range (Mon..Sun).
        /// </summary>
        public List<RushBucketDTO> PeakDaysOfWeek { get; set; } = new();

        /// <summary>
        /// Top peak weeks within a month (Week1..Week5) across the selected range.
        /// </summary>
        public List<RushBucketDTO> PeakWeeksOfMonth { get; set; } = new();

        /// <summary>
        /// Top peak months within the year/quarter window across the selected range.
        /// </summary>
        public List<RushBucketDTO> PeakMonthsOfYear { get; set; } = new();

        /// <summary>
        /// Simple action hints for the business owner derived from peaks.
        /// </summary>
        public List<string> OwnerActionHints { get; set; } = new();
    }

    public class TopOrderedProductDTO
    {
        public string MenuItemId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int TotalQuantity { get; set; }
        public int OrdersCount { get; set; }
        public decimal Revenue { get; set; }
    }

    public class BusinessAnalyticsDTO
    {
        // Business Info
        public string BusinessName { get; set; } = string.Empty;
        public string BusinessType { get; set; } = string.Empty;

        // Orders Analytics
        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal AverageOrderValue { get; set; }
        public int PendingOrders { get; set; }
        public int CompletedOrders { get; set; }

        // Customers Analytics
        public int TotalCustomers { get; set; }
        public int NewCustomersLast30Days { get; set; }

        // Tickets Analytics
        public int TotalTickets { get; set; }
        public int OpenTickets { get; set; }
        public int ClosedTickets { get; set; }
        public int InProgressTickets { get; set; }
        public double AverageTicketResolutionTime { get; set; } // in hours

        // Feedback Analytics
        public double AverageRating { get; set; }
        public int TotalFeedbacks { get; set; }
        public int PositiveFeedbacks { get; set; } // Rating >= 4
        public int NegativeFeedbacks { get; set; } // Rating <= 2

        // Sentiment Analytics
        public int PositiveSentiments { get; set; }
        public int NegativeSentiments { get; set; }
        public int NeutralSentiments { get; set; }
        public double AverageSentimentScore { get; set; }

        // Interactions Analytics
        public int TotalInteractions { get; set; }
        public int ActiveInteractions { get; set; }

        // Menu Items Analytics
        public int TotalMenuItems { get; set; }
        public int AvailableMenuItems { get; set; }

        // Top ordered products with details (for dashboard)
        public List<TopOrderedProductDTO> TopOrderedProducts { get; set; } = new List<TopOrderedProductDTO>();

        // Rush-time insights (orders + calls)
        public RushInsightsDTO RushInsights { get; set; } = new RushInsightsDTO();

        // Recent Activity
        public DateTime LastOrderDate { get; set; }
        public DateTime LastTicketDate { get; set; }
        public DateTime LastFeedbackDate { get; set; }
    }
}

