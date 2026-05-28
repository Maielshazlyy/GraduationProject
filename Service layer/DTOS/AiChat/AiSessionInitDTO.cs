using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Service_layer.DTOS.AiChat
{
    /// <summary>
    /// Sent by the backend to the AI when a NEW conversation starts, to prime the
    /// session with the business's menu + knowledge base. The AI caches this against
    /// the session_id so subsequent chat messages stay lightweight.
    ///
    /// This is a PUSH model: the backend already owns this data in its DB, so it
    /// hands it to the AI directly — the AI never calls back into the backend
    /// (no machine-to-machine auth needed).
    /// </summary>
    public class AiSessionInitDTO
    {
        [JsonPropertyName("session_id")]
        public string SessionId { get; set; } = string.Empty;

        [JsonPropertyName("business_id")]
        public string BusinessId { get; set; } = string.Empty;

        [JsonPropertyName("business_name")]
        public string? BusinessName { get; set; }

        /// <summary>The business menu. AI must echo item Names exactly in order_details.</summary>
        [JsonPropertyName("menu")]
        public List<AiMenuItemDTO> Menu { get; set; } = new();

        /// <summary>Internal knowledge base + FAQs for answering business questions.</summary>
        [JsonPropertyName("knowledge_base")]
        public List<AiKnowledgeEntryDTO> KnowledgeBase { get; set; } = new();
    }

    public class AiMenuItemDTO
    {
        [JsonPropertyName("menu_item_id")]
        public string MenuItemId { get; set; } = string.Empty;

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

        /// <summary>true = public FAQ, false = internal knowledge base.</summary>
        [JsonPropertyName("is_faq")]
        public bool IsFaq { get; set; }
    }
}
