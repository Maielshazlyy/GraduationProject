using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain_layer.Interfaces;
using Domain_layer.Models;
using Service_layer.DTOS.Chatbot;
using Service_layer.Services_Interfaces;

namespace Service_layer.Services
{
    public class BusinessAnalyticsService : IBusinessAnalyticsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BusinessAnalyticsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BusinessAnalyticsDTO> GetBusinessAnalyticsAsync(string businessId)
        {
            var business = await _unitOfWork.Businesses.GetByIdAsync(businessId);
            if (business == null)
                throw new ArgumentException($"Business with id '{businessId}' not found.");

            // Get all related data
            var orders = (await _unitOfWork.Orders.GetByBusinessIdAsync(businessId)).ToList();
            var customers = (await _unitOfWork.Customers.GetByBusinessIdAsync(businessId)).ToList();
            var tickets = (await _unitOfWork.Tickets.GetByBusinessIdAsync(businessId)).ToList();
            var feedbacks = (await _unitOfWork.Feedbacks.GetAllAsync())
                .Where(f => f.Customer != null && f.Customer.BusinessId == businessId).ToList();
            var sentiments = (await _unitOfWork.Sentiments.GetByBusinessIdAsync(businessId)).ToList();
            var interactions = (await _unitOfWork.Interactions.GetByBusinessIdAsync(businessId)).ToList();
            var menuItems = (await _unitOfWork.MenuItems.GetByBusinessIdAsync(businessId)).ToList();

            // Calculate analytics
            var analytics = new BusinessAnalyticsDTO
            {
                BusinessName = business.Name,
                BusinessType = business.Type,

                // Orders
                TotalOrders = orders.Count,
                TotalRevenue = orders.Where(o => o.Status == Domain_layer.enums.OrderStatus.Delivered || o.Status == Domain_layer.enums.OrderStatus.Paid).Sum(o => o.TotalPrice),
                AverageOrderValue = orders.Any() ? orders.Average(o => o.TotalPrice) : 0,
                PendingOrders = orders.Count(o => o.Status == Domain_layer.enums.OrderStatus.Pending),
                CompletedOrders = orders.Count(o => o.Status == Domain_layer.enums.OrderStatus.Delivered || o.Status == Domain_layer.enums.OrderStatus.Paid),

                // Customers
                TotalCustomers = customers.Count,
                NewCustomersLast30Days = customers.Count(c => c.CreatedAt >= DateTime.UtcNow.AddDays(-30)),

                // Tickets
                TotalTickets = tickets.Count,
                OpenTickets = tickets.Count(t => t.Status == "Open"),
                ClosedTickets = tickets.Count(t => t.Status == "Closed"),
                InProgressTickets = tickets.Count(t => t.Status == "In Progress"),
                AverageTicketResolutionTime = CalculateAverageResolutionTime(tickets),

                // Feedback
                TotalFeedbacks = feedbacks.Count,
                AverageRating = feedbacks.Any() ? feedbacks.Average(f => f.Rating) : 0,
                PositiveFeedbacks = feedbacks.Count(f => f.Rating >= 4),
                NegativeFeedbacks = feedbacks.Count(f => f.Rating <= 2),

                // Sentiment
                PositiveSentiments = sentiments.Count(s => s.Label.Equals("Positive", StringComparison.OrdinalIgnoreCase)),
                NegativeSentiments = sentiments.Count(s => s.Label.Equals("Negative", StringComparison.OrdinalIgnoreCase)),
                NeutralSentiments = sentiments.Count(s => s.Label.Equals("Neutral", StringComparison.OrdinalIgnoreCase)),
                AverageSentimentScore = sentiments.Any() ? sentiments.Average(s => s.Score) : 0,

                // Interactions
                TotalInteractions = interactions.Count,
                ActiveInteractions = interactions.Count(i => !i.IsEnded.HasValue || !i.IsEnded.Value),

                // Menu Items
                TotalMenuItems = menuItems.Count,
                AvailableMenuItems = menuItems.Count(m => m.IsAvailable),

                // Recent Activity
                LastOrderDate = orders.Any() ? orders.Max(o => o.CreatedAt) : DateTime.MinValue,
                LastTicketDate = tickets.Any() ? tickets.Max(t => t.CreatedAt) : DateTime.MinValue,
                LastFeedbackDate = feedbacks.Any() ? feedbacks.Max(f => f.CreatedAt) : DateTime.MinValue
            };

            return analytics;
        }

        private double CalculateAverageResolutionTime(List<Ticket> tickets)
        {
            var closedTickets = tickets.Where(t => t.Status == "Closed" && t.ClosedAt.HasValue).ToList();
            if (!closedTickets.Any()) return 0;

            var totalHours = closedTickets
                .Sum(t => (t.ClosedAt!.Value - t.CreatedAt).TotalHours);

            return totalHours / closedTickets.Count;
        }
    }
}

