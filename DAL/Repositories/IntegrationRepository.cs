using DAL.Context;
using Domain_layer.Interfaces;
using Domain_layer.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class IntegrationRepository : Repository<Integration>, IIntegrationRepository
    {
        private readonly AppDbContext _context;
        
        public IntegrationRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<Integration>> GetByBusinessIdAsync(string businessId)
        {
            return await FindAsync(i => i.BusinessId == businessId);
        }
        
        public async Task<Integration?> GetByPlatformNameAsync(string businessId, string platformName)
        {
            return await FirstOrDefaultAsync(i => 
                i.BusinessId == businessId && 
                i.PlatformName == platformName);
        }
    }
}

