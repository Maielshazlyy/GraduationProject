using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain_layer.Interfaces;
using Domain_layer.Models;
using Domain_layer.enums;
using Service_layer.DTOS.Chatbot;
using Service_layer.Services_Interfaces;

namespace Service_layer.Services
{
    public class BusinessAnalyticsService : IBusinessAnalyticsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private const int DefaultTopProductsCount = 10;
        private const int DefaultTopRushBuckets = 5;

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
            var feedbacks = (await _unitOfWork.Feedbacks.GetByBusinessIdAsync(businessId)).ToList();
            var sentiments = (await _unitOfWork.Sentiments.GetByBusinessIdAsync(businessId)).ToList();
            var interactions = (await _unitOfWork.Interactions.GetByBusinessIdAsync(businessId)).ToList();
            var menuItems = (await _unitOfWork.MenuItems.GetByBusinessIdAsync(businessId)).ToList();

            var analyses  = (await _unitOfWork.InteractionAnalyses.GetByBusinessIdAsync(businessId)).ToList();
            var topOrderedProducts = await GetTopOrderedProductsAsync(orders, menuItems, DefaultTopProductsCount);
            var rushInsights = BuildRushInsights(orders, interactions, topOrderedProducts);

            // ── Sentiment: prefer AI session-level analyses, fall back to keyword-based ──
            int positiveSentiments, negativeSentiments, neutralSentiments;
            double avgSentimentScore;

            if (analyses.Any())
            {
                positiveSentiments = analyses.Count(a => a.SentimentLabel.Equals("Positive", StringComparison.OrdinalIgnoreCase));
                negativeSentiments = analyses.Count(a => a.SentimentLabel.Equals("Negative", StringComparison.OrdinalIgnoreCase));
                neutralSentiments  = analyses.Count(a => a.SentimentLabel.Equals("Neutral",  StringComparison.OrdinalIgnoreCase));
                avgSentimentScore  = analyses.Average(a => a.SentimentScore);
            }
            else
            {
                positiveSentiments = sentiments.Count(s => s.Label.Equals("Positive", StringComparison.OrdinalIgnoreCase));
                negativeSentiments = sentiments.Count(s => s.Label.Equals("Negative", StringComparison.OrdinalIgnoreCase));
                neutralSentiments  = sentiments.Count(s => s.Label.Equals("Neutral",  StringComparison.OrdinalIgnoreCase));
                avgSentimentScore  = sentiments.Any() ? sentiments.Average(s => s.Score) : 0;
            }

            // Calculate analytics
            var analytics = new BusinessAnalyticsDTO
            {
                BusinessName = business.Name,
                BusinessType = business.Type,

                // Orders
                TotalOrders = orders.Count,
                TotalRevenue = orders.Where(o => o.Status == OrderStatus.Delivered || o.Status == OrderStatus.Paid).Sum(o => o.TotalPrice),
                AverageOrderValue = orders.Any() ? orders.Average(o => o.TotalPrice) : 0,
                PendingOrders = orders.Count(o => o.Status == OrderStatus.Pending),
                CompletedOrders = orders.Count(o => o.Status == OrderStatus.Delivered || o.Status == OrderStatus.Paid),

                // Customers
                TotalCustomers = customers.Count,
                NewCustomersLast30Days = customers.Count(c => c.CreatedAt >= DateTime.UtcNow.AddDays(-30)),

                // Tickets
                TotalTickets = tickets.Count,
                OpenTickets = tickets.Count(t => t.Status == "Open"),
                ClosedTickets = tickets.Count(t => t.Status == "Closed"),
                InProgressTickets = tickets.Count(t => t.Status == "In Progress"),
                AverageTicketResolutionTime = CalculateAverageResolutionTime(tickets),

                // Feedback — now loaded directly by businessId (no more GetAllAsync)
                TotalFeedbacks = feedbacks.Count,
                AverageRating = feedbacks.Any() ? feedbacks.Average(f => f.Rating) : 0,
                PositiveFeedbacks = feedbacks.Count(f => f.Rating >= 4),
                NegativeFeedbacks = feedbacks.Count(f => f.Rating <= 2),

                // Sentiment — AI analyses preferred, keyword fallback
                PositiveSentiments    = positiveSentiments,
                NegativeSentiments    = negativeSentiments,
                NeutralSentiments     = neutralSentiments,
                AverageSentimentScore = avgSentimentScore,

                // Interactions
                TotalInteractions = interactions.Count,
                ActiveInteractions = interactions.Count(i => !i.IsEnded.HasValue || !i.IsEnded.Value),

                // Menu Items
                TotalMenuItems = menuItems.Count,
                AvailableMenuItems = menuItems.Count(m => m.IsAvailable),

                // Top products (dashboard)
                TopOrderedProducts = topOrderedProducts,

                // Rush insights (orders + calls)
                RushInsights = rushInsights,

                // Recent Activity
                LastOrderDate = orders.Any() ? orders.Max(o => o.CreatedAt) : DateTime.MinValue,
                LastTicketDate = tickets.Any() ? tickets.Max(t => t.CreatedAt) : DateTime.MinValue,
                LastFeedbackDate = feedbacks.Any() ? feedbacks.Max(f => f.CreatedAt) : DateTime.MinValue
            };

