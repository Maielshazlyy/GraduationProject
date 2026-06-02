using System.Text.Json.Serialization;

namespace Service_layer.DTOS.AiChat
{
    /// <summary>
    /// Request sent to the AI for Voice channel interactions.
    /// The AI performs STT on the audio, processes the intent,
    /// and returns a text reply + TTS audio.
    ///
    /// ⚠️ This REST shape is a placeholder.
    /// The real Voice flow will use WebSocket (SignalR) for real-time audio streaming.
    /// </summary>
    public class AiVoiceRequestDTO
    {
        [JsonPropertyName("session_id")]
        public string SessionId { get; set; } = string.Empty;

        [JsonPropertyName("business_id")]
        public string BusinessId { get; set; } = string.Empty;

        /// <summary>Base64-encoded raw audio from the customer's device.</summary>
        [JsonPropertyName("audio_data")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? AudioData { get; set; }

        /// <summary>MIME type of the audio, e.g. "audio/wav".</summary>
        [JsonPropertyName("audio_format")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? AudioFormat { get; set; }

        /// <summary>Optional text fallback when audio is not available.</summary>
        [JsonPropertyName("message")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Message { get; set; }
    }

    /// <summary>
    /// AI response for Voice channel — extends the base Chat response with
    /// STT transcript and TTS reply audio.
    /// All business signals (order, ticket, escalation) are inherited from
    /// <see cref="AiChatResponseDTO"/>.
    /// </summary>
    public class AiVoiceResponseDTO : AiChatResponseDTO
    {
        /// <summary>
        /// Speech-to-text transcript of what the customer said.
        /// Backend stores this as the customer Message content.
        /// </summary>
        [JsonPropertyName("transcript")]
        public string? Transcript { get; set; }

        /// <summary>Base64-encoded TTS audio of the reply.</summary>
        [JsonPropertyName("reply_audio")]
        public string? ReplyAudio { get; set; }

        /// <summary>MIME type of <see cref="ReplyAudio"/>, e.g. "audio/wav".</summary>
        [JsonPropertyName("reply_audio_format")]
        public string? ReplyAudioFormat { get; set; }
    }
}
