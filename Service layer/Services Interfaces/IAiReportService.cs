using Service_layer.DTOS.BusinessReport;
using System.Threading.Tasks;

namespace Service_layer.Services_Interfaces
{
    public interface IAiReportService
    {
        Task<AiReportResponseDTO> GenerateReportAsync(AiReportRequestDTO request);
    }
}
