namespace Service_layer.DTOS.Chatbot
{
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

        // Top Selling Items (simplified - can be enhanced)
        public List<string> TopSellingItems { get; set; } = new List<string>();

        // Recent Activity
        public DateTime LastOrderDate { get; set; }
        public DateTime LastTicketDate { get; set; }
        public DateTime LastFeedbackDate { get; set; }
    }
}

