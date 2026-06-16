using System.Text.Json.Serialization;

namespace Service_layer.DTOS.AiChat
{
    /// <summary>
    /// Request sent to the external AI for WebChat (text only).
    /// Voice requests use <see cref="AiVoiceRequestDTO"/> instead.
    /// </summary>
    public class AiChatRequestDTO
    {
        [JsonPropertyName("session_id")]
        public string SessionId { get; set; } = string.Empty;

        /// <summary>
        /// The business this conversation belongs to. The AI uses this to load the
        /// correct menu + knowledge base for answering and for order item names.
        /// </summary>
        [JsonPropertyName("business_id")]
        public string BusinessId { get; set; } = string.Empty;

        /// <summary>Customer text message.</summary>
        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;
    }
}
