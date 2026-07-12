using System.Text.Json.Serialization;

namespace Service_layer.DTOS.CallSummary
{
    public class CallSummaryPayloadDTO
    {
        [JsonPropertyName("call_data")]
        public CallDataDTO CallData { get; set; } = new();

        [JsonPropertyName("analysis")]
        public AnalysisDTO Analysis { get; set; } = new();

        [JsonPropertyName("queued_at")]
        public DateTime QueuedAt { get; set; }
    }

    public class CallDataDTO
    {
        [JsonPropertyName("call_id")]
        public string CallId { get; set; } = string.Empty;

        [JsonPropertyName("start_time")]
        public DateTime StartTime { get; set; }

        [JsonPropertyName("end_time")]
        public DateTime EndTime { get; set; }

        [JsonPropertyName("duration_seconds")]
        public double DurationSeconds { get; set; }

        [JsonPropertyName("messages_count")]
        public int MessagesCount { get; set; }

        [JsonPropertyName("messages")]
        public List<CallMessageDTO> Messages { get; set; } = new();

        [JsonPropertyName("full_transcript")]
        public string FullTranscript { get; set; } = string.Empty;

        [JsonPropertyName("audio_files")]
        public AudioFilesDTO? AudioFiles { get; set; }

        [JsonPropertyName("audio_info")]
        public AudioInfoDTO? AudioInfo { get; set; }
    }

    public class CallMessageDTO
    {
        [JsonPropertyName("turnIndex")]
        public int TurnIndex { get; set; }

        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }

        [JsonPropertyName("speaker")]
        public string Speaker { get; set; } = string.Empty;

        [JsonPropertyName("transcript")]
        public string Transcript { get; set; } = string.Empty;

        [JsonPropertyName("audioLengthMs")]
        public int AudioLengthMs { get; set; }

        [JsonPropertyName("language")]
        public string? Language { get; set; }

        [JsonPropertyName("dialect")]
        public string? Dialect { get; set; }

        [JsonPropertyName("intentDetected")]
        public string? IntentDetected { get; set; }

        [JsonPropertyName("actionTaken")]
        public string? ActionTaken { get; set; }

        [JsonPropertyName("sentiment")]
        public object? Sentiment { get; set; }
    }

    public class AudioFilesDTO
    {
        [JsonPropertyName("customer")]
        public string? Customer { get; set; }

        [JsonPropertyName("agent")]
        public string? Agent { get; set; }

        [JsonPropertyName("stereo")]
        public string? Stereo { get; set; }
    }

    public class AudioInfoDTO
    {
        [JsonPropertyName("sample_rate")]
        public int SampleRate { get; set; }

        [JsonPropertyName("bit_depth")]
        public int BitDepth { get; set; }

        [JsonPropertyName("customer_duration_sec")]
        public double CustomerDurationSec { get; set; }

        [JsonPropertyName("agent_duration_sec")]
        public double AgentDurationSec { get; set; }
    }

    public class AnalysisDTO
    {
        [JsonPropertyName("callId")]
        public string CallId { get; set; } = string.Empty;

        [JsonPropertyName("durationSeconds")]
        public double DurationSeconds { get; set; }

        [JsonPropertyName("analyzedAt")]
        public DateTime AnalyzedAt { get; set; }

        [JsonPropertyName("summary")]
        public string Summary { get; set; } = string.Empty;

        [JsonPropertyName("summaryAr")]
        public string SummaryAr { get; set; } = string.Empty;

        [JsonPropertyName("overallSentiment")]
        public SentimentResultDTO? OverallSentiment { get; set; }

        [JsonPropertyName("mainTopics")]
        public List<string> MainTopics { get; set; } = new();

        [JsonPropertyName("intentsDetected")]
        public List<string> IntentsDetected { get; set; } = new();

        [JsonPropertyName("actionsPerformed")]
        public List<object> ActionsPerformed { get; set; } = new();

        [JsonPropertyName("escalationRequired")]
        public bool EscalationRequired { get; set; }

        [JsonPropertyName("escalationReason")]
        public string? EscalationReason { get; set; }

        [JsonPropertyName("keyMoments")]
        public List<KeyMomentDTO> KeyMoments { get; set; } = new();

    }

    public class SentimentResultDTO
    {
        [JsonPropertyName("score")]
        public double Score { get; set; }

        [JsonPropertyName("label")]
        public string Label { get; set; } = string.Empty;
    }

    public class KeyMomentDTO
    {
        [JsonPropertyName("turnIndex")]
        public int TurnIndex { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;
    }
}
