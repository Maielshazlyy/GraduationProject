using Service_layer.DTOS.BusinessReport;
using System.Threading.Tasks;

namespace Service_layer.Services_Interfaces
{
    public interface IBusinessReportGenerationService
    {
        Task<AiReportResponseDTO> GenerateReportAsync(string businessId, ReportGenerateRequestDTO request);
    }
}
