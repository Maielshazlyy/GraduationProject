using DAL.Context;
using Domain_layer.Interfaces;
using Domain_layer.Models;
using Domain_layer.enums;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        private readonly AppDbContext _context;
        
        public OrderRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<Order>> GetByBusinessIdAsync(string businessId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems).ThenInclude(oi => oi.MenuItem)
                .Include(o => o.Customer)
                .Include(o => o.Business)
                .Where(o => o.BusinessId == businessId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetByCustomerIdAsync(string customerId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems).ThenInclude(oi => oi.MenuItem)
                .Include(o => o.Customer)
                .Include(o => o.Business)
                .Where(o => o.CustomerId == customerId)
                .ToListAsync();
        }
        
        public async Task<IEnumerable<Order>> GetByStatusAsync(OrderStatus status)
        {
            return await FindAsync(o => o.Status == status);
        }
    }
}

