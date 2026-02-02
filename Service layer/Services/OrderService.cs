using System;
using Domain_layer.Interfaces;
using Domain_layer.Models;
using Domain_layer.enums;
using Service_layer.DTOS.Order;
using Service_layer.Services_Interfaces;

namespace Service_layer.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IBusinessRepository _businessRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IMenuItemRepository _menuItemRepository;
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(
            IOrderRepository orderRepository,
            IOrderItemRepository orderItemRepository,
            IBusinessRepository businessRepository,
            ICustomerRepository customerRepository,
            IMenuItemRepository menuItemRepository,
            IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
            _businessRepository = businessRepository;
            _customerRepository = customerRepository;
            _menuItemRepository = menuItemRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _orderRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Order>> GetByBusinessIdAsync(string businessId)
        {
            return await _orderRepository.GetByBusinessIdAsync(businessId);
        }

        public async Task<IEnumerable<Order>> GetByCustomerIdAsync(string customerId)
        {
            return await _orderRepository.GetByCustomerIdAsync(customerId);
        }

        public async Task<Order?> GetByIdAsync(string id)
        {
            return await _orderRepository.GetByIdAsync(id);
        }

        public async Task<Order> CreateAsync(OrderCreateDTO dto)
        {
            var business = await _businessRepository.GetByIdAsync(dto.BusinessId);
            if (business == null)
                throw new ArgumentException($"Business with id '{dto.BusinessId}' not found.");

            var customer = await _customerRepository.GetByIdAsync(dto.CustomerId);
            if (customer == null)
                throw new ArgumentException($"Customer with id '{dto.CustomerId}' not found.");

            if (dto.Items == null || dto.Items.Count == 0)
                throw new ArgumentException("Order must have at least one item.");

            decimal totalPrice = 0;
            var orderItems = new List<OrderItem>();

            foreach (var itemDto in dto.Items)
            {
                var menuItem = await _menuItemRepository.GetByIdAsync(itemDto.MenuItemId);
                if (menuItem == null)
                    throw new ArgumentException($"MenuItem with id '{itemDto.MenuItemId}' not found.");

                if (!menuItem.IsAvailable)
                    throw new ArgumentException($"MenuItem '{menuItem.Name}' is not available.");

                if (itemDto.Quantity <= 0)
                    throw new ArgumentException("Quantity must be greater than zero.");

                var itemPrice = menuItem.Price * itemDto.Quantity;
                totalPrice += itemPrice;

                var orderItem = new OrderItem
                {
                    OrderItemId = Guid.NewGuid().ToString(),
                    MenuItemId = itemDto.MenuItemId,
                    Quantity = itemDto.Quantity,
                    UnitPrice = menuItem.Price
                };

                orderItems.Add(orderItem);
            }

            var order = new Order
            {
                OrderId = Guid.NewGuid().ToString(),
                CustomerId = dto.CustomerId,
                BusinessId = dto.BusinessId,
                TotalPrice = totalPrice,
                Status = OrderStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            await _orderRepository.AddAsync(order);

            // Set OrderId for each OrderItem and add them
            foreach (var orderItem in orderItems)
            {
                orderItem.OrderId = order.OrderId;
                await _orderItemRepository.AddAsync(orderItem);
            }

            await _unitOfWork.CompleteAsync();
            return order;
        }

        public async Task<Order?> UpdateStatusAsync(string id, UpdateOrderStatusDTO dto)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null) return null;

            if (Enum.TryParse<OrderStatus>(dto.Status, out var status))
            {
                order.Status = status;
            }
            else
            {
                throw new ArgumentException($"Invalid order status: {dto.Status}");
            }

            _orderRepository.Update(order);
            await _unitOfWork.CompleteAsync();
            return order;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null) return false;

            _orderRepository.Delete(order);
            await _unitOfWork.CompleteAsync();
            return true;
        }
    }
}

