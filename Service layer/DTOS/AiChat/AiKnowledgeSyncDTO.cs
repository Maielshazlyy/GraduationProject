using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Service_layer.DTOS.AiChat
{
    /// <summary>
    /// Sent by the backend to the AI when a business knowledge base is created or updated.
    ///
    /// CONTRACT:
    ///   • This is NOT session-based — no session_id.
    ///   • The AI indexes this data by business_id.
    ///   • Called once when a business is created, and again on every menu/KB change.
    ///   • During chat, the AI uses business_id to select the correct indexed knowledge base.
    /// </summary>
    public class AiKnowledgeSyncDTO
    {
        [JsonPropertyName("business_id")]
        public string BusinessId { get; set; } = string.Empty;

        [JsonPropertyName("business_name")]
        public string? BusinessName { get; set; }

        [JsonPropertyName("knowledge_base")]
        public AiKnowledgeBaseDTO KnowledgeBase { get; set; } = new();
    }

    public class AiKnowledgeBaseDTO
    {
        /// <summary>Menu items — used for product answers and order item name extraction.</summary>
        [JsonPropertyName("menu_items")]
        public List<AiMenuItemDTO> MenuItems { get; set; } = new();

        /// <summary>FAQs — used to answer customer questions about the business.</summary>
        [JsonPropertyName("faqs")]
        public List<AiKnowledgeEntryDTO> Faqs { get; set; } = new();
    }

    public class AiMenuItemDTO
    {
        [JsonPropertyName("menu_item_id")]
        public string MenuItemId { get; set; } = string.Empty;

        /// <summary>Exact canonical name — the AI must echo this in order_details.items[].name.</summary>
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        [JsonPropertyName("category")]
        public string? Category { get; set; }

        [JsonPropertyName("is_available")]
        public bool IsAvailable { get; set; }
    }

    public class AiKnowledgeEntryDTO
    {
        [JsonPropertyName("question")]
        public string Question { get; set; } = string.Empty;

        [JsonPropertyName("answer")]
        public string Answer { get; set; } = string.Empty;

        [JsonPropertyName("is_faq")]
        public bool IsFaq { get; set; }
    }

}
