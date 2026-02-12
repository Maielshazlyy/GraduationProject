using Domain_layer.Models;

namespace Domain_layer.Interfaces
{
    public interface IMenuItemRepository : IRepository<MenuItem>
    {
        Task<IEnumerable<MenuItem>> GetByBusinessIdAsync(string businessId);
        Task<IEnumerable<MenuItem>> GetByCategoryAsync(string categoryId);
        Task<IEnumerable<MenuItem>> GetAvailableItemsAsync(string businessId);
    }
}

