using Domain_layer.Interfaces;
using Domain_layer.Models;
using Moq;
using Service_layer.DTOS.Customer;
using Service_layer.Services;
using Xunit;

namespace Tests
{
    public class CustomerServiceTests
    {
        private readonly Mock<ICustomerRepository> _customerRepoMock = new();
        private readonly Mock<IBusinessRepository> _businessRepoMock = new();
        private readonly Mock<IUnitOfWork> _uowMock = new();

        private CustomerService CreateService() =>
            new(_customerRepoMock.Object, _businessRepoMock.Object, _uowMock.Object);

        private static Business FakeBusiness(string id) => new() { BusinessId = id, Name = "Biz" };

        // ── CreateAsync ──────────────────────────────────────────────────────

        [Fact]
        public async Task CreateAsync_ValidData_ReturnsCustomer()
        {
            _businessRepoMock.Setup(r => r.GetByIdAsync("biz-1")).ReturnsAsync(FakeBusiness("biz-1"));
            _customerRepoMock.Setup(r => r.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync((Customer?)null);
            _uowMock.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            var service = CreateService();
            var result  = await service.CreateAsync(new CustomerCreateDTO
            {
                BusinessId = "biz-1",
                FullName   = "Ahmed Ali",
                Email      = "ahmed@test.com",
                Phone      = "01012345678"
            });

            Assert.NotNull(result);
            Assert.Equal("Ahmed Ali", result.FullName);
            Assert.Equal("biz-1", result.BusinessId);
            _customerRepoMock.Verify(r => r.AddAsync(It.IsAny<Customer>()), Times.Once);
            _uowMock.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_BusinessNotFound_ThrowsArgumentException()
        {
            _businessRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<string>())).ReturnsAsync((Business?)null);

            var service = CreateService();
            await Assert.ThrowsAsync<ArgumentException>(() =>
                service.CreateAsync(new CustomerCreateDTO { BusinessId = "bad", FullName = "X" }));
        }

        [Fact]
        public async Task CreateAsync_DuplicateEmailSameBusiness_ThrowsArgumentException()
        {
            var existing = new Customer { CustomerId = "c-1", Email = "taken@test.com", BusinessId = "biz-1" };

            _businessRepoMock.Setup(r => r.GetByIdAsync("biz-1")).ReturnsAsync(FakeBusiness("biz-1"));
            _customerRepoMock.Setup(r => r.GetByEmailAsync("taken@test.com")).ReturnsAsync(existing);

            var service = CreateService();
            await Assert.ThrowsAsync<ArgumentException>(() =>
                service.CreateAsync(new CustomerCreateDTO
                {
                    BusinessId = "biz-1",
                    FullName   = "New Person",
                    Email      = "taken@test.com"
                }));
        }

        [Fact]
        public async Task CreateAsync_SameEmailDifferentBusiness_Succeeds()
        {
            // Email exists but in a different business — should be allowed
            var existing = new Customer { CustomerId = "c-1", Email = "shared@test.com", BusinessId = "biz-other" };

            _businessRepoMock.Setup(r => r.GetByIdAsync("biz-1")).ReturnsAsync(FakeBusiness("biz-1"));
            _customerRepoMock.Setup(r => r.GetByEmailAsync("shared@test.com")).ReturnsAsync(existing);
            _uowMock.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            var service = CreateService();
            var result  = await service.CreateAsync(new CustomerCreateDTO
            {
                BusinessId = "biz-1",
                FullName   = "Sara",
                Email      = "shared@test.com"
            });

            Assert.NotNull(result);
        }

        [Fact]
        public async Task CreateAsync_NoEmail_SkipsDuplicateCheck()
        {
            _businessRepoMock.Setup(r => r.GetByIdAsync("biz-1")).ReturnsAsync(FakeBusiness("biz-1"));
            _uowMock.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            var service = CreateService();
            var result  = await service.CreateAsync(new CustomerCreateDTO
            {
                BusinessId = "biz-1",
                FullName   = "Walk-In Customer"
            });

            Assert.NotNull(result);
            _customerRepoMock.Verify(r => r.GetByEmailAsync(It.IsAny<string>()), Times.Never);
        }

        // ── UpdateAsync ──────────────────────────────────────────────────────

        [Fact]
        public async Task UpdateAsync_ExistingCustomer_UpdatesFields()
        {
            var customer = new Customer { CustomerId = "c-2", FullName = "Old Name", Email = "old@test.com" };
            _customerRepoMock.Setup(r => r.GetByIdAsync("c-2")).ReturnsAsync(customer);
            _uowMock.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            var service = CreateService();
            var result  = await service.UpdateAsync("c-2", new CustomerUpdateDTO
            {
                FullName = "New Name",
                Email    = "new@test.com",
                Phone    = "01099999999"
            });

            Assert.Equal("New Name", result!.FullName);
            Assert.Equal("new@test.com", result.Email);
        }

        [Fact]
        public async Task UpdateAsync_CustomerNotFound_ReturnsNull()
        {
            _customerRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<string>())).ReturnsAsync((Customer?)null);

            var service = CreateService();
            var result  = await service.UpdateAsync("ghost", new CustomerUpdateDTO { FullName = "X" });

            Assert.Null(result);
        }

        // ── DeleteAsync ──────────────────────────────────────────────────────

        [Fact]
        public async Task DeleteAsync_ExistingCustomer_ReturnsTrueAndDeletes()
        {
            var customer = new Customer { CustomerId = "c-3" };
            _customerRepoMock.Setup(r => r.GetByIdAsync("c-3")).ReturnsAsync(customer);
            _uowMock.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            var service = CreateService();
            var result  = await service.DeleteAsync("c-3");

            Assert.True(result);
            _customerRepoMock.Verify(r => r.Delete(customer), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_NotFound_ReturnsFalse()
        {
            _customerRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<string>())).ReturnsAsync((Customer?)null);

            var service = CreateService();
            var result  = await service.DeleteAsync("ghost");

            Assert.False(result);
        }
    }
}
