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
        /// <summary>
        /// Returns the full dashboard in one call. Each section can be filtered independently
        /// via <paramref name="filter"/> ("today" | "7d" | "30d" | "all"); null sections fall back to "30d".
        /// </summary>
        Task<FullDashboardDTO> GetFullDashboardAsync(string businessId, DashboardFilterDTO filter);
    }
}

