using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain_layer.Interfaces;
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
                LastMenuUpdate = null, // MenuItem doesn't have CreatedAt field
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
    }
}

