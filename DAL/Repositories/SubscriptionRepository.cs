using DAL.Context;
using Domain_layer.Interfaces;
using Domain_layer.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class SubscriptionRepository : Repository<Subscription>, ISubscriptionRepository
    {
        private readonly AppDbContext _context;
        
        public SubscriptionRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<Subscription>> GetByBusinessIdAsync(string businessId)
        {
            return await FindAsync(s => s.BusinessId == businessId);
        }
        
        public async Task<Subscription?> GetActiveSubscriptionAsync(string businessId)
        {
            return await FirstOrDefaultAsync(s => 
                s.BusinessId == businessId && 
                s.IsActive == true &&
                s.EndDate > DateTime.UtcNow);
        }
    }
}

