using System;
using System.Collections.Generic;
using System.Linq;
using Domain_layer.Models;
using Domain_layer.enums;
using Service_layer.DTOS.Business;

namespace Service_layer.Mapping
{
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
                // AgentName can be stored later or used in prompts/messages
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
                BusinessId = businessId
            });
        }

        public static Subscription ToSubscription(this BusinessOnboardingDTO dto, string businessId)
        {
            return new Subscription
            {
                Id = Guid.NewGuid().ToString(),
                PlanName = dto.PlanName,
                Price = dto.Price,
                StartDate = DateTime.UtcNow,
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
            };
        }
    }
}


