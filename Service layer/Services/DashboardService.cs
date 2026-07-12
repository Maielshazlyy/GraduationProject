using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Domain_layer.Interfaces;
using Domain_layer.Models;
using Domain_layer.enums;
using Feedback = Domain_layer.Models.Feedback;
using Service_layer.DTOS.AiAnalysis;
using Service_layer.DTOS.Dashboard;
using Service_layer.DTOS.AuditLog;
using Service_layer.Services_Interfaces;
using Service_layer.Mapping;

namespace Service_layer.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISettingService _settingService;
        private readonly IAuditLogService _auditLogService;

        public DashboardService(
            IUnitOfWork unitOfWork, 
            ISettingService settingService,
            IAuditLogService auditLogService)
        {
            _unitOfWork = unitOfWork;
            _settingService = settingService;
            _auditLogService = auditLogService;
        }

        public async Task<DashboardSummaryDTO> GetDashboardSummaryAsync(string businessId)
        {
            var business = await _unitOfWork.Businesses.GetByIdAsync(businessId);
            if (business == null)
                throw new ArgumentException($"Business with id '{businessId}' not found.");

            // Get Settings
            var settings = await _settingService.GetByBusinessIdAsync(businessId);

            // Get Menu Items and Categories
            var menuItems = (await _unitOfWork.MenuItems.GetByBusinessIdAsync(businessId)).ToList();
            var menuCategories = (await _unitOfWork.MenuCategories.GetByBusinessIdAsync(businessId)).ToList();

            // Get Knowledge Base
            var knowledgeBase = (await _unitOfWork.KnowledgeBases.GetByBusinessIdAsync(businessId)).ToList();

            // Get FAQs (using KnowledgeBase with a flag or separate entity)
            // For now, we'll use all KnowledgeBase items as FAQs
            var faqs = knowledgeBase;

            // Determine setup status
            var setupStepsCompleted = new List<string>();
            var setupStepsPending = new List<string>();

            if (settings != null)
            {
                setupStepsCompleted.Add("Settings");
                if (!string.IsNullOrWhiteSpace(settings.ChatbotWelcomeMessage))
                    setupStepsCompleted.Add("Welcome Message");
                if (!string.IsNullOrWhiteSpace(settings.AgentVoice) && settings.AgentVoice != "default")
                    setupStepsCompleted.Add("Voice Settings");
            }
            else
            {
                setupStepsPending.Add("Settings");
            }

            if (menuItems.Any())
                setupStepsCompleted.Add("Menu Items");
            else
                setupStepsPending.Add("Menu Items");

            if (menuCategories.Any())
                setupStepsCompleted.Add("Menu Categories");
            else
                setupStepsPending.Add("Menu Categories");

            if (knowledgeBase.Any())
                setupStepsCompleted.Add("Knowledge Base");
            else
                setupStepsPending.Add("Knowledge Base");

            // Get Audit Log Statistics
            var auditLogs = (await _auditLogService.GetByBusinessIdAsync(businessId)).ToList();
            var auditLogsLast24Hours = auditLogs.Count(a => a.CreatedAt >= DateTime.UtcNow.AddDays(-1));
            var lastAuditLog = auditLogs.OrderByDescending(a => a.CreatedAt).FirstOrDefault();

            var dashboard = new DashboardSummaryDTO
            {
                BusinessId = business.Id,
                BusinessName = business.Name,
                BusinessType = business.Type,

                // Settings Summary
                HasSettings = settings != null,
                ChatbotEnabled = settings?.ChatbotEnabled ?? false,
                WelcomeMessage = settings?.ChatbotWelcomeMessage,
                AgentVoice = settings?.AgentVoice,

                // Quick Stats
                TotalMenuItems = menuItems.Count,
                TotalMenuCategories = menuCategories.Count,
                TotalKnowledgeBaseItems = knowledgeBase.Count,
                TotalFAQs = faqs.Count,

                // Recent Activity
                LastMenuUpdate = menuItems.Any() ? menuItems.Max(m => m.CreatedAt) : null,
                LastKnowledgeBaseUpdate = knowledgeBase.Any() ? knowledgeBase.Max(k => k.CreatedAt) : null,

                // Setup Status
                IsSetupComplete = setupStepsPending.Count == 0,
                SetupStepsCompleted = setupStepsCompleted,
                SetupStepsPending = setupStepsPending,

                // Audit Log Summary
                TotalAuditLogs = auditLogs.Count,
                AuditLogsLast24Hours = auditLogsLast24Hours,
                LastAuditLogDate = lastAuditLog?.CreatedAt
            };

            return dashboard;
        }

        public async Task<List<AuditLogResponseDTO>> GetRecentAuditLogsAsync(string businessId, int count = 20)
        {
            var auditLogs = (await _auditLogService.GetByBusinessIdAsync(businessId))
                .OrderByDescending(a => a.CreatedAt)
                .Take(count)
                .ToList();

            return auditLogs.ToDtoList().ToList();
        }

        public async Task<AuditLogStatisticsDTO> GetAuditLogStatisticsAsync(string businessId)
        {
            var auditLogs = (await _auditLogService.GetByBusinessIdAsync(businessId)).ToList();
            var now = DateTime.UtcNow;

            var statistics = new AuditLogStatisticsDTO
            {
                TotalActions = auditLogs.Count,
                ActionsLast24Hours = auditLogs.Count(a => a.CreatedAt >= now.AddDays(-1)),
                ActionsLast7Days = auditLogs.Count(a => a.CreatedAt >= now.AddDays(-7)),
                ActionsLast30Days = auditLogs.Count(a => a.CreatedAt >= now.AddDays(-30))
            };

            // Actions by Entity Type
            statistics.ActionsByEntity = auditLogs
                .GroupBy(a => a.Entity)
                .ToDictionary(g => g.Key, g => g.Count());

            // Actions by Type (extract action prefix, e.g., "CreateOrder" -> "Create")
            statistics.ActionsByType = auditLogs
                .Select(a => a.Action.Split('_')[0].Split(new[] { "FromChat", "FromVoice" }, StringSplitOptions.None)[0])
                .GroupBy(action => action)
                .ToDictionary(g => g.Key, g => g.Count());

            // Most Active Users
            statistics.MostActiveUsers = auditLogs
                .Where(a => !string.IsNullOrWhiteSpace(a.UserId))
                .GroupBy(a => a.UserId)
                .Select(g => new UserActivityDTO
                {
                    UserId = g.Key ?? string.Empty,
                    UserName = "User", // Can be enhanced to get actual user name
                    ActionCount = g.Count()
                })
                .OrderByDescending(u => u.ActionCount)
                .Take(10)
                .ToList();

            // Recent Critical Actions (Delete, Escalate, etc.)
            var criticalActions = new[] { "Delete", "Escalate", "CloseTicket", "AssignTicket" };
            statistics.RecentCriticalActions = auditLogs
                .Where(a => criticalActions.Any(ca => a.Action.Contains(ca)))
                .OrderByDescending(a => a.CreatedAt)
                .Take(10)
                .Select(a => $"{a.Action} on {a.Entity} ({a.EntityId}) at {a.CreatedAt:yyyy-MM-dd HH:mm}")
                .ToList();

            return statistics;
        }

        public async Task<List<AuditLogResponseDTO>> GetCustomerAuditLogsAsync(string businessId, string customerId)
        {
            // Verify customer exists and belongs to business
            var customer = await _unitOfWork.Customers.GetByIdAsync(customerId);
            if (customer == null)
                throw new ArgumentException($"Customer with id '{customerId}' not found.");

            if (customer.BusinessId != businessId)
                throw new ArgumentException($"Customer does not belong to the specified business.");

            // Get all related entity IDs for this customer
            var orders = (await _unitOfWork.Orders.GetByCustomerIdAsync(customerId)).ToList();
            var tickets = (await _unitOfWork.Tickets.GetByCustomerIdAsync(customerId)).ToList();
            var interactions = (await _unitOfWork.Interactions.GetByCustomerIdAsync(customerId)).ToList();
            var feedbacks = (await _unitOfWork.Feedbacks.GetByCustomerIdAsync(customerId)).ToList();

            var orderIds = orders.Select(o => o.OrderId).ToHashSet();
            var ticketIds = tickets.Select(t => t.TicketId).ToHashSet();
            var interactionIds = interactions.Select(i => i.InteractionId).ToHashSet();
            var feedbackIds = feedbacks.Select(f => f.FeedbackId).ToHashSet();

            // Get all audit logs for this business
            var allAuditLogs = (await _auditLogService.GetByBusinessIdAsync(businessId)).ToList();

            // Filter audit logs that relate to this customer's entities
            var customerAuditLogs = allAuditLogs
                .Where(log =>
                    (log.Entity == "Order" && orderIds.Contains(log.EntityId)) ||
                    (log.Entity == "Ticket" && ticketIds.Contains(log.EntityId)) ||
                    (log.Entity == "Interaction" && interactionIds.Contains(log.EntityId)) ||
                    (log.Entity == "Feedback" && feedbackIds.Contains(log.EntityId))
                )
                .OrderByDescending(log => log.CreatedAt)
                .ToList();

            return customerAuditLogs.ToDtoList().ToList();
        }

        public async Task<FullDashboardDTO> GetFullDashboardAsync(string businessId, DashboardFilterDTO filter)
        {
            var business = await _unitOfWork.Businesses.GetByIdAsync(businessId);
            if (business == null)
                throw new ArgumentException($"Business with id '{businessId}' not found.");

            var now = DateTime.UtcNow;
            var customFrom = filter?.CustomFrom;
            var customTo = filter?.CustomTo;

            // Resolve per-section periods (null -> "30d" default)
            var insightsPeriod      = Normalize(filter?.InsightsPeriod);
            var channelsPeriod      = Normalize(filter?.ChannelsPeriod);
            var sentimentPeriod     = Normalize(filter?.SentimentPeriod);
            var revenuePeriod       = Normalize(filter?.RevenuePeriod);
            var productsPeriod      = Normalize(filter?.ProductsPeriod);
            var leadsPeriod         = Normalize(filter?.LeadsPeriod);
            var chatAnalysisPeriod  = Normalize(filter?.ChatAnalysisPeriod);
            var feedbacksPeriod     = Normalize(filter?.FeedbacksPeriod);

            // Load all data once
            var orders = (await _unitOfWork.Orders.GetByBusinessIdAsync(businessId)).ToList();
            var customers = (await _unitOfWork.Customers.GetByBusinessIdAsync(businessId)).ToList();
            var tickets = (await _unitOfWork.Tickets.GetByBusinessIdAsync(businessId)).ToList();
            var interactions = (await _unitOfWork.Interactions.GetByBusinessIdAsync(businessId)).ToList();
            var sentiments = (await _unitOfWork.Sentiments.GetByBusinessIdAsync(businessId)).ToList();
            var menuItems = (await _unitOfWork.MenuItems.GetByBusinessIdAsync(businessId)).ToList();
            var menuCategories = (await _unitOfWork.MenuCategories.GetByBusinessIdAsync(businessId)).ToList();
            var analyses = (await _unitOfWork.InteractionAnalyses.GetByBusinessIdAsync(businessId)).ToList();
            var feedbacks = (await _unitOfWork.Feedbacks.GetByBusinessIdAsync(businessId)).ToList();

            var businessOrderIds = orders.Select(o => o.OrderId).ToHashSet(StringComparer.Ordinal);
            var allOrderItems = (await _unitOfWork.OrderItems.GetAllAsync())
                .Where(oi => businessOrderIds.Contains(oi.OrderId))
                .ToList();

            var paidStatuses = new[] { OrderStatus.Paid, OrderStatus.Delivered };
            var categoryById = menuCategories.ToDictionary(c => c.MenuCategoryId, c => c.Name, StringComparer.Ordinal);
            var menuItemById = menuItems.ToDictionary(m => m.MenuItemId, m => m, StringComparer.Ordinal);

            // Date ranges per section: (from, to, prevFrom, prevTo)
            var ins      = ResolveRange(insightsPeriod, now, customFrom, customTo);
            var ch       = ResolveRange(channelsPeriod, now, customFrom, customTo);
            var sent     = ResolveRange(sentimentPeriod, now, customFrom, customTo);
            var prod     = ResolveRange(productsPeriod, now, customFrom, customTo);
            var leads    = ResolveRange(leadsPeriod, now, customFrom, customTo);
            var chatAnal = ResolveRange(chatAnalysisPeriod, now, customFrom, customTo);
            var fb       = ResolveRange(feedbacksPeriod, now, customFrom, customTo);

            var anyCustom = new[] { insightsPeriod, channelsPeriod, sentimentPeriod, revenuePeriod, productsPeriod, leadsPeriod, chatAnalysisPeriod, feedbacksPeriod }
                .Contains("custom");

            return new FullDashboardDTO
            {
                AppliedPeriods = new AppliedPeriodsDTO
                {
                    Insights     = insightsPeriod,
                    Channels     = channelsPeriod,
                    Sentiment    = sentimentPeriod,
                    Revenue      = revenuePeriod,
                    Products     = productsPeriod,
                    Leads        = leadsPeriod,
                    ChatAnalysis = chatAnalysisPeriod,
                    Feedbacks    = feedbacksPeriod,
                    CustomFrom   = anyCustom ? customFrom : null,
                    CustomTo     = anyCustom ? customTo : null
                },
                GeneralInsights = BuildGeneralInsights(orders, tickets, interactions, customers, paidStatuses, ins.from, ins.to, ins.prevFrom, ins.prevTo),
                ChannelDistribution = BuildChannelDistribution(interactions, ch.from, ch.to),
                SentimentAnalysis = BuildSentimentAnalysis(sentiments, analyses, sent.from, sent.to),
                RevenueTrend = BuildRevenueTrend(orders, paidStatuses, now, revenuePeriod, customFrom, customTo),
                TopProducts = BuildTopProducts(orders, allOrderItems, menuItemById, categoryById, paidStatuses, prod.from, prod.to),
                RecentAlerts = BuildRecentAlerts(tickets),
                CustomerLeads = BuildCustomerLeads(customers, orders, interactions, paidStatuses, leads.from, leads.to),
                ChatAnalysis    = BuildChatAnalysis(analyses, chatAnal.from, chatAnal.to),
                FeedbackSummary = BuildFeedbackSummary(feedbacks, fb.from, fb.to)
            };
        }

        // ─── helpers ──────────────────────────────────────────────────────────────

        private static readonly string[] AllowedPeriods = { "today", "7d", "30d", "all", "custom" };

        private static string Normalize(string? period)
        {
            if (string.IsNullOrWhiteSpace(period)) return "30d";
            var p = period.Trim().ToLowerInvariant();
            return AllowedPeriods.Contains(p) ? p : "30d";
        }

        private static (DateTime from, DateTime to, DateTime prevFrom, DateTime prevTo) ResolveRange(
            string period, DateTime now, DateTime? customFrom, DateTime? customTo)
        {
            switch (period)
            {
                case "today":
                    return (now.Date, now, now.Date.AddDays(-1), now.Date);
                case "7d":
                    return (now.AddDays(-7), now, now.AddDays(-14), now.AddDays(-7));
                case "all":
                    return (DateTime.MinValue, now, DateTime.MinValue, DateTime.MinValue); // no filter, no comparison
                case "custom" when customFrom.HasValue && customTo.HasValue:
                    var f = customFrom.Value;
                    var t = customTo.Value;
                    var span = t - f;
                    return (f, t, f - span, f); // previous = equal-length window immediately before
                default: // 30d (and custom without dates, defensively)
                    return (now.AddDays(-30), now, now.AddDays(-60), now.AddDays(-30));
            }
        }

        private static MetricWithChangeDTO BuildMetric(double current, double previous)
        {
            double? change = previous == 0
                ? (current > 0 ? 100 : (double?)null)
                : Math.Round((current - previous) / previous * 100, 1);

            return new MetricWithChangeDTO
            {
                Value = Math.Round(current, 2),
                PreviousValue = Math.Round(previous, 2),
                ChangePercentage = change,
                Trend = change == null ? "neutral" : change > 0 ? "up" : change < 0 ? "down" : "neutral"
            };
        }

        private static GeneralInsightsDTO BuildGeneralInsights(
            List<Domain_layer.Models.Order> orders,
            List<Domain_layer.Models.Ticket> tickets,
            List<Domain_layer.Models.Interaction> interactions,
            List<Domain_layer.Models.Customer> customers,
            OrderStatus[] paidStatuses,
            DateTime from, DateTime to, DateTime previousFrom, DateTime previousTo)
        {
            // Current period
            var curOrders = orders.Where(o => o.CreatedAt >= from && o.CreatedAt <= to).ToList();
            var curPaidOrders = curOrders.Where(o => paidStatuses.Contains(o.Status)).ToList();
            var curTickets = tickets.Where(t => t.CreatedAt >= from && t.CreatedAt <= to).ToList();
            var curInteractions = interactions.Where(i => i.StartedAt >= from && i.StartedAt <= to).ToList();
            var curCustomers = customers.Where(c => c.CreatedAt >= from && c.CreatedAt <= to).ToList();

            // Previous period
            var prevOrders = orders.Where(o => o.CreatedAt >= previousFrom && o.CreatedAt < previousTo).ToList();
            var prevPaidOrders = prevOrders.Where(o => paidStatuses.Contains(o.Status)).ToList();
            var prevTickets = tickets.Where(t => t.CreatedAt >= previousFrom && t.CreatedAt < previousTo).ToList();
            var prevInteractions = interactions.Where(i => i.StartedAt >= previousFrom && i.StartedAt < previousTo).ToList();
            var prevCustomers = customers.Where(c => c.CreatedAt >= previousFrom && c.CreatedAt < previousTo).ToList();

            // Open tickets is a snapshot (not period-filtered) but we compare counts at period boundaries
            var openTicketsCurrent = tickets.Count(t => t.Status == "Open" && t.CreatedAt <= to);
            var openTicketsPrev = tickets.Count(t => t.Status == "Open" && t.CreatedAt < previousTo);

            // Avg resolution time (hours)
            static double AvgResolution(List<Domain_layer.Models.Ticket> ts)
            {
                var closed = ts.Where(t => t.Status == "Closed" && t.ClosedAt.HasValue).ToList();
                return closed.Any() ? closed.Average(t => (t.ClosedAt!.Value - t.CreatedAt).TotalHours) : 0;
            }

            return new GeneralInsightsDTO
            {
                TotalRevenue          = BuildMetric((double)curPaidOrders.Sum(o => o.TotalPrice),  (double)prevPaidOrders.Sum(o => o.TotalPrice)),
                AvgResolutionTimeHours = BuildMetric(AvgResolution(curTickets),                    AvgResolution(prevTickets)),
                TotalInteractions     = BuildMetric(curInteractions.Count,                          prevInteractions.Count),
                AvgOrderValue         = BuildMetric(curOrders.Any() ? (double)curOrders.Average(o => o.TotalPrice) : 0,
                                                    prevOrders.Any() ? (double)prevOrders.Average(o => o.TotalPrice) : 0),
                NewCustomers          = BuildMetric(curCustomers.Count,                             prevCustomers.Count),
                OpenTickets           = BuildMetric(openTicketsCurrent,                             openTicketsPrev)
            };
        }

        private static ChannelDistributionDTO BuildChannelDistribution(
            List<Domain_layer.Models.Interaction> interactions, DateTime from, DateTime to)
        {
            var filtered = interactions.Where(i => i.StartedAt >= from && i.StartedAt <= to).ToList();
            var total = filtered.Count;

            var channels = filtered
                .GroupBy(i => string.IsNullOrWhiteSpace(i.Channel) ? "Unknown" : i.Channel)
                .Select(g => new ChannelCountDTO
                {
                    Channel = g.Key,
                    Count = g.Count(),
                    Percentage = total == 0 ? 0 : Math.Round(g.Count() * 100.0 / total, 1)
                })
                .OrderByDescending(c => c.Count)
                .ToList();

            return new ChannelDistributionDTO { Total = total, Channels = channels };
        }

        private static SentimentAnalysisDTO BuildSentimentAnalysis(
            List<Sentiment> sentiments,
            List<InteractionAnalysis> analyses,
            DateTime from, DateTime to)
        {
            // Prefer AI session-level sentiment (InteractionAnalysis) when available,
            // fall back to keyword-based per-message sentiment (Sentiments table).
            if (analyses.Any(a => a.AnalyzedAt >= from && a.AnalyzedAt <= to))
            {
                var filtered = analyses.Where(a => a.AnalyzedAt >= from && a.AnalyzedAt <= to).ToList();
                var total    = filtered.Count;

                var satisfied = filtered.Count(a => a.SentimentLabel.Equals("Positive", StringComparison.OrdinalIgnoreCase));
                var neutral   = filtered.Count(a => a.SentimentLabel.Equals("Neutral",  StringComparison.OrdinalIgnoreCase));
                var angry     = filtered.Count(a => a.SentimentLabel.Equals("Negative", StringComparison.OrdinalIgnoreCase));

                return new SentimentAnalysisDTO
                {
                    Satisfied           = satisfied,
                    Neutral             = neutral,
                    Angry               = angry,
                    Total               = total,
                    SatisfiedPercentage = total == 0 ? 0 : Math.Round(satisfied * 100.0 / total, 1),
                    NeutralPercentage   = total == 0 ? 0 : Math.Round(neutral   * 100.0 / total, 1),
                    AngryPercentage     = total == 0 ? 0 : Math.Round(angry     * 100.0 / total, 1)
                };
            }

            // Fallback: keyword-based per-message sentiment
            var msgs  = sentiments.Where(s => s.AnalyzedAt >= from && s.AnalyzedAt <= to).ToList();
            var total2     = msgs.Count;
            var satisfied2 = msgs.Count(s => s.Label.Equals("Positive", StringComparison.OrdinalIgnoreCase));
            var neutral2   = msgs.Count(s => s.Label.Equals("Neutral",  StringComparison.OrdinalIgnoreCase));
            var angry2     = msgs.Count(s => s.Label.Equals("Negative", StringComparison.OrdinalIgnoreCase));

            return new SentimentAnalysisDTO
            {
                Satisfied           = satisfied2,
                Neutral             = neutral2,
                Angry               = angry2,
                Total               = total2,
                SatisfiedPercentage = total2 == 0 ? 0 : Math.Round(satisfied2 * 100.0 / total2, 1),
                NeutralPercentage   = total2 == 0 ? 0 : Math.Round(neutral2   * 100.0 / total2, 1),
                AngryPercentage     = total2 == 0 ? 0 : Math.Round(angry2     * 100.0 / total2, 1)
            };
        }

        private static ChatAnalysisDTO BuildChatAnalysis(
            List<InteractionAnalysis> analyses, DateTime from, DateTime to)
        {
            var filtered = analyses
                .Where(a => a.AnalyzedAt >= from && a.AnalyzedAt <= to)
                .ToList();

            if (!filtered.Any())
                return new ChatAnalysisDTO();

            var total = filtered.Count;

            // ── Sentiment ─────────────────────────────────────────────────────
            var avgScore = filtered.Average(a => a.SentimentScore);
            var overallLabel = avgScore >= 0.2 ? "Positive" : avgScore <= -0.2 ? "Negative" : "Neutral";

            // ── Intents — parse JSON from each session ─────────────────────────
            var intentCounts = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            foreach (var a in filtered.Where(a => !string.IsNullOrWhiteSpace(a.IntentsDetectedJson)))
            {
                try
                {
                    var intents = JsonSerializer.Deserialize<List<AiIntentResultDTO>>(a.IntentsDetectedJson!);
                    if (intents == null) continue;
                    foreach (var intent in intents)
                    {
                        if (string.IsNullOrWhiteSpace(intent.Name)) continue;
                        intentCounts.TryGetValue(intent.Name, out var existing);
                        intentCounts[intent.Name] = existing + intent.Count;
                    }
                }
                catch { /* skip malformed JSON */ }
            }

            var totalIntentCount = intentCounts.Values.Sum();
            var topIntents = intentCounts
                .OrderByDescending(kv => kv.Value)
                .Take(8)
                .Select(kv => new IntentDistributionDTO
                {
                    Intent     = kv.Key,
                    Count      = kv.Value,
                    Percentage = totalIntentCount == 0 ? 0 : Math.Round(kv.Value * 100.0 / totalIntentCount, 1)
                })
                .ToList();

            // ── Topics — parse JSON from each session ──────────────────────────
            var topicCounts = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            foreach (var a in filtered.Where(a => !string.IsNullOrWhiteSpace(a.MainTopicsJson)))
            {
                try
                {
                    var topics = JsonSerializer.Deserialize<List<string>>(a.MainTopicsJson!);
                    if (topics == null) continue;
                    foreach (var topic in topics.Where(t => !string.IsNullOrWhiteSpace(t)))
                    {
                        topicCounts.TryGetValue(topic, out var existing);
                        topicCounts[topic] = existing + 1;
                    }
                }
                catch { /* skip malformed JSON */ }
            }

            var topTopics = topicCounts
                .OrderByDescending(kv => kv.Value)
                .Take(10)
                .Select(kv => new TopicCountDTO { Topic = kv.Key, Count = kv.Value })
                .ToList();

            // ── Key moments — aggregate across sessions (most recent sessions first) ──
            var topKeyMoments = new List<string>();
            foreach (var a in filtered.OrderByDescending(a => a.AnalyzedAt))
            {
                if (string.IsNullOrWhiteSpace(a.KeyMomentsJson)) continue;
                try
                {
                    var moments = JsonSerializer.Deserialize<List<string>>(a.KeyMomentsJson!);
                    if (moments == null) continue;
                    topKeyMoments.AddRange(moments.Where(m => !string.IsNullOrWhiteSpace(m)));
                    if (topKeyMoments.Count >= 10) break;
                }
                catch { /* skip malformed JSON */ }
            }
            topKeyMoments = topKeyMoments.Take(10).ToList();

            // ── Recent sessions ────────────────────────────────────────────────
            var recentSessions = filtered
                .OrderByDescending(a => a.AnalyzedAt)
                .Take(5)
                .Select(a => new RecentSessionInsightDTO
                {
                    InteractionId  = a.InteractionId,
                    CustomerId     = a.Interaction?.CustomerId,
                    CustomerName   = a.Interaction?.Customer?.FullName,
                    MainIntent     = a.MainIntent,
                    Summary        = a.Summary,
                    SummaryAr      = a.SummaryAr,
                    SentimentLabel = a.SentimentLabel,
                    SentimentScore = a.SentimentScore,
                    AnalyzedAt     = a.AnalyzedAt
                })
                .ToList();

            return new ChatAnalysisDTO
            {
                TotalAnalyzedSessions = total,
                AvgSentimentScore     = Math.Round(avgScore, 2),
                OverallSentimentLabel = overallLabel,
                TopIntents            = topIntents,
                TopTopics             = topTopics,
                TopKeyMoments         = topKeyMoments,
                RecentSessions        = recentSessions
            };
        }

        private static FeedbackSummaryDTO BuildFeedbackSummary(
            List<Feedback> feedbacks, DateTime from, DateTime to)
        {
            var filtered = feedbacks.Where(f => f.CreatedAt >= from && f.CreatedAt <= to).ToList();
            var total = filtered.Count;

            if (total == 0) return new FeedbackSummaryDTO();

            var positive = filtered.Count(f => f.Rating >= 4);
            var neutral  = filtered.Count(f => f.Rating == 3);
            var negative = filtered.Count(f => f.Rating <= 2);

            return new FeedbackSummaryDTO
            {
                Total              = total,
                AverageRating      = Math.Round(filtered.Average(f => f.Rating), 1),
                PositiveCount      = positive,
                NeutralCount       = neutral,
                NegativeCount      = negative,
                PositivePercentage = Math.Round(positive * 100.0 / total, 1),
                NeutralPercentage  = Math.Round(neutral  * 100.0 / total, 1),
                NegativePercentage = Math.Round(negative * 100.0 / total, 1)
            };
        }

        private static List<RevenueTrendPointDTO> BuildRevenueTrend(
            List<Domain_layer.Models.Order> orders,
            OrderStatus[] paidStatuses,
            DateTime now, string period, DateTime? customFrom, DateTime? customTo)
        {
            var paidOrders = orders.Where(o => paidStatuses.Contains(o.Status)).ToList();

            if (period == "today")
            {
                // Hourly buckets for today (0–23)
                var todays = paidOrders.Where(o => o.CreatedAt.Date == now.Date).ToList();
                return Enumerable.Range(0, 24).Select(h =>
                {
                    var bucket = todays.Where(o => o.CreatedAt.Hour == h).ToList();
                    return new RevenueTrendPointDTO
                    {
                        Label = $"{h:00}:00",
                        Revenue = bucket.Sum(o => o.TotalPrice),
                        OrderCount = bucket.Count
                    };
                }).ToList();
            }

            if (period == "all")
                return MonthlyBuckets(paidOrders);

            // Resolve the window [from, to]
            DateTime from, to;
            if (period == "custom" && customFrom.HasValue && customTo.HasValue)
            {
                from = customFrom.Value.Date;
                to = customTo.Value;
            }
            else
            {
                int days = period == "7d" ? 7 : 30;
                from = now.AddDays(-days).Date;
                to = now;
            }

            var totalDays = (int)Math.Ceiling((to.Date - from.Date).TotalDays) + 1;

            // For long custom ranges, switch to monthly buckets to keep the series readable
            if (totalDays > 62)
                return MonthlyBuckets(paidOrders.Where(o => o.CreatedAt >= from && o.CreatedAt <= to).ToList());

            // Daily buckets across [from, to]
            var windowed = paidOrders.Where(o => o.CreatedAt >= from && o.CreatedAt <= to).ToList();
            return Enumerable.Range(0, totalDays).Select(d =>
            {
                var day = from.AddDays(d).Date;
                var bucket = windowed.Where(o => o.CreatedAt.Date == day).ToList();
                return new RevenueTrendPointDTO
                {
                    Label = day.ToString("MM-dd"),
                    Revenue = bucket.Sum(o => o.TotalPrice),
                    OrderCount = bucket.Count
                };
            }).ToList();
        }

        private static List<RevenueTrendPointDTO> MonthlyBuckets(List<Domain_layer.Models.Order> paidOrders)
        {
            if (!paidOrders.Any()) return new List<RevenueTrendPointDTO>();
            return paidOrders
                .GroupBy(o => new { o.CreatedAt.Year, o.CreatedAt.Month })
                .OrderBy(g => g.Key.Year).ThenBy(g => g.Key.Month)
                .Select(g => new RevenueTrendPointDTO
                {
                    Label = $"{g.Key.Year}-{g.Key.Month:00}",
                    Revenue = g.Sum(o => o.TotalPrice),
                    OrderCount = g.Count()
                })
                .ToList();
        }

        private static List<TopProductDashboardDTO> BuildTopProducts(
            List<Domain_layer.Models.Order> orders,
            List<Domain_layer.Models.OrderItem> orderItems,
            Dictionary<string, Domain_layer.Models.MenuItem> menuItemById,
            Dictionary<string, string> categoryById,
            OrderStatus[] paidStatuses,
            DateTime from, DateTime to)
        {
            var paidOrderIds = orders
                .Where(o => paidStatuses.Contains(o.Status) && o.CreatedAt >= from && o.CreatedAt <= to)
                .Select(o => o.OrderId)
                .ToHashSet(StringComparer.Ordinal);

            return orderItems
                .Where(oi => paidOrderIds.Contains(oi.OrderId))
                .GroupBy(oi => oi.MenuItemId)
                .Select(g =>
                {
                    menuItemById.TryGetValue(g.Key, out var item);
                    var categoryName = item?.MenuCategoryId != null && categoryById.TryGetValue(item.MenuCategoryId, out var cat) ? cat : "Uncategorized";
                    return new TopProductDashboardDTO
                    {
                        MenuItemId    = g.Key,
                        Name          = item?.Name ?? "Unknown",
                        Category      = categoryName,
                        TotalUnitsSold = g.Sum(x => x.Quantity),
                        Revenue       = g.Sum(x => x.UnitPrice * x.Quantity),
                        OrdersCount   = g.Select(x => x.OrderId).Distinct(StringComparer.Ordinal).Count()
                    };
                })
                .OrderByDescending(p => p.TotalUnitsSold)
                .ThenByDescending(p => p.Revenue)
                .Take(10)
                .ToList();
        }

        private static List<AlertInsightDTO> BuildRecentAlerts(List<Domain_layer.Models.Ticket> tickets)
        {
            // Surface open/escalated/high-priority tickets as actionable alerts
            return tickets
                .Where(t => t.Status is "Open" or "Escalated" || t.PriorityLevel is "High" or "Critical")
                .OrderByDescending(t => t.CreatedAt)
                .Take(20)
                .Select(t => new AlertInsightDTO
                {
                    Id       = t.TicketId,
                    Type     = t.TicketType ?? (t.Status == "Escalated" ? "Escalation" : "OpenTicket"),
                    Category = t.PriorityLevel ?? "Normal",
                    Message  = string.IsNullOrWhiteSpace(t.Subject)
                        ? $"Ticket #{t.TicketId[..8]} — {t.Status}"
                        : t.Subject,
                    CreatedAt = t.CreatedAt,
                    Severity  = t.PriorityLevel ?? "Normal"
                })
                .ToList();
        }

        private static List<CustomerLeadDTO> BuildCustomerLeads(
            List<Domain_layer.Models.Customer> customers,
            List<Domain_layer.Models.Order> orders,
            List<Domain_layer.Models.Interaction> interactions,
            OrderStatus[] paidStatuses,
            DateTime from, DateTime to)
        {
            // Active customers: created in window OR had an order in the window
            var activeInPeriod = orders
                .Where(o => o.CreatedAt >= from && o.CreatedAt <= to)
                .Select(o => o.CustomerId)
                .ToHashSet(StringComparer.Ordinal);

            var periodCustomers = customers
                .Where(c => (c.CreatedAt >= from && c.CreatedAt <= to) || activeInPeriod.Contains(c.CustomerId))
                .ToList();

            // Pre-compute per-customer stats from all-time data
            var ordersByCustomer = orders
                .GroupBy(o => o.CustomerId)
                .ToDictionary(g => g.Key, g => g.ToList(), StringComparer.Ordinal);

            var lastInteractionByCustomer = interactions
                .GroupBy(i => i.CustomerId)
                .ToDictionary(g => g.Key, g => g.Max(i => i.StartedAt), StringComparer.Ordinal);

            return periodCustomers
                .Select(c =>
                {
                    ordersByCustomer.TryGetValue(c.CustomerId, out var custOrders);
                    custOrders ??= new List<Domain_layer.Models.Order>();

                    var paidOrders = custOrders.Where(o => paidStatuses.Contains(o.Status)).ToList();
                    var totalOrders = custOrders.Count;
                    var totalSpend = paidOrders.Sum(o => o.TotalPrice);

                    lastInteractionByCustomer.TryGetValue(c.CustomerId, out var lastInteraction);
                    var lastOrderDate = custOrders.Any() ? custOrders.Max(o => o.CreatedAt) : c.CreatedAt;
                    var lastActivity = lastInteraction > lastOrderDate ? lastInteraction : lastOrderDate;

                    return new CustomerLeadDTO
                    {
                        CustomerId   = c.CustomerId,
                        Name         = c.FullName,
                        Phone        = c.Phone,
                        Tag          = totalOrders >= 10 ? "VIP" : totalOrders >= 3 ? "Regular" : totalOrders >= 1 ? "New" : "Potential",
                        TotalSpend   = totalSpend,
                        TotalOrders  = totalOrders,
                        LastActivity = lastActivity
                    };
                })
                .OrderByDescending(c => c.TotalSpend)
                .ToList();
        }
    }
}

