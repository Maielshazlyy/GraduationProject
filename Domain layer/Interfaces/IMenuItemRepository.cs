using Domain_layer.Models;

namespace Domain_layer.Interfaces
{
    public interface IMenuItemRepository : IRepository<MenuItem>
    {
        Task<IEnumerable<MenuItem>> GetByBusinessIdAsync(string businessId);
        Task<IEnumerable<MenuItem>> GetByCategoryAsync(string category);
        Task<IEnumerable<MenuItem>> GetAvailableItemsAsync(string businessId);
    }
}

