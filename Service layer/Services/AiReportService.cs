using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Service_layer.DTOS.BusinessReport;
using Service_layer.Services_Interfaces;

namespace Service_layer.Services
{
    public class AiReportService : IAiReportService
    {
        private readonly HttpClient _httpClient;

        public AiReportService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<AiReportResponseDTO> GenerateReportAsync(AiReportRequestDTO request)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/v1/analysis/report/generate", request);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<AiReportResponseDTO>();
            return result ?? new AiReportResponseDTO();
        }
    }
}
