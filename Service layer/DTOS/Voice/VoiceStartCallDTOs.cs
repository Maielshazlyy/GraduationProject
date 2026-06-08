using System.Text.Json.Serialization;

namespace Service_layer.DTOS.Voice
{
    /// <summary>
    /// Sent by the frontend when the customer clicks the "Call" button.
    /// Backend creates an Interaction, reads the MeetingUrl from Settings,
    /// notifies the AI to join, then returns the URL to the frontend.
    /// </summary>
    public class StartVoiceCallRequestDTO
    {
        public string BusinessId { get; set; } = string.Empty;

        /// <summary>The fixed meeting URL the frontend already has.</summary>
        public string MeetingUrl { get; set; } = string.Empty;

        /// <summary>Optional — if the customer is identified (logged in / known phone).</summary>
        public string? CustomerId { get; set; }
    }

    /// <summary>
    /// Returned to the frontend immediately after the call is started.
    /// </summary>
    public class StartVoiceCallResponseDTO
    {
        public string InteractionId { get; set; } = string.Empty;

        /// <summary>"connecting" — AI has been notified; call is being set up.</summary>
        public string Status { get; set; } = "connecting";
    }

    /// <summary>
    /// Payload the backend sends to the Voice AI to tell it to join the call.
    /// Shape must match what the AI team expects on their /voice/join endpoint.
    /// </summary>
    public class AiVoiceJoinRequestDTO
    {
        [JsonPropertyName("interaction_id")]
        public string InteractionId { get; set; } = string.Empty;

        [JsonPropertyName("business_id")]
        public string BusinessId { get; set; } = string.Empty;

        [JsonPropertyName("meeting_url")]
        public string MeetingUrl { get; set; } = string.Empty;
    }
}
