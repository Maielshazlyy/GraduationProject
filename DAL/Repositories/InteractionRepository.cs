using DAL.Context;
using Domain_layer.Interfaces;
using Domain_layer.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class InteractionRepository : Repository<Interaction>, IInteractionRepository
    {
        private readonly AppDbContext _context;
        
        public InteractionRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<Interaction>> GetByBusinessIdAsync(string businessId)
        {
            return await FindAsync(i => i.BusinessId == businessId);
        }
        
        public async Task<IEnumerable<Interaction>> GetByCustomerIdAsync(string customerId)
        {
            return await FindAsync(i => i.CustomerId == customerId);
        }
        
        public async Task<IEnumerable<Interaction>> GetByUserIdAsync(string userId)
        {
            return await FindAsync(i => i.HandledByUserId == userId);
        }
    }
}

