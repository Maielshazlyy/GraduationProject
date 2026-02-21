using Service_layer.DTOS.Dashboard;
using Service_layer.DTOS.AuditLog;

namespace Service_layer.Services_Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardSummaryDTO> GetDashboardSummaryAsync(string businessId);
        Task<List<AuditLogResponseDTO>> GetRecentAuditLogsAsync(string businessId, int count = 20);
        Task<AuditLogStatisticsDTO> GetAuditLogStatisticsAsync(string businessId);
        Task<List<AuditLogResponseDTO>> GetCustomerAuditLogsAsync(string businessId, string customerId);
    }
}

