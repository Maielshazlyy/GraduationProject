using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Service_layer.DTOS.AiAnalysis
{
    /// <summary>
    /// Returned by the AI after analyzing completed chat sessions.
    /// POST /api/v1/analysis/chat-batch
    /// </summary>
    public class AiAnalysisResponseDTO
    {
        [JsonPropertyName("businessId")]
        public string BusinessId { get; set; } = string.Empty;

        [JsonPropertyName("results")]
        public List<AiSessionAnalysisResultDTO> Results { get; set; } = new();
    }

    public class AiSessionAnalysisResultDTO
    {
        /// <summary>Maps back to Interaction.InteractionId.</summary>
        [JsonPropertyName("sessionId")]
        public string SessionId { get; set; } = string.Empty;

        /// <summary>English summary of the conversation.</summary>
        [JsonPropertyName("summary")]
        public string Summary { get; set; } = string.Empty;

        /// <summary>Arabic summary of the conversation.</summary>
        [JsonPropertyName("summaryAr")]
        public string SummaryAr { get; set; } = string.Empty;

        /// <summary>
        /// Overall sentiment for the session.
        /// Fallback if undetermined: { score: 0.0, label: "Neutral" }
        /// </summary>
        [JsonPropertyName("overallSentiment")]
        public AiSentimentResultDTO OverallSentiment { get; set; } = new();

        /// <summary>
        /// The single dominant intent for the session (always equals intentsDetected[0].name).
        /// Never null — AI returns "Unknown" when undetermined.
        /// </summary>
        [JsonPropertyName("mainIntent")]
        public string MainIntent { get; set; } = "Unknown";

        /// <summary>
        /// Intent distribution detected across the session.
        /// Fallback if undetermined: [{ name: "Unknown", count: 1 }]
        /// </summary>
        [JsonPropertyName("intentsDetected")]
        public List<AiIntentResultDTO> IntentsDetected { get; set; } = new();

        /// <summary>Main topics discussed in the session.</summary>
        [JsonPropertyName("mainTopics")]
        public List<string> MainTopics { get; set; } = new();

        /// <summary>Most important moments in the conversation.</summary>
        [JsonPropertyName("keyMoments")]
        public List<string> KeyMoments { get; set; } = new();
    }

    public class AiSentimentResultDTO
    {
        /// <summary>Range: -1.0 (Very Negative) → 0.0 (Neutral) → +1.0 (Very Positive)</summary>
        [JsonPropertyName("score")]
        public double Score { get; set; } = 0.0;

        /// <summary>Positive | Neutral | Negative</summary>
        [JsonPropertyName("label")]
        public string Label { get; set; } = "Neutral";
    }

    public class AiIntentResultDTO
    {
        /// <summary>
        /// Supported values: CreateOrder, ModifyOrder, CancelOrder, AskAboutProducts,
        /// AskAboutPrice, Complaint, RequestHumanAgent, Compliment, Greeting,
        /// Farewell, GeneralQuestion, Unknown
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>Number of times this intent appeared in the session.</summary>
        [JsonPropertyName("count")]
        public int Count { get; set; } = 1;
    }
}
