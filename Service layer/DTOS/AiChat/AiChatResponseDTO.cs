using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Service_layer.DTOS.AiChat
{
    /// <summary>
    /// Full response returned by the external AI chatbot.
    ///
    /// CONTRACT — AI is responsible for:
    ///   • Generating the natural-language reply.
    ///   • Detecting order intent, ticket intent, escalation need, feedback need.
    ///   • Returning structured details so the backend can act.
    ///
    /// Backend is responsible for:
    ///   • Creating orders, tickets, escalations in its own DB.
    ///   • Using AI signals (flags below) to decide WHAT to create.
    ///   • Ignoring AI-side system_events (those are AI-internal tracking only).
    /// </summary>
    public class AiChatResponseDTO
    {
        [JsonPropertyName("session_id")]
        public string? SessionId { get; set; }

        /// <summary>Natural-language reply to show the customer (text).</summary>
        [JsonPropertyName("reply")]
        public string? Reply { get; set; }

        // ── Voice (STT/TTS handled by AI) ────────────────────────────────

        /// <summary>
        /// Speech-to-text transcript of what the customer said (Voice channel).
        /// Backend stores this as the customer message content.
        /// </summary>
        [JsonPropertyName("transcript")]
        public string? Transcript { get; set; }

        /// <summary>Base64-encoded text-to-speech audio of the reply (Voice channel).</summary>
        [JsonPropertyName("reply_audio")]
        public string? ReplyAudio { get; set; }

        /// <summary>Format of <see cref="ReplyAudio"/>, e.g. "audio/wav".</summary>
        [JsonPropertyName("reply_audio_format")]
        public string? ReplyAudioFormat { get; set; }

        // ── Order signals ────────────────────────────────────────────────

        /// <summary>
        /// True while the customer is actively building a cart.
        /// Does NOT mean the order should be persisted yet.
        /// </summary>
        [JsonPropertyName("order_detected")]
        public bool OrderDetected { get; set; }

        /// <summary>
        /// True when the customer has confirmed the order ("تمام كدة" / "confirm").
        /// Backend MUST create the order in DB when this is true.
        /// </summary>
        [JsonPropertyName("order_finalized")]
        public bool OrderFinalized { get; set; }

        /// <summary>
        /// Current cart state: items (name, qty, price) and running total.
        /// Present whenever order_detected OR order_finalized is true.
        /// Backend matches item names against the menu DB to get MenuItemId.
        /// </summary>
        [JsonPropertyName("order_details")]
        public AiOrderDetailsDTO? OrderDetails { get; set; }

        // ── Ticket signals ───────────────────────────────────────────────

        /// <summary>
        /// True when a complaint/issue is detected.
        /// Backend MUST create a support ticket in DB when this is true.
        /// </summary>
        [JsonPropertyName("ticket_detected")]
        public bool TicketDetected { get; set; }

        /// <summary>
        /// Structured complaint data for populating the ticket.
        /// Present whenever ticket_detected is true.
        /// </summary>
        [JsonPropertyName("ticket_details")]
        public AiTicketDetailsDTO? TicketDetails { get; set; }

        // ── Escalation & Feedback signals ────────────────────────────────

        /// <summary>
        /// True when the customer requests a human agent or AI detects high frustration.
        /// Backend MUST create a HumanEscalation ticket and update interaction status.
        /// </summary>
        [JsonPropertyName("escalation_requested")]
        public bool EscalationRequested { get; set; }

        /// <summary>
        /// True when the AI wants the backend to prompt the customer for a rating.
        /// Backend stores the feedback against the current interaction/ticket.
        /// </summary>
        [JsonPropertyName("feedback_requested")]
        public bool FeedbackRequested { get; set; }

        [JsonPropertyName("processing_time_ms")]
        public int ProcessingTimeMs { get; set; }

        /// <summary>Returns the reply text, falling back to empty string.</summary>
        public string GetReplyText() => Reply ?? string.Empty;
    }

    // ── Nested DTOs ───────────────────────────────────────────────────────

    /// <summary>Current cart state returned by the AI.</summary>
    public class AiOrderDetailsDTO
    {
        [JsonPropertyName("intent")]
        public string? Intent { get; set; }

        /// <summary>Items in the cart as the AI currently understands them.</summary>
        [JsonPropertyName("items")]
        public List<AiOrderItemDTO> Items { get; set; } = new();

        /// <summary>Running total computed by the AI (informational; backend recomputes from menu prices).</summary>
        [JsonPropertyName("total_amount")]
        public decimal TotalAmount { get; set; }
    }

    public class AiOrderItemDTO
    {
        /// <summary>Display name of the item — backend matches this against MenuItem.Name.</summary>
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; } = 1;

        /// <summary>Price hint from AI (informational; backend uses the menu DB price as authoritative).</summary>
        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        [JsonPropertyName("notes")]
        public string? Notes { get; set; }
    }

    /// <summary>Complaint details extracted by the AI.</summary>
    public class AiTicketDetailsDTO
    {
        [JsonPropertyName("subject")]
        public string Subject { get; set; } = "Customer Complaint";

        /// <summary>Full description of the issue as stated by the customer.</summary>
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        /// <summary>AI-suggested priority: low | normal | high | critical.</summary>
        [JsonPropertyName("priority")]
        public string Priority { get; set; } = "Normal";

        /// <summary>
        /// Issue category used as TicketType: complaint | quality | delivery | payment | wrong_order | etc.
        /// Maps to Ticket.TicketType in backend.
        /// </summary>
        [JsonPropertyName("category")]
        public string? Category { get; set; }
    }
}
