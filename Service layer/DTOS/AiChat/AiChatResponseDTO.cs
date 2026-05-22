using System.Text.Json.Serialization;

namespace Service_layer.DTOS.AiChat
{
    public class AiChatResponseDTO
    {
        [JsonPropertyName("reply")]
        public string? Reply { get; set; }

        [JsonPropertyName("response")]
        public string? Response { get; set; }

        [JsonPropertyName("session_id")]
        public string? SessionId { get; set; }

        [JsonPropertyName("intent")]
        public string? Intent { get; set; }

        [JsonPropertyName("order")]
        public object? Order { get; set; }

        public string GetReplyText() => Reply ?? Response ?? string.Empty;
    }
}
