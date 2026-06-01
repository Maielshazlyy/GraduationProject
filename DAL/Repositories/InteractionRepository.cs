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

        private IQueryable<Interaction> WithIncludes()
            => _context.Set<Interaction>()
                .Include(i => i.Customer)
                .Include(i => i.Business)
                .Include(i => i.HandledByUser);

        public override async Task<IEnumerable<Interaction>> GetAllAsync()
            => await WithIncludes().ToListAsync();

        public override async Task<Interaction?> GetByIdAsync(string id)
            => await WithIncludes().FirstOrDefaultAsync(i => i.InteractionId == id);

        public async Task<IEnumerable<Interaction>> GetByBusinessIdAsync(string businessId)
            => await WithIncludes().Where(i => i.BusinessId == businessId).ToListAsync();

        public async Task<IEnumerable<Interaction>> GetByCustomerIdAsync(string customerId)
            => await WithIncludes().Where(i => i.CustomerId == customerId).ToListAsync();

        public async Task<IEnumerable<Interaction>> GetByUserIdAsync(string userId)
            => await WithIncludes().Where(i => i.HandledByUserId == userId).ToListAsync();
    }
}

