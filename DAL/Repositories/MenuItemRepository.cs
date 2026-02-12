using DAL.Context;
using Domain_layer.Interfaces;
using Domain_layer.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class MenuItemRepository : Repository<MenuItem>, IMenuItemRepository
    {
        private readonly AppDbContext _context;
        
        public MenuItemRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<MenuItem>> GetByBusinessIdAsync(string businessId)
        {
            return await FindAsync(m => m.BusinessId == businessId);
        }
        
        public async Task<IEnumerable<MenuItem>> GetByCategoryAsync(string categoryId)
        {
            return await FindAsync(m => m.MenuCategoryId == categoryId);
        }
        
        public async Task<IEnumerable<MenuItem>> GetAvailableItemsAsync(string businessId)
        {
            return await FindAsync(m => m.BusinessId == businessId && m.IsAvailable == true);
        }
    }
}

