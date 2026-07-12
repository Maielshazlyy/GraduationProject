using DAL.Context;
using Domain_layer.Interfaces;
using Domain_layer.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class MenuCategoryRepository : Repository<MenuCategory>, IMenuCategoryRepository
    {
        private readonly AppDbContext _context;

        public MenuCategoryRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MenuCategory>> GetByBusinessIdAsync(string businessId)
        {
            return await FindAsync(mc => mc.BusinessId == businessId);
        }

        public async Task<IEnumerable<MenuCategory>> GetActiveByBusinessIdAsync(string businessId)
        {
            return await FindAsync(mc => mc.BusinessId == businessId && mc.IsActive == true);
        }
    }
}

