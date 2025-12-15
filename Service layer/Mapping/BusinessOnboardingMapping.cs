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
                Name = dto.Name,
                Type = string.IsNullOrWhiteSpace(dto.Type) ? "Restaurant" : dto.Type,
                Address = dto.Address,
                Phone = dto.Phone,
                CreatedAt = DateTime.UtcNow
            };
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
                PushNotifications = true
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
    }
}

