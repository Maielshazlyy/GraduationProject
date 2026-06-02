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
            var response = await _httpClient.PostAsJsonAsync("/api/v1/chat", request);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<AiChatResponseDTO>();
            return result ?? new AiChatResponseDTO { Reply = string.Empty, SessionId = request.SessionId };
        }

        public async Task<AiVoiceResponseDTO> SendVoiceAsync(AiVoiceRequestDTO request)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/v1/chat", request);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<AiVoiceResponseDTO>();
            return result ?? new AiVoiceResponseDTO { Reply = string.Empty, SessionId = request.SessionId };
        }
    }
}
