using Domain_layer.Models;

namespace Domain_layer.Interfaces
{
    public interface ISubscriptionRepository : IRepository<Subscription>
    {
        Task<IEnumerable<Subscription>> GetByBusinessIdAsync(string businessId);
        Task<Subscription?> GetActiveSubscriptionAsync(string businessId);
    }
}

