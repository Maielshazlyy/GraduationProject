using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Service_layer.DTOS.AiChat;
using Service_layer.Services_Interfaces;

namespace Service_layer.Services
{
    public class AiChatService : IAiChatService
    {
        private readonly HttpClient _httpClient;

        public AiChatService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<AiChatResponseDTO> SendMessageAsync(AiChatRequestDTO request)
        {
            // Serialize the DTO directly so [JsonIgnore(WhenWritingNull)] applies —
            // WebChat requests omit audio fields, Voice requests omit message when null.
            var response = await _httpClient.PostAsJsonAsync("/api/v1/chat", request);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<AiChatResponseDTO>();
            return result ?? new AiChatResponseDTO { Reply = string.Empty, SessionId = request.SessionId };
        }

        public async Task InitSessionAsync(AiSessionInitDTO init)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/v1/session/init", init);
            response.EnsureSuccessStatusCode();
        }
    }
}
