using Domain_layer.Interfaces;
using Domain_layer.Models;
using Moq;
using Service_layer.DTOS.Business;
using Service_layer.Services;
using Xunit;

namespace Tests
{
    public class BusinessServiceTests
    {
        private readonly Mock<IUnitOfWork> _uowMock = new();
        private readonly Mock<ISettingRepository> _settingRepoMock = new();
        private readonly Mock<IKnowledgeBaseRepository> _kbRepoMock = new();
        private readonly Mock<ISubscriptionRepository> _subscriptionRepoMock = new();
        private readonly Mock<IPaymentTransactionRepository> _paymentRepoMock = new();
        private readonly Mock<IBusinessRepository> _businessRepoMock = new();

        private BusinessService CreateService()
        {
            _uowMock.Setup(u => u.Businesses).Returns(_businessRepoMock.Object);
            return new BusinessService(
                _uowMock.Object,
                _settingRepoMock.Object,
                _kbRepoMock.Object,
                _subscriptionRepoMock.Object,
                _paymentRepoMock.Object);
        }

        private static Business FakeBusiness(string id = "biz-1") =>
            new() { Id = id, BusinessId = id, Name = "Test Restaurant" };

        // ── CreateAsync ──────────────────────────────────────────────────────

        [Fact]
        public async Task CreateAsync_ValidBusiness_ReturnsBusiness()
        {
            _uowMock.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            var service  = CreateService();
            var business = FakeBusiness();
            var result   = await service.CreateAsync(business);

            Assert.NotNull(result);
            Assert.Equal("Test Restaurant", result.Name);
            _businessRepoMock.Verify(r => r.AddAsync(business), Times.Once);
            _uowMock.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_AutoCreatesDefaultSettings()
        {
            _uowMock.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            var service = CreateService();
            await service.CreateAsync(FakeBusiness());

            // Default settings must be created for every new business
            _settingRepoMock.Verify(r => r.AddAsync(It.Is<Setting>(s =>
                s.ChatbotEnabled == true &&
                s.Language == "en" &&
                s.TimeZone == "UTC"
            )), Times.Once);
        }

        // ── UpdateAsync ──────────────────────────────────────────────────────

        [Fact]
        public async Task UpdateAsync_BusinessNotFound_ReturnsNull()
        {
            _businessRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<string>())).ReturnsAsync((Business?)null);

            var service = CreateService();
            var result  = await service.UpdateAsync("ghost", new BusinessUpdateDTO { Name = "New Name" });

            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateAsync_PartialUpdate_OnlyOverwritesProvidedFields()
        {
            var existing = new Business
            {
                Id   = "biz-1",
                Name = "Old Name",
                Type = "Restaurant",
                City = "Cairo"
            };
            _businessRepoMock.Setup(r => r.GetByIdAsync("biz-1")).ReturnsAsync(existing);
            _uowMock.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            var service = CreateService();
            var result  = await service.UpdateAsync("biz-1", new BusinessUpdateDTO
            {
                Name = "New Name"
                // Type and City are null — should NOT be overwritten
            });

            Assert.Equal("New Name", result!.Name);
            Assert.Equal("Restaurant", result.Type);  // unchanged
            Assert.Equal("Cairo", result.City);        // unchanged
        }

        [Fact]
        public async Task UpdateAsync_BooleanFeatures_OnlyOverwritesWhenProvided()
        {
            var existing = new Business { Id = "biz-2", HasDelivery = false, HasWiFi = true };
            _businessRepoMock.Setup(r => r.GetByIdAsync("biz-2")).ReturnsAsync(existing);
            _uowMock.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            var service = CreateService();
            var result  = await service.UpdateAsync("biz-2", new BusinessUpdateDTO
            {
                HasDelivery = true
                // HasWiFi not provided — must stay true
            });

            Assert.True(result!.HasDelivery);
            Assert.True(result.HasWiFi); // unchanged
        }

        // ── DeleteAsync ──────────────────────────────────────────────────────

        [Fact]
        public async Task DeleteAsync_ExistingBusiness_ReturnsTrueAndDeletes()
        {
            var business = FakeBusiness("biz-3");
            _businessRepoMock.Setup(r => r.GetByIdAsync("biz-3")).ReturnsAsync(business);
            _uowMock.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            var service = CreateService();
            var result  = await service.DeleteAsync("biz-3");

            Assert.True(result);
            _businessRepoMock.Verify(r => r.Delete(business), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_NotFound_ReturnsFalse()
        {
            _businessRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<string>())).ReturnsAsync((Business?)null);

            var service = CreateService();
            var result  = await service.DeleteAsync("ghost");

            Assert.False(result);
        }
    }
}
