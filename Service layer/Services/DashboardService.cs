using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain_layer.Interfaces;
using Service_layer.DTOS.Dashboard;
using Service_layer.Services_Interfaces;

namespace Service_layer.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISettingService _settingService;

        public DashboardService(IUnitOfWork unitOfWork, ISettingService settingService)
        {
            _unitOfWork = unitOfWork;
            _settingService = settingService;
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
                SetupStepsPending = setupStepsPending
            };

            return dashboard;
        }
    }
}

