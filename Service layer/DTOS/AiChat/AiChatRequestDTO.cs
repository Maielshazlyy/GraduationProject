using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Service_layer.DTOS.AiChat
{
    /// <summary>
    /// Request sent to the external AI.
    ///
    /// For WebChat: send <see cref="Message"/> (text).
    /// For Voice:   send <see cref="AudioData"/> (base64) + <see cref="AudioFormat"/>.
    ///              The AI performs STT, processes, and returns the transcript + reply audio.
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

        /// <summary>Channel: "WebChat" or "Voice". Tells the AI whether to do STT/TTS.</summary>
        [JsonPropertyName("channel")]
        public string Channel { get; set; } = "WebChat";

        /// <summary>Text message (WebChat). Omitted from the payload when null.</summary>
        [JsonPropertyName("message")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Message { get; set; }

        /// <summary>Base64-encoded audio (Voice only). Omitted from the payload when null.</summary>
        [JsonPropertyName("audio_data")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? AudioData { get; set; }

        /// <summary>Audio format, e.g. "audio/wav" (Voice only). Omitted when null.</summary>
        [JsonPropertyName("audio_format")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? AudioFormat { get; set; }
    }
}
