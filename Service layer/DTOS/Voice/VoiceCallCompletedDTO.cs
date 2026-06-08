using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Service_layer.DTOS.Voice
{
    /// <summary>
    /// Payload the autonomous Voice AI POSTs to the backend AFTER a call ends.
    /// The backend is a data sink here — it stores the call, its turns, and the
    /// AI's analysis. Shape mirrors the Voice AI's actual output exactly.
    ///
    /// Endpoint (backend exposes, AI calls): POST /api/voice/call-completed
    /// </summary>
    public class VoiceCallCompletedDTO
    {
        /// <summary>
        /// The Interaction created by the backend when the call was started.
        /// The AI receives this in the join request and must echo it back here.
        /// </summary>
        [JsonPropertyName("interaction_id")]
        public string InteractionId { get; set; } = string.Empty;

        /// <summary>Which business the call belongs to.</summary>
        [JsonPropertyName("business_id")]
        public string? BusinessId { get; set; }

        /// <summary>
        /// Optional caller identifier (phone/email). If absent, the call is
        /// treated as an anonymous customer.
        /// </summary>
        [JsonPropertyName("customer_identifier")]
        public string? CustomerIdentifier { get; set; }

        [JsonPropertyName("call_data")]
        public VoiceCallDataDTO CallData { get; set; } = new();

        [JsonPropertyName("analysis")]
        public VoiceAnalysisDTO Analysis { get; set; } = new();

        [JsonPropertyName("queued_at")]
        public DateTime? QueuedAt { get; set; }
    }

    /// <summary>The call itself: timing, turns, transcript, audio.</summary>
    public class VoiceCallDataDTO
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
        public List<VoiceTurnDTO> Messages { get; set; } = new();

        [JsonPropertyName("full_transcript")]
        public string? FullTranscript { get; set; }

        [JsonPropertyName("audio_files")]
        public VoiceAudioFilesDTO? AudioFiles { get; set; }

        [JsonPropertyName("audio_info")]
        public VoiceAudioInfoDTO? AudioInfo { get; set; }
    }

    /// <summary>A single turn in the call (rich per-turn metadata).</summary>
    public class VoiceTurnDTO
    {
        [JsonPropertyName("turnIndex")]
        public int TurnIndex { get; set; }

        /// <summary>ISO timestamp of when the turn occurred.</summary>
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }

        /// <summary>"Agent" or "Customer".</summary>
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

        /// <summary>Intent detected for this turn (usually populated on Agent turns).</summary>
        [JsonPropertyName("intentDetected")]
        public string? IntentDetected { get; set; }

        /// <summary>Action taken on this turn, e.g. an OrderCreated.</summary>
        [JsonPropertyName("actionTaken")]
        public VoiceActionDTO? ActionTaken { get; set; }

        /// <summary>Per-turn sentiment (usually populated on Customer turns).</summary>
        [JsonPropertyName("sentiment")]
        public VoiceSentimentDTO? Sentiment { get; set; }
    }

    /// <summary>References to the recorded audio files for the call.</summary>
    public class VoiceAudioFilesDTO
    {
        [JsonPropertyName("customer")]
        public string? Customer { get; set; }

        [JsonPropertyName("agent")]
        public string? Agent { get; set; }

        [JsonPropertyName("stereo")]
        public string? Stereo { get; set; }
    }

    /// <summary>Technical info about the recorded audio.</summary>
    public class VoiceAudioInfoDTO
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

    /// <summary>The AI's post-call analysis of the conversation.</summary>
    public class VoiceAnalysisDTO
    {
        [JsonPropertyName("callId")]
        public string? CallId { get; set; }

        [JsonPropertyName("durationSeconds")]
        public double DurationSeconds { get; set; }

        [JsonPropertyName("analyzedAt")]
        public DateTime? AnalyzedAt { get; set; }

        /// <summary>English summary of the call.</summary>
        [JsonPropertyName("summary")]
        public string? Summary { get; set; }

        /// <summary>Arabic summary of the call.</summary>
        [JsonPropertyName("summaryAr")]
        public string? SummaryAr { get; set; }

        /// <summary>Overall sentiment of the whole call.</summary>
        [JsonPropertyName("overallSentiment")]
        public VoiceSentimentDTO? OverallSentiment { get; set; }

        [JsonPropertyName("mainTopics")]
        public List<string> MainTopics { get; set; } = new();

        [JsonPropertyName("intentsDetected")]
        public List<string> IntentsDetected { get; set; } = new();

        /// <summary>
        /// Actions taken during the call, e.g. OrderCreated with item details + total.
        /// </summary>
        [JsonPropertyName("actionsPerformed")]
        public List<VoiceActionDTO> ActionsPerformed { get; set; } = new();

        [JsonPropertyName("escalationRequired")]
        public bool EscalationRequired { get; set; }

        [JsonPropertyName("escalationReason")]
        public string? EscalationReason { get; set; }

        /// <summary>Notable moments in the conversation with the turn they happened at.</summary>
        [JsonPropertyName("keyMoments")]
        public List<VoiceKeyMomentDTO> KeyMoments { get; set; } = new();

        /// <summary>AI models used (e.g. whisper, gpt-4o, tts).</summary>
        [JsonPropertyName("modelsUsed")]
        public List<string> ModelsUsed { get; set; } = new();
    }

    /// <summary>Sentiment score + label, used both per-turn and overall.</summary>
    public class VoiceSentimentDTO
    {
        [JsonPropertyName("score")]
        public double Score { get; set; }

        [JsonPropertyName("label")]
        public string? Label { get; set; }
    }

    /// <summary>
    /// An action taken by the AI during the call (e.g. OrderCreated).
    /// Used for both per-turn `actionTaken` and the analysis-level `actionsPerformed`.
    /// </summary>
    public class VoiceActionDTO
    {
        /// <summary>e.g. "OrderCreated".</summary>
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;

        /// <summary>Reference id (e.g. order id) if the AI created/used something. May be null.</summary>
        [JsonPropertyName("referenceId")]
        public string? ReferenceId { get; set; }

        [JsonPropertyName("details")]
        public VoiceActionDetailsDTO? Details { get; set; }
    }

    /// <summary>
    /// Details for an action. For OrderCreated: item names + total price.
    /// NOTE: items are NAMES only — no per-item quantity, no per-item price,
    /// no menu_item_id. Backend can record the total but cannot build a
    /// fully structured order from this alone.
    /// </summary>
    public class VoiceActionDetailsDTO
    {
        [JsonPropertyName("items")]
        public List<string> Items { get; set; } = new();

        [JsonPropertyName("totalPrice")]
        public decimal TotalPrice { get; set; }
    }

    /// <summary>A noteworthy moment in the call.</summary>
    public class VoiceKeyMomentDTO
    {
        [JsonPropertyName("turnIndex")]
        public int TurnIndex { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }
    }
}
