using Domain_layer.enums;
using Domain_layer.Interfaces;
using Domain_layer.Models;
using Moq;
using Service_layer.DTOS.Order;
using Service_layer.Services;
using Xunit;

namespace Tests
{
    public class OrderServiceTests
    {
        private readonly Mock<IOrderRepository> _orderRepoMock = new();
        private readonly Mock<IOrderItemRepository> _orderItemRepoMock = new();
        private readonly Mock<IBusinessRepository> _businessRepoMock = new();
        private readonly Mock<ICustomerRepository> _customerRepoMock = new();
        private readonly Mock<IMenuItemRepository> _menuItemRepoMock = new();
        private readonly Mock<IUnitOfWork> _uowMock = new();

        private OrderService CreateService() =>
            new(_orderRepoMock.Object, _orderItemRepoMock.Object,
                _businessRepoMock.Object, _customerRepoMock.Object,
                _menuItemRepoMock.Object, _uowMock.Object);

        private static Business FakeBusiness(string id) => new() { BusinessId = id };
        private static Customer FakeCustomer(string id) => new() { CustomerId = id };
        private static MenuItem FakeMenuItem(string id, decimal price, bool available = true) =>
            new() { MenuItemId = id, Name = "Item", Price = price, IsAvailable = available };

        // ── CreateAsync ──────────────────────────────────────────────────────

        [Fact]
        public async Task CreateAsync_ValidOrder_ReturnsPendingOrderWithCorrectTotal()
        {
            _businessRepoMock.Setup(r => r.GetByIdAsync("biz-1")).ReturnsAsync(FakeBusiness("biz-1"));
            _customerRepoMock.Setup(r => r.GetByIdAsync("cust-1")).ReturnsAsync(FakeCustomer("cust-1"));
            _menuItemRepoMock.Setup(r => r.GetByIdAsync("item-1")).ReturnsAsync(FakeMenuItem("item-1", 50m));
            _menuItemRepoMock.Setup(r => r.GetByIdAsync("item-2")).ReturnsAsync(FakeMenuItem("item-2", 30m));
            _uowMock.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            var dto = new OrderCreateDTO
            {
                BusinessId = "biz-1",
                CustomerId = "cust-1",
                Items      =
                [
                    new OrderItemDTO { MenuItemId = "item-1", Quantity = 2 },
                    new OrderItemDTO { MenuItemId = "item-2", Quantity = 1 }
                ]
            };

            var service = CreateService();
            var result  = await service.CreateAsync(dto);

            Assert.NotNull(result);
            Assert.Equal(OrderStatus.Pending, result.Status);
            Assert.Equal(130m, result.TotalPrice); // 50*2 + 30*1
            _orderRepoMock.Verify(r => r.AddAsync(It.IsAny<Order>()), Times.Once);
            _orderItemRepoMock.Verify(r => r.AddAsync(It.IsAny<OrderItem>()), Times.Exactly(2));
        }

        [Fact]
        public async Task CreateAsync_BusinessNotFound_ThrowsArgumentException()
        {
            _businessRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<string>())).ReturnsAsync((Business?)null);

