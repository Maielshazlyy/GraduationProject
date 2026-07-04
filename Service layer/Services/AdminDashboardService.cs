using Domain_layer.enums;
using Domain_layer.Interfaces;
using Service_layer.DTOS.Dashboard;
using Service_layer.Services_Interfaces;

namespace Service_layer.Services
{
    public class AdminDashboardService : IAdminDashboardService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuditLogService _auditLogService;
        private readonly IUserService _userService;

        public AdminDashboardService(IUnitOfWork unitOfWork, IAuditLogService auditLogService, IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _auditLogService = auditLogService;
            _userService = userService;
        }

        public async Task<AdminDashboardSummaryDTO> GetSummaryAsync()
        {
            var businesses = (await _unitOfWork.Businesses.GetAllAsync()).ToList();
            var orders = (await _unitOfWork.Orders.GetAllAsync()).ToList();
            var tickets = (await _unitOfWork.Tickets.GetAllAsync()).ToList();
            var interactions = (await _unitOfWork.Interactions.GetAllAsync()).ToList();
            var feedbacks = (await _unitOfWork.Feedbacks.GetAllAsync()).ToList();
            var sentiments = (await _unitOfWork.Sentiments.GetAllAsync()).ToList();
            var auditLogs = (await _unitOfWork.AuditLogs.GetAllAsync()).ToList();
            var users = (await _userService.GetAllAsync()).ToList();
            var calls = (await _unitOfWork.CallSummaries.GetAllAsync()).ToList();

            var now = DateTime.UtcNow;

            return new AdminDashboardSummaryDTO
            {
                TotalBusinesses = businesses.Count,
                ActiveBusinesses = businesses.Count(b => b.IsActive),
                NewBusinessesLast30Days = businesses.Count(b => b.CreatedAt >= now.AddDays(-30)),
                TotalUsers = users.Count,
                TotalCalls = calls.Count,

                CustomerSatisfactionRate = sentiments.Any()
                    ? Math.Round(sentiments.Count(s => s.Label.Equals("Positive", StringComparison.OrdinalIgnoreCase)) / (double)sentiments.Count * 100, 1)
                    : 0,
                TopBusinessTypes = businesses
                    .Where(b => !string.IsNullOrWhiteSpace(b.Type))
                    .GroupBy(b => b.Type)
                    .Select(g => new AdminCategoryCountDTO { Category = g.Key, Count = g.Count() })
                    .OrderByDescending(x => x.Count)
                    .Take(10)
                    .ToList(),
                TopCuisineTypes = businesses
                    .Where(b => !string.IsNullOrWhiteSpace(b.CuisineType))
                    .GroupBy(b => b.CuisineType!)
                    .Select(g => new AdminCategoryCountDTO { Category = g.Key, Count = g.Count() })
                    .OrderByDescending(x => x.Count)
                    .Take(10)
                    .ToList(),

                TotalOrders = orders.Count,
                TotalRevenue = orders
                    .Where(o => o.Status == OrderStatus.Delivered || o.Status == OrderStatus.Paid)
                    .Sum(o => o.TotalPrice),
                PendingOrders = orders.Count(o => o.Status == OrderStatus.Pending || o.Status == OrderStatus.PendingConfirmation),
                CompletedOrders = orders.Count(o => o.Status == OrderStatus.Delivered || o.Status == OrderStatus.Paid),
                CancelledOrders = orders.Count(o => o.Status == OrderStatus.Cancelled),

                TotalTickets = tickets.Count,
                OpenTickets = tickets.Count(t => string.Equals(t.Status, "Open", StringComparison.OrdinalIgnoreCase)),
                EscalatedTickets = tickets.Count(t =>
                    string.Equals(t.Status, "Escalated", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(t.TicketType, "HumanEscalation", StringComparison.OrdinalIgnoreCase)),
                TotalInteractions = interactions.Count,
                ActiveInteractions = interactions.Count(i => !i.IsEnded.HasValue || !i.IsEnded.Value),

                TotalFeedbacks = feedbacks.Count,
                AverageRating = feedbacks.Any() ? feedbacks.Average(f => f.Rating) : 0,
                PositiveSentiments = sentiments.Count(s => s.Label.Equals("Positive", StringComparison.OrdinalIgnoreCase)),
                NegativeSentiments = sentiments.Count(s => s.Label.Equals("Negative", StringComparison.OrdinalIgnoreCase)),
                NeutralSentiments = sentiments.Count(s => s.Label.Equals("Neutral", StringComparison.OrdinalIgnoreCase)),
                AverageSentimentScore = sentiments.Any() ? sentiments.Average(s => s.Score) : 0,

                TotalAuditLogs = auditLogs.Count,
                AuditLogsLast24Hours = auditLogs.Count(a => a.CreatedAt >= now.AddDays(-1)),
                LastAuditLogDate = auditLogs.OrderByDescending(a => a.CreatedAt).FirstOrDefault()?.CreatedAt,

                LastOrderDate = orders.Any() ? orders.Max(o => o.CreatedAt) : null,
                LastTicketDate = tickets.Any() ? tickets.Max(t => t.CreatedAt) : null,
                LastFeedbackDate = feedbacks.Any() ? feedbacks.Max(f => f.CreatedAt) : null
            };
        }

        public async Task<List<AdminBusinessSnapshotDTO>> GetTopBusinessesByRevenueAsync(int count = 10)
        {
            var normalizedCount = count <= 0 ? 10 : count;
            var businesses = (await _unitOfWork.Businesses.GetAllAsync()).ToList();
            var orders = (await _unitOfWork.Orders.GetAllAsync()).ToList();
            var tickets = (await _unitOfWork.Tickets.GetAllAsync()).ToList();
            var customers = (await _unitOfWork.Customers.GetAllAsync()).ToList();

            var snapshots = businesses.Select(b =>
            {
                var businessOrders = orders.Where(o => o.BusinessId == b.Id).ToList();
                var businessTickets = tickets.Where(t => t.BusinessId == b.Id).ToList();
                var businessCustomersCount = customers.Count(c => c.BusinessId == b.Id);

                return new AdminBusinessSnapshotDTO
                {
                    BusinessId = b.Id,
                    BusinessName = b.Name,
                    IsActive = b.IsActive,
                    OrdersCount = businessOrders.Count,
                    Revenue = businessOrders
                        .Where(o => o.Status == OrderStatus.Delivered || o.Status == OrderStatus.Paid)
                        .Sum(o => o.TotalPrice),
                    OpenTicketsCount = businessTickets.Count(t => string.Equals(t.Status, "Open", StringComparison.OrdinalIgnoreCase)),
                    CustomersCount = businessCustomersCount
                };
            });

            return snapshots
                .OrderByDescending(s => s.Revenue)
                .ThenByDescending(s => s.OrdersCount)
                .Take(normalizedCount)
                .ToList();
        }

        public async Task<AdminDashboardFullDTO> GetFullDashboardAsync(int topBusinessesCount = 10)
        {
            var summary = await GetSummaryAsync();
            var topBusinesses = await GetTopBusinessesByRevenueAsync(topBusinessesCount);

            return new AdminDashboardFullDTO
            {
                Summary = summary,
                TopBusinessesByRevenue = topBusinesses
            };
        }

        public async Task<List<AdminAlertDTO>> GetAlertsAsync()
        {
            var now = DateTime.UtcNow;
            var businesses = (await _unitOfWork.Businesses.GetAllAsync()).ToList();
            var orders = (await _unitOfWork.Orders.GetAllAsync()).ToList();
            var tickets = (await _unitOfWork.Tickets.GetAllAsync()).ToList();
            var sentiments = (await _unitOfWork.Sentiments.GetAllAsync()).ToList();
            var interactions = (await _unitOfWork.Interactions.GetAllAsync()).ToList();

            var alerts = new List<AdminAlertDTO>();

            foreach (var business in businesses)
            {
                var businessOrders = orders.Where(o => o.BusinessId == business.Id).ToList();
                var businessTickets = tickets.Where(t => t.BusinessId == business.Id).ToList();
                var businessInteractions = interactions.Where(i => i.BusinessId == business.Id).ToList();

                // 1) Inactive business alert (no orders in last 14 days)
                var hasRecentOrders = businessOrders.Any(o => o.CreatedAt >= now.AddDays(-14));
                if (!hasRecentOrders)
                {
                    alerts.Add(new AdminAlertDTO
                    {
                        Id = Guid.NewGuid().ToString(),
                        Type = "InactiveBusiness",
                        Severity = "Medium",
                        BusinessId = business.Id,
                        BusinessName = business.Name,
                        Message = "No orders recorded in the last 14 days.",
                        CreatedAt = now
                    });
                }

                // 2) High open tickets alert
                var openTickets = businessTickets.Count(t => string.Equals(t.Status, "Open", StringComparison.OrdinalIgnoreCase));
                if (openTickets >= 10)
                {
                    alerts.Add(new AdminAlertDTO
                    {
                        Id = Guid.NewGuid().ToString(),
                        Type = "HighOpenTickets",
                        Severity = openTickets >= 20 ? "Critical" : "High",
                        BusinessId = business.Id,
                        BusinessName = business.Name,
                        Message = $"High number of open tickets detected ({openTickets}).",
                        CreatedAt = now
                    });
                }

                // 3) High cancellation rate alert (last 30 days, at least 10 orders)
                var ordersLast30 = businessOrders.Where(o => o.CreatedAt >= now.AddDays(-30)).ToList();
                if (ordersLast30.Count >= 10)
                {
                    var cancelled = ordersLast30.Count(o => o.Status == OrderStatus.Cancelled);
                    var cancellationRate = (double)cancelled / ordersLast30.Count;

                    if (cancellationRate >= 0.25)
                    {
                        alerts.Add(new AdminAlertDTO
                        {
                            Id = Guid.NewGuid().ToString(),
                            Type = "HighCancellationRate",
                            Severity = cancellationRate >= 0.4 ? "Critical" : "High",
                            BusinessId = business.Id,
                            BusinessName = business.Name,
                            Message = $"Cancellation rate is high ({cancellationRate:P0}) in the last 30 days.",
                            CreatedAt = now
                        });
                    }
                }

                // 4) Negative sentiment spike alert (last 7 days)
                var businessInteractionIds = businessInteractions.Select(i => i.InteractionId).ToHashSet();
                var businessSentiments = sentiments
                    .Where(s => s.Message != null
                                && s.Message.InteractionId != null
                                && businessInteractionIds.Contains(s.Message.InteractionId)
                                && s.AnalyzedAt >= now.AddDays(-7))
                    .ToList();

                if (businessSentiments.Count >= 5)
                {
                    var negativeCount = businessSentiments.Count(s =>
                        s.Label.Equals("Negative", StringComparison.OrdinalIgnoreCase));
                    var negativeRatio = (double)negativeCount / businessSentiments.Count;

                    if (negativeRatio >= 0.5)
                    {
                        alerts.Add(new AdminAlertDTO
                        {
                            Id = Guid.NewGuid().ToString(),
                            Type = "HighNegativeSentiment",
                            Severity = negativeRatio >= 0.7 ? "Critical" : "High",
                            BusinessId = business.Id,
                            BusinessName = business.Name,
                            Message = $"Negative sentiment ratio is high ({negativeRatio:P0}) in the last 7 days.",
                            CreatedAt = now
                        });
                    }
                }
            }

            return alerts
                .OrderByDescending(a => SeverityRank(a.Severity))
                .ThenByDescending(a => a.CreatedAt)
                .ToList();
        }

        public async Task<List<AdminRevenueTrendPointDTO>> GetRevenueTrendAsync(int months = 12)
        {
            var normalizedMonths = months <= 0 ? 12 : months;
            var now = DateTime.UtcNow;
            var startMonth = new DateTime(now.Year, now.Month, 1).AddMonths(-(normalizedMonths - 1));

            var orders = (await _unitOfWork.Orders.GetAllAsync())
                .Where(o => o.CreatedAt >= startMonth)
                .ToList();

            var paidStatuses = new[] { OrderStatus.Paid, OrderStatus.Delivered };

            var grouped = orders
                .Where(o => paidStatuses.Contains(o.Status))
                .GroupBy(o => new { o.CreatedAt.Year, o.CreatedAt.Month })
                .ToDictionary(
                    g => $"{g.Key.Year:D4}-{g.Key.Month:D2}",
                    g => new AdminRevenueTrendPointDTO
                    {
                        Period = $"{g.Key.Year:D4}-{g.Key.Month:D2}",
                        Revenue = g.Sum(x => x.TotalPrice),
                        OrdersCount = g.Count()
                    });

            var result = new List<AdminRevenueTrendPointDTO>();
            for (var i = 0; i < normalizedMonths; i++)
            {
                var month = startMonth.AddMonths(i);
                var periodKey = $"{month.Year:D4}-{month.Month:D2}";
                result.Add(grouped.TryGetValue(periodKey, out var point)
                    ? point
                    : new AdminRevenueTrendPointDTO { Period = periodKey, Revenue = 0, OrdersCount = 0 });
            }

            return result;
        }

        public async Task<List<AdminStatusCountDTO>> GetOrdersByStatusAsync()
        {
            var orders = (await _unitOfWork.Orders.GetAllAsync()).ToList();

            return orders
                .GroupBy(o => o.Status.ToString())
                .Select(g => new AdminStatusCountDTO
                {
                    Status = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(x => x.Count)
                .ToList();
        }

        public async Task<List<AdminPriorityCountDTO>> GetTicketsByPriorityAsync()
        {
            var tickets = (await _unitOfWork.Tickets.GetAllAsync()).ToList();

            return tickets
                .GroupBy(t => string.IsNullOrWhiteSpace(t.PriorityLevel) ? "Unknown" : t.PriorityLevel!)
                .Select(g => new AdminPriorityCountDTO
                {
                    Priority = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(x => x.Count)
                .ToList();
        }

        public async Task<List<AdminBusinessHealthDTO>> GetBusinessHealthAsync(int top = 20, bool ascending = false)
        {
            var normalizedTop = top <= 0 ? 20 : top;
            var businesses = (await _unitOfWork.Businesses.GetAllAsync()).ToList();
            var orders = (await _unitOfWork.Orders.GetAllAsync()).ToList();
            var tickets = (await _unitOfWork.Tickets.GetAllAsync()).ToList();
            var feedbacks = (await _unitOfWork.Feedbacks.GetAllAsync()).ToList();
            var sentiments = (await _unitOfWork.Sentiments.GetAllAsync()).ToList();
            var interactions = (await _unitOfWork.Interactions.GetAllAsync()).ToList();

            var health = businesses.Select(b =>
            {
                var businessOrders = orders.Where(o => o.BusinessId == b.Id).ToList();
                var businessTickets = tickets.Where(t => t.BusinessId == b.Id).ToList();
                var businessInteractions = interactions.Where(i => i.BusinessId == b.Id).ToList();
                var interactionIds = businessInteractions.Select(i => i.InteractionId).ToHashSet();

                var businessFeedbacks = feedbacks
                    .Where(f => f.Customer != null && f.Customer.BusinessId == b.Id)
                    .ToList();

                var businessSentiments = sentiments
                    .Where(s => s.Message != null
                                && s.Message.InteractionId != null
                                && interactionIds.Contains(s.Message.InteractionId))
                    .ToList();

                var avgRating = businessFeedbacks.Any() ? businessFeedbacks.Average(f => f.Rating) : 0;
                var openTickets = businessTickets.Count(t => string.Equals(t.Status, "Open", StringComparison.OrdinalIgnoreCase));
                var escalatedTickets = businessTickets.Count(t =>
                    string.Equals(t.Status, "Escalated", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(t.TicketType, "HumanEscalation", StringComparison.OrdinalIgnoreCase));

                var negativeSentimentRatio = businessSentiments.Any()
                    ? (double)businessSentiments.Count(s => s.Label.Equals("Negative", StringComparison.OrdinalIgnoreCase)) / businessSentiments.Count
                    : 0;

                var cancellationRate = businessOrders.Any()
                    ? (double)businessOrders.Count(o => o.Status == OrderStatus.Cancelled) / businessOrders.Count
                    : 0;

                // Weighted score (simple and explainable)
                // Rating (40), Sentiment (25), Cancellation (20), Tickets (15)
                var ratingScore = Math.Min(40, (avgRating / 5.0) * 40.0);
                var sentimentScore = Math.Max(0, 25.0 * (1.0 - negativeSentimentRatio));
                var cancellationScore = Math.Max(0, 20.0 * (1.0 - cancellationRate));
                var ticketPenalty = Math.Min(15.0, (openTickets * 0.75) + (escalatedTickets * 1.5));
                var ticketsScore = Math.Max(0, 15.0 - ticketPenalty);

                var healthScore = (int)Math.Round(ratingScore + sentimentScore + cancellationScore + ticketsScore);
                healthScore = Math.Clamp(healthScore, 0, 100);

                return new AdminBusinessHealthDTO
                {
                    BusinessId = b.Id,
                    BusinessName = b.Name,
                    HealthScore = healthScore,
                    AverageRating = avgRating,
                    NegativeSentimentRatio = negativeSentimentRatio,
                    CancellationRate = cancellationRate,
                    OpenTicketsCount = openTickets,
                    EscalatedTicketsCount = escalatedTickets
                };
            });

            var ordered = ascending
                ? health.OrderBy(h => h.HealthScore).ThenBy(h => h.BusinessName)
                : health.OrderByDescending(h => h.HealthScore).ThenBy(h => h.BusinessName);

            return ordered.Take(normalizedTop).ToList();
        }

        public async Task<List<AdminSentimentTrendPointDTO>> GetSentimentTrendAsync(int days = 30)
        {
            var normalizedDays = days <= 0 ? 30 : days;
            var today = DateTime.UtcNow.Date;
            var startDate = today.AddDays(-(normalizedDays - 1));

            var sentiments = (await _unitOfWork.Sentiments.GetAllAsync())
                .Where(s => s.AnalyzedAt.Date >= startDate)
                .ToList();

            var grouped = sentiments
                .GroupBy(s => s.AnalyzedAt.Date)
                .ToDictionary(
                    g => g.Key,
                    g => new AdminSentimentTrendPointDTO
                    {
                        Date = g.Key.ToString("yyyy-MM-dd"),
                        Positive = g.Count(s => s.Label.Equals("Positive", StringComparison.OrdinalIgnoreCase)),
                        Negative = g.Count(s => s.Label.Equals("Negative", StringComparison.OrdinalIgnoreCase)),
                        Neutral = g.Count(s => s.Label.Equals("Neutral", StringComparison.OrdinalIgnoreCase))
                    });

            var result = new List<AdminSentimentTrendPointDTO>();
            for (var i = 0; i < normalizedDays; i++)
            {
                var date = startDate.AddDays(i);
                result.Add(grouped.TryGetValue(date, out var point)
                    ? point
                    : new AdminSentimentTrendPointDTO
                    {
                        Date = date.ToString("yyyy-MM-dd"),
                        Positive = 0,
                        Negative = 0,
                        Neutral = 0
                    });
            }

            return result;
        }

        public async Task<bool> SetBusinessActiveStateAsync(string businessId, bool isActive, string? adminUserId = null)
        {
            var business = await _unitOfWork.Businesses.GetByIdAsync(businessId);
            if (business == null)
            {
                return false;
            }

            business.IsActive = isActive;
            _unitOfWork.Businesses.Update(business);
            await _unitOfWork.CompleteAsync();

            await _auditLogService.LogBusinessActionAsync(
                businessId: business.Id,
                action: isActive ? "AdminActivateBusiness" : "AdminSuspendBusiness",
                userId: adminUserId);

            return true;
        }

        public async Task<bool> SetBusinessVerificationStateAsync(string businessId, bool isVerified, string? adminUserId = null)
        {
            var business = await _unitOfWork.Businesses.GetByIdAsync(businessId);
            if (business == null)
            {
                return false;
            }

            business.IsVerified = isVerified;
            _unitOfWork.Businesses.Update(business);
            await _unitOfWork.CompleteAsync();

            await _auditLogService.LogBusinessActionAsync(
                businessId: business.Id,
                action: isVerified ? "AdminVerifyBusiness" : "AdminUnverifyBusiness",
                userId: adminUserId);

            return true;
        }

        private static int SeverityRank(string severity)
        {
            if (string.Equals(severity, "Critical", StringComparison.OrdinalIgnoreCase)) return 4;
            if (string.Equals(severity, "High", StringComparison.OrdinalIgnoreCase)) return 3;
            if (string.Equals(severity, "Medium", StringComparison.OrdinalIgnoreCase)) return 2;
            return 1;
        }
    }
}
