using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Service_layer.DTOS.AiOwnerChat;
using Service_layer.Services_Interfaces;

namespace Service_layer.Services
{
    public class AiOwnerChatService : IAiOwnerChatService
    {
        private readonly HttpClient _httpClient;

        public AiOwnerChatService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<AiOwnerChatResponseDTO> SendMessageAsync(AiOwnerChatRequestDTO request)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/v1/owner/chat", request);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<AiOwnerChatResponseDTO>();
            return result ?? new AiOwnerChatResponseDTO { Reply = string.Empty, SessionId = request.SessionId };
        }

        public async Task<AiOwnerReportResponseDTO> GetReportAsync()
        {
            var response = await _httpClient.GetAsync("/api/v1/owner/report");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<AiOwnerReportResponseDTO>();
            return result ?? new AiOwnerReportResponseDTO();
        }

        public async Task<AiOwnerReloadResponseDTO> ReloadAsync()
        {
            var response = await _httpClient.PostAsync("/api/v1/owner/reload", null);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<AiOwnerReloadResponseDTO>();
            return result ?? new AiOwnerReloadResponseDTO { Status = "success", ReportsLoaded = 0, Message = "Reloaded." };
        }
    }
}
