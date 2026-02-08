using Domain_layer.Models;

namespace Domain_layer.Interfaces
{
    public interface IInteractionRepository : IRepository<Interaction>
    {
        Task<IEnumerable<Interaction>> GetByBusinessIdAsync(string businessId);
        Task<IEnumerable<Interaction>> GetByCustomerIdAsync(string customerId);
        Task<IEnumerable<Interaction>> GetByUserIdAsync(string userId);
    }
}

