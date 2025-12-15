using System;
using System.Collections.Generic;
using Service_layer.DTOS.KnowledgeBase;

namespace Service_layer.DTOS.Business
{
    /// <summary>
    /// Full onboarding payload for a new business (restaurant) owner.
    /// </summary>
    public class BusinessOnboardingDTO
    {
        // Business basic info
        public string Name { get; set; }
        public string Type { get; set; } = "Restaurant";
        public string Address { get; set; }
        public string Phone { get; set; }

        // Agent / chatbot configuration
        public string AgentName { get; set; }
        public string AgentTone { get; set; }  // maps to ChatbotPersonality
        public string WelcomeMessage { get; set; }

        // Knowledge base seed data (for restaurant menu, FAQs, etc.)
        public List<KnowledgeBaseItemDTO> KnowledgeBaseItems { get; set; } = new();

        // Subscription
        
        // e.g. \"Monthly\", \"Yearly\"
        
        public string PlanName { get; set; }
        public decimal Price { get; set; }

        // Payment (simple prototype – in real life you'd use a payment gateway token)
        public string CardHolderName { get; set; }
        public string CardNumber { get; set; }
        public string CardExpiryMonth { get; set; }
        public string CardExpiryYear { get; set; }
        public string CardCvv { get; set; }
    }

    public class KnowledgeBaseItemDTO
    {
        public string Question { get; set; }
        public string Answer { get; set; }
    }
}