            return analytics;
        }

        private RushInsightsDTO BuildRushInsights(List<Order> orders, List<Interaction> interactions, List<TopOrderedProductDTO> topOrderedProducts)
        {
            // Window used for "rush" computation. Adjust later if you want separate windows per granularity.
            var toUtc = DateTime.UtcNow;
            var fromUtc = toUtc.AddDays(-90);

            var windowOrders = orders.Where(o => o.CreatedAt >= fromUtc && o.CreatedAt <= toUtc).ToList();
            var voiceCalls = interactions
                .Where(i => i.StartedAt >= fromUtc && i.StartedAt <= toUtc)
                .Where(i => string.Equals(i.Channel, "Voice", StringComparison.OrdinalIgnoreCase))
                .ToList();

            // callsWeight: each call consumes time but less than a full order fulfillment; tune as needed.
            const double callsWeight = 0.7;

            List<RushBucketDTO> TakeTop(IEnumerable<RushBucketDTO> buckets, int take = DefaultTopRushBuckets)
                => buckets.OrderByDescending(b => b.LoadScore).ThenByDescending(b => b.OrdersCount).Take(take).ToList();

            var peakHours = TakeTop(
                Enumerable.Range(0, 24).Select(h =>
                {
                    var oCount = windowOrders.Count(o => o.CreatedAt.Hour == h);
                    var cCount = voiceCalls.Count(c => c.StartedAt.Hour == h);
                    return new RushBucketDTO
                    {
                        Key = h.ToString(),
                        Label = $"{h:00}:00–{h:00}:59",
                        OrdersCount = oCount,
                        CallsCount = cCount,
                        LoadScore = oCount + (cCount * callsWeight)
                    };
                })
            );

            // Monday=1..Sunday=7 for display stability
            int DayKey(DayOfWeek d) => d == DayOfWeek.Sunday ? 7 : (int)d;
            string DayLabel(DayOfWeek d) => d.ToString();

            var peakDays = TakeTop(
                Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>().Select(d =>
                {
                    var oCount = windowOrders.Count(o => o.CreatedAt.DayOfWeek == d);
                    var cCount = voiceCalls.Count(c => c.StartedAt.DayOfWeek == d);
                    return new RushBucketDTO
                    {
                        Key = DayKey(d).ToString(),
                        Label = DayLabel(d),
                        OrdersCount = oCount,
                        CallsCount = cCount,
                        LoadScore = oCount + (cCount * callsWeight)
                    };
                }).OrderBy(b => int.Parse(b.Key))
            );

            int WeekOfMonth(DateTime dt)
            {
                // 1..5 based on day-of-month
                return ((dt.Day - 1) / 7) + 1;
            }

            var peakWeeksOfMonth = TakeTop(
                Enumerable.Range(1, 5).Select(w =>
                {
                    var oCount = windowOrders.Count(o => WeekOfMonth(o.CreatedAt) == w);
                    var cCount = voiceCalls.Count(c => WeekOfMonth(c.StartedAt) == w);
                    return new RushBucketDTO
                    {
                        Key = $"Week{w}",
                        Label = $"Week {w} of month",
                        OrdersCount = oCount,
                        CallsCount = cCount,
                        LoadScore = oCount + (cCount * callsWeight)
                    };
                })
            );

            var peakMonths = TakeTop(
                Enumerable.Range(1, 12).Select(m =>
                {
                    var oCount = windowOrders.Count(o => o.CreatedAt.Month == m);
                    var cCount = voiceCalls.Count(c => c.StartedAt.Month == m);
                    return new RushBucketDTO
                    {
                        Key = m.ToString(),
                        Label = new DateTime(2000, m, 1).ToString("MMMM"),
                        OrdersCount = oCount,
                        CallsCount = cCount,
                        LoadScore = oCount + (cCount * callsWeight)
                    };
                })
            );

            var hints = new List<string>();
            if (peakHours.Any())
            {
                var top = peakHours.First();
                hints.Add($"Peak hour is {top.Label}. Prepare extra ingredients and schedule more staff around this hour.");
            }
            if (peakDays.Any())
            {
                var top = peakDays.OrderByDescending(x => x.LoadScore).First();
                hints.Add($"Peak day is {top.Label}. Plan inventory and staffing for this day (pre-prep and longer coverage).");
            }
            if (peakWeeksOfMonth.Any())
            {
                var top = peakWeeksOfMonth.First();
                hints.Add($"Peak period inside the month is {top.Label}. Consider increasing purchasing and prep in the days before it.");
            }
            if (peakMonths.Any())
            {
                var top = peakMonths.First();
                hints.Add($"Peak month is {top.Label}. Use it for quarterly planning: suppliers, hiring, and promos timing.");
            }

            if (topOrderedProducts != null && topOrderedProducts.Count > 0)
            {
                var topItems = topOrderedProducts.Take(3).ToList();
                foreach (var item in topItems)
                {
                    if (string.IsNullOrWhiteSpace(item?.Name))
                        continue;

                    // We don't have explicit ingredient tables yet, so we give an actionable inventory/prep hint per item.
                    hints.Add($"High-demand item: {item.Name} (qty {item.TotalQuantity}, orders {item.OrdersCount}). Keep its ingredients stocked and do prep before peak hours to avoid running out.");
                }
            }

            return new RushInsightsDTO
            {
                FromUtc = fromUtc,
                ToUtc = toUtc,
                PeakHoursOfDay = peakHours,
                PeakDaysOfWeek = peakDays,
                PeakWeeksOfMonth = peakWeeksOfMonth,
                PeakMonthsOfYear = peakMonths,
                OwnerActionHints = hints
            };
        }

        private async Task<List<TopOrderedProductDTO>> GetTopOrderedProductsAsync(
            List<Order> orders,
            List<MenuItem> menuItems,
            int count)
        {
            var normalizedCount = count <= 0 ? DefaultTopProductsCount : count;

            // Only consider "successful" orders for product popularity
            var paidStatuses = new[] { OrderStatus.Paid, OrderStatus.Delivered };
            var validOrderIds = orders
                .Where(o => paidStatuses.Contains(o.Status))
                .Select(o => o.OrderId)
                .ToHashSet(StringComparer.Ordinal);

            if (validOrderIds.Count == 0)
            {
                return new List<TopOrderedProductDTO>();
            }

            var allOrderItems = (await _unitOfWork.OrderItems.GetAllAsync()).ToList();
            var businessOrderItems = allOrderItems.Where(oi => validOrderIds.Contains(oi.OrderId)).ToList();

            var menuItemById = menuItems
                .GroupBy(m => m.MenuItemId)
                .ToDictionary(g => g.Key, g => g.First(), StringComparer.Ordinal);

            var ranked = businessOrderItems
                .GroupBy(oi => oi.MenuItemId)
                .Select(g =>
                {
                    var menuItemId = g.Key;
                    menuItemById.TryGetValue(menuItemId, out var menuItem);

                    return new TopOrderedProductDTO
                    {
                        MenuItemId = menuItemId,
                        Name = menuItem?.Name ?? "Unknown Item",
                        TotalQuantity = g.Sum(x => x.Quantity),
                        OrdersCount = g.Select(x => x.OrderId).Distinct(StringComparer.Ordinal).Count(),
                        Revenue = g.Sum(x => x.UnitPrice * x.Quantity)
                    };
                })
                .OrderByDescending(x => x.TotalQuantity)
                .ThenByDescending(x => x.Revenue)
                .Take(normalizedCount)
                .ToList();

            return ranked;
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

