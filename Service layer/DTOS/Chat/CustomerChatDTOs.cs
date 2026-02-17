using System;
using System.Collections.Generic;

namespace Service_layer.DTOS.Chat
{
    /// <summary>
    /// Request sent from frontend customer chat widget or voice call.
    /// </summary>
    public class CustomerChatRequestDTO
    {
        public string BusinessId { get; set; } = string.Empty;

        /// <summary>
        /// Optional known customer id (if the customer is logged in).
        /// </summary>
        public string? CustomerId { get; set; }

        /// <summary>
        /// Existing interaction id. If null, backend will create a new interaction.
        /// </summary>
        public string? InteractionId { get; set; }

        /// <summary>
        /// Channel type: "WebChat" or "Voice". Defaults to "WebChat".
        /// </summary>
        public string Channel { get; set; } = "WebChat";

        /// <summary>
        /// For Voice calls: Unique session identifier from telephony provider.
        /// </summary>
        public string? CallSessionId { get; set; }

        /// <summary>
        /// The plain text message from the customer (for Chat).
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// Base64 encoded audio data (for Voice).
        /// </summary>
        public string? AudioData { get; set; }

        /// <summary>
        /// Audio format (e.g., "audio/wav", "audio/mp3", "audio/webm").
        /// </summary>
        public string? AudioFormat { get; set; }
    }

    /// <summary>
    /// Response returned to the chat widget or voice call.
    /// </summary>
    public class CustomerChatResponseDTO
    {
        public string InteractionId { get; set; } = string.Empty;
        public string ReplyText { get; set; } = string.Empty;

        /// <summary>
        /// Base64 encoded audio response (for Voice channel).
        /// </summary>
        public string? ReplyAudio { get; set; }

        /// <summary>
        /// Audio format of the reply (e.g., "audio/wav", "audio/mp3").
        /// </summary>
        public string? ReplyAudioFormat { get; set; }

        public string? OrderId { get; set; }
        public string? TicketId { get; set; }

        public ChatCartSummaryDTO? Cart { get; set; }

        public List<RecommendationItemDTO> Recommendations { get; set; } = new();

        /// <summary>
        /// Indicates if delivery delay was detected (for Voice orders).
        /// </summary>
        public bool? HasDeliveryDelay { get; set; }

        /// <summary>
        /// Suggested alternative time slots if delay detected.
        /// </summary>
        public List<string>? AlternativeTimeSlots { get; set; }

        /// <summary>
        /// Indicates if interaction was interrupted (for recovery).
        /// </summary>
        public bool IsInterrupted { get; set; } = false;
    }

    /// <summary>
    /// Request from customer site (order page) to get recommendations
    /// for a main menu item without going through chat/voice.
    /// </summary>
    public class CustomerOrderRecommendationRequestDTO
    {
        /// <summary>
        /// Business that owns the menu.
        /// </summary>
        public string BusinessId { get; set; } = string.Empty;

        /// <summary>
        /// The main menu item the customer selected (e.g., burger).
        /// </summary>
        public string MainMenuItemId { get; set; } = string.Empty;
    }

    public class ChatCartSummaryDTO
    {
        public decimal TotalPrice { get; set; }
        public List<ChatCartItemDTO> Items { get; set; } = new();
    }

    public class ChatCartItemDTO
    {
        public string MenuItemId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }

    /// <summary>
    /// Recommendation item computed by backend (upsell / cross-sell).
    /// </summary>
    public class RecommendationItemDTO
    {
        public string MenuItemId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Reason { get; set; } = string.Empty;
    }

    /// <summary>
    /// Result of intent detection from AI layer.
    /// </summary>
    public class DetectedIntentResultDTO
    {
        public string Intent { get; set; } = "GeneralQuestion";
        public Dictionary<string, string> Entities { get; set; } = new();
        public double Confidence { get; set; }
        public bool RequiresAction { get; set; }

        /// <summary>
        /// Detected dialect (e.g., "Egyptian", "Standard Arabic", "English").
        /// </summary>
        public string? DetectedDialect { get; set; }

        /// <summary>
        /// Conversation complexity level as assessed by AI: Low, Medium, High.
        /// Used for escalation decisions.
        /// </summary>
        public string? ComplexityLevel { get; set; }

        /// <summary>
        /// Whether AI recommends escalating to a human agent.
        /// Backend still makes the final decision.
        /// </summary>
        public bool RequiresEscalation { get; set; }

        /// <summary>
        /// Suggested ticket priority: Low, Normal, High, Critical.
        /// </summary>
        public string? PriorityLevel { get; set; }

        /// <summary>
        /// Human-readable reason for escalation to support audit & reporting.
        /// </summary>
        public string? EscalationReason { get; set; }
    }

    /// <summary>
    /// Available communication channels/capabilities for a business.
    /// </summary>
    public class CustomerCapabilitiesDTO
    {
        public string BusinessId { get; set; } = string.Empty;
        public string BusinessName { get; set; } = string.Empty;
        public bool ChatEnabled { get; set; } = true;
        public bool VoiceEnabled { get; set; } = true;
        public string? WelcomeMessage { get; set; }
        public VoiceSettingsDTO? VoiceSettings { get; set; }
    }

    /// <summary>
    /// Voice settings for the AI agent (from Business Settings).
    /// </summary>
    public class VoiceSettingsDTO
    {
        public string AgentVoice { get; set; } = string.Empty;
        public string AgentVoiceProvider { get; set; } = string.Empty;
        public double AgentVoiceSpeed { get; set; } = 1.0;
        public double AgentVoicePitch { get; set; } = 1.0;
        public string AgentVoiceLanguage { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO for collecting voice feedback at end of call.
    /// </summary>
    public class VoiceFeedbackDTO
    {
        public string InteractionId { get; set; } = string.Empty;
        public int Rating { get; set; } // 1-5
        public string? Comment { get; set; }
    }
}


