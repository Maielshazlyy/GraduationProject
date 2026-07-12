using Domain_layer.Models;

namespace Domain_layer.Interfaces
{
    public interface IOrderItemRepository : IRepository<OrderItem>
    {
        Task<IEnumerable<OrderItem>> GetByOrderIdAsync(string orderId);
        Task<IEnumerable<OrderItem>> GetByMenuItemIdAsync(string menuItemId);
    }
}

