using System;
using System.Collections.Generic;

namespace Service_layer.DTOS.Business
{
    /// <summary>
    /// DTO for complete restaurant business onboarding flow.
    /// Includes business info, agent configuration, knowledge base, subscription, and payment.
    /// </summary>
    public class BusinessOnboardingDTO
    {
        // Business Information
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = "Restaurant"; // Default to Restaurant
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;

        // Agent/Chatbot Configuration
        public string AgentName { get; set; } = string.Empty;
        public string AgentTone { get; set; } = "Friendly"; // e.g., Friendly, Professional, Casual
        public string WelcomeMessage { get; set; } = string.Empty;

        // Knowledge Base (Q&A pairs for the restaurant)
        public List<KnowledgeBaseItemDTO> KnowledgeBaseItems { get; set; } = new List<KnowledgeBaseItemDTO>();

        // Subscription Plan
        public string PlanName { get; set; } = string.Empty; // e.g., "Monthly", "Yearly"
        public decimal Price { get; set; }

        // Payment Card Details (prototype - in production, use payment gateway)
        public string CardHolderName { get; set; } = string.Empty;
        public string CardNumber { get; set; } = string.Empty;
        public int CardExpiryMonth { get; set; }
        public int CardExpiryYear { get; set; }
        public string CardCvv { get; set; } = string.Empty;
    }

    /// <summary>
    /// Knowledge base item (Question/Answer pair)
    /// </summary>
    public class KnowledgeBaseItemDTO
    {
        public string Question { get; set; } = string.Empty;
        public string Answer { get; set; } = string.Empty;
    }
}

