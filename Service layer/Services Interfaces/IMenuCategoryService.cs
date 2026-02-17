using Domain_layer.Models;
using Service_layer.DTOS.MenuCategory;

namespace Service_layer.Services_Interfaces
{
    public interface IMenuCategoryService
    {
        Task<IEnumerable<MenuCategory>> GetAllAsync();
        Task<IEnumerable<MenuCategory>> GetByBusinessIdAsync(string businessId);
        Task<IEnumerable<MenuCategory>> GetActiveByBusinessIdAsync(string businessId);
        Task<MenuCategory?> GetByIdAsync(string id);
        Task<MenuCategory> CreateAsync(MenuCategoryCreateDTO dto);
        Task<MenuCategory?> UpdateAsync(string id, MenuCategoryUpdateDTO dto);
        Task<bool> DeleteAsync(string id);
        Task<bool> ReorderCategoriesAsync(string businessId, ReorderCategoriesDTO dto);
    }
}

