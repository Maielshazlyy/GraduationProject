using DAL.Context;
using Domain_layer.Interfaces;
using Domain_layer.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class TicketRepository : Repository<Ticket>, ITicketRepository
    {
        private readonly AppDbContext _context;
        
        public TicketRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<Ticket>> GetByBusinessIdAsync(string businessId)
        {
            return await FindAsync(t => t.BusinessId == businessId);
        }
        
        public async Task<IEnumerable<Ticket>> GetByCustomerIdAsync(string customerId)
        {
            return await FindAsync(t => t.CustomerId == customerId);
        }
        
        public async Task<IEnumerable<Ticket>> GetByStatusAsync(string status)
        {
            return await FindAsync(t => t.Status == status);
        }
        
        public async Task<IEnumerable<Ticket>> GetByAssignedUserIdAsync(string userId)
        {
            return await FindAsync(t => t.AssignedToUserId == userId);
        }
    }
}

