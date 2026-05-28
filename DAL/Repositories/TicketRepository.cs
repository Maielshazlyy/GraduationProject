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

        private IQueryable<Ticket> WithIncludes()
            => _context.Set<Ticket>()
                .Include(t => t.Customer)
                .Include(t => t.Business)
                .Include(t => t.AssignedToUser)
                .Include(t => t.Feedbacks);

        public override async Task<IEnumerable<Ticket>> GetAllAsync()
            => await WithIncludes().ToListAsync();

        public override async Task<Ticket?> GetByIdAsync(string id)
            => await WithIncludes().FirstOrDefaultAsync(t => t.Id == id);

        public async Task<IEnumerable<Ticket>> GetByBusinessIdAsync(string businessId)
            => await WithIncludes().Where(t => t.BusinessId == businessId).ToListAsync();

        public async Task<IEnumerable<Ticket>> GetByCustomerIdAsync(string customerId)
            => await WithIncludes().Where(t => t.CustomerId == customerId).ToListAsync();

        public async Task<IEnumerable<Ticket>> GetByStatusAsync(string status)
            => await WithIncludes().Where(t => t.Status == status).ToListAsync();

        public async Task<IEnumerable<Ticket>> GetByAssignedUserIdAsync(string userId)
            => await WithIncludes().Where(t => t.AssignedToUserId == userId).ToListAsync();
    }
}

