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

        public async Task<AiOwnerReportSyncResponseDTO> SyncReportAsync(AiOwnerReportSyncRequestDTO request)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/v1/owner/reports/sync", request);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<AiOwnerReportSyncResponseDTO>();
            return result ?? new AiOwnerReportSyncResponseDTO { Status = "ok" };
        }
    }
}
