using System.Net.Http.Json;
using System.Diagnostics;
using Service_layer.DTOS.Voice;
using Service_layer.Services_Interfaces;

namespace Service_layer.Services
{
    /// <summary>
    /// HTTP client that tells the Voice AI to join an active meeting room.
    /// Endpoint the AI exposes: POST /voice/join
    /// </summary>
    public class AiVoiceJoinService : IAiVoiceJoinService
    {
        private readonly HttpClient _http;

        public AiVoiceJoinService(HttpClient http)
        {
            _http = http;
        }

        public async Task JoinCallAsync(string interactionId, string businessId, string customerIdentifier)
        {
            var payload = new AiVoiceJoinRequestDTO
            {
                InteractionId      = interactionId,
                BusinessId         = businessId,
                CustomerIdentifier = customerIdentifier
            };

            try
            {
                var response = await _http.PostAsJsonAsync("/api/voice/start-call", payload);
                if (!response.IsSuccessStatusCode)
                {
                    Debug.WriteLine($"[AiVoiceJoinService] AI returned {(int)response.StatusCode} for interactionId={interactionId}");
                }
            }
            catch (Exception ex)
            {
                // Non-fatal — the Interaction is already created; AI can be retried later
                Debug.WriteLine($"[AiVoiceJoinService] Failed to notify AI: {ex.Message}");
            }
        }
    }
}
