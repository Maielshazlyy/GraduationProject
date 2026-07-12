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

            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException(
                    $"AI /api/v1/chat returned {(int)response.StatusCode} {response.StatusCode} " +
                    $"(base={_httpClient.BaseAddress}). Body: {errorBody}");
            }

            var result = await response.Content.ReadFromJsonAsync<AiChatResponseDTO>();
            return result ?? new AiChatResponseDTO { Reply = string.Empty, SessionId = request.SessionId };
        }

    }
}
