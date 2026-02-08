using Domain_layer.Models;
using Service_layer.DTOS.Report;

namespace Service_layer.Services_Interfaces
{
    public interface IReportService
    {
        Task<IEnumerable<Report>> GetAllAsync();
        Task<IEnumerable<Report>> GetByBusinessIdAsync(string businessId);
        Task<Report?> GetByIdAsync(string id);
        Task<Report> CreateAsync(ReportCreateDTO dto);
        Task<bool> DeleteAsync(string id);
    }
}