            var service = CreateService();
            await Assert.ThrowsAsync<ArgumentException>(() =>
                service.CreateAsync(new OrderCreateDTO
                {
                    BusinessId = "bad",
                    CustomerId = "cust-1",
                    Items      = [new OrderItemDTO { MenuItemId = "x", Quantity = 1 }]
                }));
        }

        [Fact]
        public async Task CreateAsync_CustomerNotFound_ThrowsArgumentException()
        {
            _businessRepoMock.Setup(r => r.GetByIdAsync("biz-1")).ReturnsAsync(FakeBusiness("biz-1"));
            _customerRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<string>())).ReturnsAsync((Customer?)null);

            var service = CreateService();
            await Assert.ThrowsAsync<ArgumentException>(() =>
                service.CreateAsync(new OrderCreateDTO
                {
                    BusinessId = "biz-1",
                    CustomerId = "bad",
                    Items      = [new OrderItemDTO { MenuItemId = "x", Quantity = 1 }]
                }));
        }

        [Fact]
        public async Task CreateAsync_EmptyItems_ThrowsArgumentException()
        {
            _businessRepoMock.Setup(r => r.GetByIdAsync("biz-1")).ReturnsAsync(FakeBusiness("biz-1"));
            _customerRepoMock.Setup(r => r.GetByIdAsync("cust-1")).ReturnsAsync(FakeCustomer("cust-1"));

            var service = CreateService();
            await Assert.ThrowsAsync<ArgumentException>(() =>
                service.CreateAsync(new OrderCreateDTO
                {
                    BusinessId = "biz-1",
                    CustomerId = "cust-1",
                    Items      = []
                }));
        }

        [Fact]
        public async Task CreateAsync_UnavailableMenuItem_ThrowsArgumentException()
        {
            _businessRepoMock.Setup(r => r.GetByIdAsync("biz-1")).ReturnsAsync(FakeBusiness("biz-1"));
            _customerRepoMock.Setup(r => r.GetByIdAsync("cust-1")).ReturnsAsync(FakeCustomer("cust-1"));
            _menuItemRepoMock.Setup(r => r.GetByIdAsync("item-1")).ReturnsAsync(FakeMenuItem("item-1", 50m, available: false));

            var service = CreateService();
            await Assert.ThrowsAsync<ArgumentException>(() =>
                service.CreateAsync(new OrderCreateDTO
                {
                    BusinessId = "biz-1",
                    CustomerId = "cust-1",
                    Items      = [new OrderItemDTO { MenuItemId = "item-1", Quantity = 1 }]
                }));
        }

        [Fact]
        public async Task CreateAsync_ZeroQuantity_ThrowsArgumentException()
        {
            _businessRepoMock.Setup(r => r.GetByIdAsync("biz-1")).ReturnsAsync(FakeBusiness("biz-1"));
            _customerRepoMock.Setup(r => r.GetByIdAsync("cust-1")).ReturnsAsync(FakeCustomer("cust-1"));
            _menuItemRepoMock.Setup(r => r.GetByIdAsync("item-1")).ReturnsAsync(FakeMenuItem("item-1", 50m));

            var service = CreateService();
            await Assert.ThrowsAsync<ArgumentException>(() =>
                service.CreateAsync(new OrderCreateDTO
                {
                    BusinessId = "biz-1",
                    CustomerId = "cust-1",
                    Items      = [new OrderItemDTO { MenuItemId = "item-1", Quantity = 0 }]
                }));
        }

        // ── UpdateStatusAsync ────────────────────────────────────────────────

        [Fact]
        public async Task UpdateStatusAsync_ValidStatus_UpdatesOrder()
        {
            var order = new Order { OrderId = "ord-1", Status = OrderStatus.Pending };
            _orderRepoMock.Setup(r => r.GetByIdAsync("ord-1")).ReturnsAsync(order);
            _uowMock.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            var service = CreateService();
            var result  = await service.UpdateStatusAsync("ord-1", new UpdateOrderStatusDTO { Status = "Delivered" });

            Assert.Equal(OrderStatus.Delivered, result!.Status);
        }

        [Fact]
        public async Task UpdateStatusAsync_InvalidStatus_ThrowsArgumentException()
        {
            var order = new Order { OrderId = "ord-2", Status = OrderStatus.Pending };
            _orderRepoMock.Setup(r => r.GetByIdAsync("ord-2")).ReturnsAsync(order);

            var service = CreateService();
            await Assert.ThrowsAsync<ArgumentException>(() =>
                service.UpdateStatusAsync("ord-2", new UpdateOrderStatusDTO { Status = "Flying" }));
        }

        [Fact]
        public async Task UpdateStatusAsync_OrderNotFound_ReturnsNull()
        {
            _orderRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<string>())).ReturnsAsync((Order?)null);

            var service = CreateService();
            var result  = await service.UpdateStatusAsync("ghost", new UpdateOrderStatusDTO { Status = "Delivered" });

            Assert.Null(result);
        }

        // ── DeleteAsync ──────────────────────────────────────────────────────

        [Fact]
        public async Task DeleteAsync_ExistingOrder_ReturnsTrueAndDeletes()
        {
            var order = new Order { OrderId = "ord-3" };
            _orderRepoMock.Setup(r => r.GetByIdAsync("ord-3")).ReturnsAsync(order);
            _uowMock.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            var service = CreateService();
            var result  = await service.DeleteAsync("ord-3");

            Assert.True(result);
            _orderRepoMock.Verify(r => r.Delete(order), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_NotFound_ReturnsFalse()
        {
            _orderRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<string>())).ReturnsAsync((Order?)null);

            var service = CreateService();
            var result  = await service.DeleteAsync("ghost");

            Assert.False(result);
        }
    }
}
