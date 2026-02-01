using Domain_layer.Models;
using Domain_layer.enums;
using Service_layer.DTOS.Order;

namespace Service_layer.Services_Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetAllAsync();
        Task<IEnumerable<Order>> GetByBusinessIdAsync(string businessId);
        Task<IEnumerable<Order>> GetByCustomerIdAsync(string customerId);
        Task<Order?> GetByIdAsync(string id);
        Task<Order> CreateAsync(OrderCreateDTO dto);
        Task<Order?> UpdateStatusAsync(string id, UpdateOrderStatusDTO dto);
        Task<bool> DeleteAsync(string id);
    }
}

