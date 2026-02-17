using Domain_layer.Models;

namespace Domain_layer.Interfaces
{
    public interface IMenuCategoryRepository : IRepository<MenuCategory>
    {
        Task<IEnumerable<MenuCategory>> GetByBusinessIdAsync(string businessId);
        Task<IEnumerable<MenuCategory>> GetActiveByBusinessIdAsync(string businessId);
    }
}

