using Domain_layer.Models;
using Service_layer.DTOS.Subscription;

namespace Service_layer.Services_Interfaces
{
    public interface ISubscriptionService
    {
        Task<IEnumerable<Subscription>> GetAllAsync();
        Task<IEnumerable<Subscription>> GetByBusinessIdAsync(string businessId);
        Task<Subscription?> GetByIdAsync(string id);
        Task<Subscription?> GetActiveSubscriptionAsync(string businessId);
        Task<Subscription> CreateAsync(SubscriptionCreateDTO dto);
        Task<Subscription?> RenewAsync(string id, SubscriptionRenewDTO dto);
        Task<bool> DeleteAsync(string id);
    }
}

