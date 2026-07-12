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

        // Contact Information
        public string? Email { get; set; }
        public string? Website { get; set; }
        public string? FacebookUrl { get; set; }
        public string? InstagramUrl { get; set; }

        // Location
        public string? City { get; set; }
        public string? Country { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        // Restaurant Information
        public string? Description { get; set; }
        public string? CuisineType { get; set; }
        public string? PriceRange { get; set; }
        public string? LogoUrl { get; set; }
        public string? CoverImageUrl { get; set; }

        // Features & Services
        public bool HasDelivery { get; set; } = false;
        public bool HasTakeout { get; set; } = false;
        public bool HasParking { get; set; } = false;
        public bool HasWiFi { get; set; } = false;
        public bool HasOutdoorSeating { get; set; } = false;
        public bool AcceptsReservations { get; set; } = false;

        // Payment Methods
        public string? PaymentMethods { get; set; }

        // Working Hours
        public List<WorkingHoursItemDTO> WorkingHours { get; set; } = new List<WorkingHoursItemDTO>();

        // Agent/Chatbot Configuration
        public string AgentName { get; set; } = string.Empty;
        public string AgentTone { get; set; } = "Friendly"; // e.g., Friendly, Professional, Casual
        public string WelcomeMessage { get; set; } = string.Empty;

        // Knowledge Base (Q&A pairs for the restaurant)
        public List<KnowledgeBaseItemDTO> KnowledgeBaseItems { get; set; } = new List<KnowledgeBaseItemDTO>();

        // Menu Categories (فئات القائمة)
        public List<MenuCategoryItemDTO> MenuCategories { get; set; } = new List<MenuCategoryItemDTO>();

        // Menu Items (عناصر القائمة)
        public List<MenuItemOnboardingDTO> MenuItems { get; set; } = new List<MenuItemOnboardingDTO>();

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

    /// <summary>
    /// Menu Category for onboarding
    /// </summary>
    public class MenuCategoryItemDTO
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int DisplayOrder { get; set; } = 0;
    }

    /// <summary>
    /// Menu Item for onboarding
    /// </summary>
    public class MenuItemOnboardingDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string? MenuCategoryName { get; set; } // اسم الفئة (سيتم البحث عنها أو إنشاؤها)
        public bool IsAvailable { get; set; } = true;
    }

    /// <summary>
    /// Working Hours for onboarding
    /// </summary>
    public class WorkingHoursItemDTO
    {
        public int DayOfWeek { get; set; } // 0 = Sunday, 1 = Monday, ..., 6 = Saturday
        public string? OpenTime { get; set; } // Format: "HH:mm" e.g., "09:00"
        public string? CloseTime { get; set; } // Format: "HH:mm" e.g., "22:00"
        public bool IsClosed { get; set; } = false;
    }
}

