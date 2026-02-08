using Domain_layer.Models;
using Service_layer.DTOS.menuItem;

namespace Service_layer.Services_Interfaces
{
    public interface IMenuItemService
    {
        Task<IEnumerable<MenuItem>> GetAllAsync();
        Task<IEnumerable<MenuItem>> GetByBusinessIdAsync(string businessId);
        Task<MenuItem?> GetByIdAsync(string id);
        Task<MenuItem> CreateAsync(MenuItemCreateDTO dto);
        Task<MenuItem?> UpdateAsync(string id, MenuItemUpdateDTO dto);
        Task<bool> DeleteAsync(string id);
    }
}

