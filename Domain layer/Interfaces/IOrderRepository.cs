using Domain_layer.Models;
using Domain_layer.enums;

namespace Domain_layer.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<IEnumerable<Order>> GetByBusinessIdAsync(string businessId);
        Task<IEnumerable<Order>> GetByCustomerIdAsync(string customerId);
        Task<IEnumerable<Order>> GetByStatusAsync(OrderStatus status);
    }
}

