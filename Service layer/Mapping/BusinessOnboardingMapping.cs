using System;
using System.Collections.Generic;
using System.Linq;
using Domain_layer.Models;
using Domain_layer.enums;
using Service_layer.DTOS.Business;

namespace Service_layer.Mapping
{
    /// <summary>
    /// Mapping extensions for BusinessOnboardingDTO to domain entities
    /// </summary>
    public static class BusinessOnboardingMapping
    {
        public static Business ToBusiness(this BusinessOnboardingDTO dto)
        {
            return new Business
            {
                Id = Guid.NewGuid().ToString(),
                Name = dto.Name,
                Type = string.IsNullOrWhiteSpace(dto.Type) ? "Restaurant" : dto.Type,
                Address = dto.Address,
                Phone = dto.Phone,
                
                // Contact Information
                Email = dto.Email,
                Website = dto.Website,
                FacebookUrl = dto.FacebookUrl,
                InstagramUrl = dto.InstagramUrl,

                // Location
                City = dto.City,
                Country = dto.Country,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,

                // Restaurant Information
                Description = dto.Description,
                CuisineType = dto.CuisineType,
                PriceRange = dto.PriceRange,
                LogoUrl = dto.LogoUrl,
                CoverImageUrl = dto.CoverImageUrl,

                // Features & Services
                HasDelivery = dto.HasDelivery,
                HasTakeout = dto.HasTakeout,
                HasParking = dto.HasParking,
                HasWiFi = dto.HasWiFi,
                HasOutdoorSeating = dto.HasOutdoorSeating,
                AcceptsReservations = dto.AcceptsReservations,

                // Payment Methods
                PaymentMethods = dto.PaymentMethods,

                // Status
                IsActive = true,
                IsVerified = false, // Default to false, admin can verify later

                CreatedAt = DateTime.UtcNow
            };
        }

        public static IEnumerable<WorkingHours> ToWorkingHoursEntities(this BusinessOnboardingDTO dto, string businessId)
        {
            if (dto.WorkingHours == null || dto.WorkingHours.Count == 0)
                return Enumerable.Empty<WorkingHours>();

            return dto.WorkingHours.Select(wh => new WorkingHours
            {
                WorkingHoursId = Guid.NewGuid().ToString(),
                DayOfWeek = wh.DayOfWeek,
                OpenTime = wh.OpenTime,
                CloseTime = wh.CloseTime,
                IsClosed = wh.IsClosed,
                BusinessId = businessId
            });
        }

        public static Setting ToSetting(this BusinessOnboardingDTO dto, string businessId)
        {
            return new Setting
            {
                SettingId = Guid.NewGuid().ToString(),
                BusinessId = businessId,
                ChatbotWelcomeMessage = dto.WelcomeMessage,
                ChatbotPersonality = dto.AgentTone,
                ChatbotEnabled = true,
                // Defaults from Setting entity
                AutoAssignTickets = true,
                EnableNotifications = true,
                Language = "en",
                TimeZone = "UTC",
                EmailNotifications = true,
                SmsNotifications = false,
                PushNotifications = true,
                // Voice Settings defaults
                AgentVoice = "default",
                AgentVoiceProvider = "azure",
                AgentVoiceSpeed = 1.0,
                AgentVoicePitch = 1.0,
                AgentVoiceLanguage = "en-US"
            };
        }

        public static IEnumerable<KnowledgeBase> ToKnowledgeBaseEntities(this BusinessOnboardingDTO dto, string businessId)
        {
            if (dto.KnowledgeBaseItems == null || dto.KnowledgeBaseItems.Count == 0)
                return Enumerable.Empty<KnowledgeBase>();

            return dto.KnowledgeBaseItems.Select(item => new KnowledgeBase
            {
                KnowledgeBaseId = Guid.NewGuid().ToString(),
                Question = item.Question,
                Answer = item.Answer,
                BusinessId = businessId,
                CreatedAt = DateTime.UtcNow
            });
        }

        public static Subscription ToSubscription(this BusinessOnboardingDTO dto, string businessId)
        {
            var startDate = DateTime.UtcNow;
            var endDate = dto.PlanName.ToLower().Contains("yearly") || dto.PlanName.ToLower().Contains("year")
                ? startDate.AddYears(1)
                : startDate.AddMonths(1);

            return new Subscription
            {
                Id = Guid.NewGuid().ToString(),
                PlanName = dto.PlanName,
                Price = dto.Price,
                StartDate = startDate,
                EndDate = endDate,
                IsActive = true,
                BusinessId = businessId
            };
        }

        public static PaymentTransaction ToPaymentTransaction(this BusinessOnboardingDTO dto, string subscriptionId)
        {
            return new PaymentTransaction
            {
                Id = Guid.NewGuid().ToString(),
                SubscriptionId = subscriptionId,
                Amount = dto.Price,
                PaymentMethod = PaymentMethod.Card,
                TransactionDate = DateTime.UtcNow,
                Status = PaymentStatus.Success
                // NOTE: Card details (CardNumber, CVV, etc.) are NOT stored for security reasons.
                // In production, you would call a payment gateway (Stripe, PayPal, etc.) and store only a token/reference.
            };
        }

        public static IEnumerable<MenuCategory> ToMenuCategoryEntities(this BusinessOnboardingDTO dto, string businessId)
        {
            if (dto.MenuCategories == null || dto.MenuCategories.Count == 0)
                return Enumerable.Empty<MenuCategory>();

            return dto.MenuCategories.Select((category, index) => new MenuCategory
            {
                MenuCategoryId = Guid.NewGuid().ToString(),
                Name = category.Name,
                Description = category.Description,
                DisplayOrder = category.DisplayOrder > 0 ? category.DisplayOrder : index,
                IsActive = true,
                BusinessId = businessId,
                CreatedAt = DateTime.UtcNow
            });
        }

        public static IEnumerable<MenuItem> ToMenuItemEntities(
            this BusinessOnboardingDTO dto, 
            string businessId, 
            Dictionary<string, string> categoryNameToIdMap)
        {
            if (dto.MenuItems == null || dto.MenuItems.Count == 0)
                return Enumerable.Empty<MenuItem>();

            return dto.MenuItems.Select(item => new MenuItem
            {
                MenuItemId = Guid.NewGuid().ToString(),
                Name = item.Name,
                Description = item.Description,
                Price = item.Price,
                MenuCategoryId = !string.IsNullOrWhiteSpace(item.MenuCategoryName) 
                    && categoryNameToIdMap.ContainsKey(item.MenuCategoryName)
                    ? categoryNameToIdMap[item.MenuCategoryName] 
                    : null,
                IsAvailable = item.IsAvailable,
                BusinessId = businessId
            });
        }
    }
}

