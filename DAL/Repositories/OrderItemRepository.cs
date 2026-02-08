using DAL.Context;
using Domain_layer.Interfaces;
using Domain_layer.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class OrderItemRepository : Repository<OrderItem>, IOrderItemRepository
    {
        private readonly AppDbContext _context;
        
        public OrderItemRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<OrderItem>> GetByOrderIdAsync(string orderId)
        {
            return await FindAsync(oi => oi.OrderId == orderId);
        }
        
        public async Task<IEnumerable<OrderItem>> GetByMenuItemIdAsync(string menuItemId)
        {
            return await FindAsync(oi => oi.MenuItemId == menuItemId);
        }
    }
}

