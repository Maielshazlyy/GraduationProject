using DAL.Context;
using Domain_layer.Interfaces;
using Domain_layer.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        private readonly AppDbContext _context;
        
        public CustomerRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<Customer>> GetByBusinessIdAsync(string businessId)
        {
            return await _context.Customers
                .Include(c => c.Orders)
                .Include(c => c.Tickets)
                .Where(c => c.BusinessId == businessId)
                .ToListAsync();
        }
        
        public async Task<Customer?> GetByEmailAsync(string email)
        {
            return await FirstOrDefaultAsync(c => c.Email == email);
        }
    }
}

