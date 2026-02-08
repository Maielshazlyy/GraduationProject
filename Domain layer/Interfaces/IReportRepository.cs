using Domain_layer.Models;

namespace Domain_layer.Interfaces
{
    public interface IReportRepository : IRepository<Report>
    {
        Task<IEnumerable<Report>> GetByBusinessIdAsync(string businessId);
        Task<IEnumerable<Report>> GetByReportTypeAsync(string reportType);
    }
}

