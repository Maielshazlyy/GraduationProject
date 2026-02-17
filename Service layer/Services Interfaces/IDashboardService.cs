using Service_layer.DTOS.Dashboard;

namespace Service_layer.Services_Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardSummaryDTO> GetDashboardSummaryAsync(string businessId);
    }
}

