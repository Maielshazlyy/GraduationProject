using DAL.Context;
using Domain_layer.Interfaces;
using Domain_layer.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class SettingRepository : Repository<Setting>, ISettingRepository
    {
        private readonly AppDbContext _context;
        
        public SettingRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        
        public async Task<Setting?> GetByBusinessIdAsync(string businessId)
        {
            return await FirstOrDefaultAsync(s => s.BusinessId == businessId);
        }
    }
}

