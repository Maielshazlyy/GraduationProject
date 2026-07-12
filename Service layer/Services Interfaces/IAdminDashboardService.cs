using Service_layer.DTOS.Dashboard;

namespace Service_layer.Services_Interfaces
{
    public interface IAdminDashboardService
    {
        Task<AdminDashboardSummaryDTO> GetSummaryAsync();
        Task<List<AdminBusinessSnapshotDTO>> GetTopBusinessesByRevenueAsync(int count = 10);
        Task<AdminDashboardFullDTO> GetFullDashboardAsync(int topBusinessesCount = 10);
        Task<List<AdminAlertDTO>> GetAlertsAsync();
        Task<List<AdminRevenueTrendPointDTO>> GetRevenueTrendAsync(int months = 12);
        Task<List<AdminStatusCountDTO>> GetOrdersByStatusAsync();
        Task<List<AdminPriorityCountDTO>> GetTicketsByPriorityAsync();
        Task<List<AdminBusinessHealthDTO>> GetBusinessHealthAsync(int top = 20, bool ascending = false);
        Task<List<AdminSentimentTrendPointDTO>> GetSentimentTrendAsync(int days = 30);
        Task<bool> SetBusinessActiveStateAsync(string businessId, bool isActive, string? adminUserId = null);
        Task<bool> SetBusinessVerificationStateAsync(string businessId, bool isVerified, string? adminUserId = null);
    }
}
