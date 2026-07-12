using Domain_layer.Models;
using Service_layer.DTOS.Integration;

namespace Service_layer.Services_Interfaces
{
    public interface IIntegrationService
    {
        Task<IEnumerable<Integration>> GetAllAsync();
        Task<IEnumerable<Integration>> GetByBusinessIdAsync(string businessId);
        Task<Integration?> GetByIdAsync(string id);
        Task<Integration> ConnectAsync(IntegrationConnectDTO dto);
        Task<Integration?> SyncAsync(string id, IntegrationSyncDTO dto);
        Task<bool> DeleteAsync(string id);
    }
}

