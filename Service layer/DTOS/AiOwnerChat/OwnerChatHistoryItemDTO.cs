using System;

namespace Service_layer.DTOS.AiOwnerChat
{
    /// <summary>
    /// One saved Owner Chat exchange, returned to the frontend so past messages
    /// can be shown again when the owner reopens "Chat with IRIS".
    /// </summary>
    public class OwnerChatHistoryItemDTO
    {
        public string Message { get; set; } = string.Empty;
        public string Reply { get; set; } = string.Empty;
        public string? Confidence { get; set; }
        public DateTime SentAt { get; set; }
    }
}
