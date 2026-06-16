using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Service_layer.DTOS.AiAnalysis
{
    /// <summary>
    /// Sent by the backend to the AI after one or more sessions are completed.
    /// POST /api/v1/analysis/chat-batch
    /// </summary>
    public class AiAnalysisRequestDTO
    {
        [JsonPropertyName("businessId")]
        public string BusinessId { get; set; } = string.Empty;

        [JsonPropertyName("sessions")]
        public List<AiAnalysisSessionDTO> Sessions { get; set; } = new();
    }

    public class AiAnalysisSessionDTO
    {
        /// <summary>Maps to Interaction.InteractionId in the backend DB.</summary>
        [JsonPropertyName("sessionId")]
        public string SessionId { get; set; } = string.Empty;

        [JsonPropertyName("messages")]
        public List<AiAnalysisMessageDTO> Messages { get; set; } = new();
    }

    public class AiAnalysisMessageDTO
    {
        /// <summary>customer or assistant</summary>
        [JsonPropertyName("role")]
        public string Role { get; set; } = string.Empty;

        [JsonPropertyName("text")]
        public string Text { get; set; } = string.Empty;
    }
}
