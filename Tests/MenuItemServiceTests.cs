using Domain_layer.Interfaces;
using Domain_layer.Models;
using Moq;
using Service_layer.DTOS.menuItem;
using Service_layer.Services;
using Service_layer.Services_Interfaces;
using Xunit;

namespace Tests
{
    public class MenuItemServiceTests
    {
        private readonly Mock<IMenuItemRepository> _itemRepoMock = new();
        private readonly Mock<IBusinessRepository> _businessRepoMock = new();
        private readonly Mock<IUnitOfWork> _uowMock = new();
        private readonly Mock<IMenuCategoryRepository> _categoryRepoMock = new();
        private readonly Mock<IAiKnowledgeSyncService> _aiSyncMock = new();

        private MenuItemService CreateService()
        {
            _uowMock.Setup(u => u.MenuCategories).Returns(_categoryRepoMock.Object);
            return new MenuItemService(
                _itemRepoMock.Object,
                _businessRepoMock.Object,
                _uowMock.Object,
                _aiSyncMock.Object);
        }

        private static Business FakeBusiness(string id) =>
            new() { BusinessId = id, Name = "Test Business" };

        // ── CreateAsync ──────────────────────────────────────────────────────

        [Fact]
        public async Task CreateAsync_ValidBusiness_ReturnsMenuItem()
        {
            var businessId = "biz-1";
            _businessRepoMock
                .Setup(r => r.GetByIdAsync(businessId))
                .ReturnsAsync(FakeBusiness(businessId));

            _uowMock.Setup(u => u.CompleteAsync()).ReturnsAsync(1);
            _aiSyncMock.Setup(a => a.SyncBusinessAsync(businessId)).Returns(Task.CompletedTask);

            var dto = new MenuItemCreateDTO
            {
                BusinessId  = businessId,
                Name        = "Pizza",
                Description = "Cheesy pizza",
                Price       = 49.99m
            };

            var service = CreateService();
            var result  = await service.CreateAsync(dto);

            Assert.NotNull(result);
            Assert.Equal("Pizza", result.Name);
            Assert.Equal(businessId, result.BusinessId);
            _itemRepoMock.Verify(r => r.AddAsync(It.IsAny<MenuItem>()), Times.Once);
            _uowMock.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_BusinessNotFound_ThrowsArgumentException()
        {
            _businessRepoMock
                .Setup(r => r.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((Business?)null);

            var service = CreateService();

            await Assert.ThrowsAsync<ArgumentException>(() =>
                service.CreateAsync(new MenuItemCreateDTO { BusinessId = "bad-id", Name = "X", Price = 1 }));
        }

        // ── BulkCreateAsync ──────────────────────────────────────────────────

        [Fact]
        public async Task BulkCreateAsync_WithExistingCategory_UsesExistingCategoryId()
        {
            var businessId = "biz-2";
            var category   = new MenuCategory
            {
                MenuCategoryId = "cat-1",
                Name           = "Burgers",
                BusinessId     = businessId
            };

            _businessRepoMock
                .Setup(r => r.GetByIdAsync(businessId))
                .ReturnsAsync(FakeBusiness(businessId));

            _categoryRepoMock
                .Setup(r => r.GetByBusinessIdAsync(businessId))
                .ReturnsAsync(new List<MenuCategory> { category });

            _uowMock.Setup(u => u.CompleteAsync()).ReturnsAsync(1);
            _aiSyncMock.Setup(a => a.SyncBusinessAsync(businessId)).Returns(Task.CompletedTask);

            var dto = new MenuItemBulkCreateDTO
            {
                BusinessId = businessId,
                Items      =
                [
                    new MenuItemBulkEntryDTO { Name = "Big Burger", Price = 35m, CategoryName = "Burgers" }
                ]
            };

            var service = CreateService();
            var results = (await service.BulkCreateAsync(dto)).ToList();

            Assert.Single(results);
            Assert.Equal("cat-1", results[0].MenuCategoryId);
            _categoryRepoMock.Verify(r => r.AddAsync(It.IsAny<MenuCategory>()), Times.Never);
        }

        [Fact]
        public async Task BulkCreateAsync_WithNewCategoryName_CreatesNewCategory()
        {
            var businessId = "biz-3";

            _businessRepoMock
                .Setup(r => r.GetByIdAsync(businessId))
                .ReturnsAsync(FakeBusiness(businessId));

            _categoryRepoMock
                .Setup(r => r.GetByBusinessIdAsync(businessId))
                .ReturnsAsync(new List<MenuCategory>());

            _uowMock.Setup(u => u.CompleteAsync()).ReturnsAsync(1);
            _aiSyncMock.Setup(a => a.SyncBusinessAsync(businessId)).Returns(Task.CompletedTask);

            var dto = new MenuItemBulkCreateDTO
            {
                BusinessId = businessId,
                Items      =
                [
                    new MenuItemBulkEntryDTO { Name = "Pasta", Price = 25m, CategoryName = "Italian" }
                ]
            };

            var service = CreateService();
            var results = (await service.BulkCreateAsync(dto)).ToList();

            Assert.Single(results);
            Assert.NotNull(results[0].MenuCategoryId);
            _categoryRepoMock.Verify(r => r.AddAsync(It.IsAny<MenuCategory>()), Times.Once);
        }

        [Fact]
        public async Task BulkCreateAsync_SameCategoryNameForMultipleItems_CreatesOnlyOneCategory()
        {
            var businessId = "biz-4";

            _businessRepoMock
                .Setup(r => r.GetByIdAsync(businessId))
                .ReturnsAsync(FakeBusiness(businessId));

            _categoryRepoMock
                .Setup(r => r.GetByBusinessIdAsync(businessId))
                .ReturnsAsync(new List<MenuCategory>());

            _uowMock.Setup(u => u.CompleteAsync()).ReturnsAsync(1);
            _aiSyncMock.Setup(a => a.SyncBusinessAsync(businessId)).Returns(Task.CompletedTask);

            var dto = new MenuItemBulkCreateDTO
            {
                BusinessId = businessId,
                Items      =
                [
                    new MenuItemBulkEntryDTO { Name = "Latte",     Price = 20m, CategoryName = "Drinks" },
                    new MenuItemBulkEntryDTO { Name = "Cappuccino", Price = 22m, CategoryName = "Drinks" },
                    new MenuItemBulkEntryDTO { Name = "Espresso",  Price = 18m, CategoryName = "drinks" }
                ]
            };

            var service = CreateService();
            var results = (await service.BulkCreateAsync(dto)).ToList();

            Assert.Equal(3, results.Count);
            // All 3 items should share the same category ID
            Assert.Single(results.Select(r => r.MenuCategoryId).Distinct());
            // Category created only once despite 3 items with same name (case-insensitive)
            _categoryRepoMock.Verify(r => r.AddAsync(It.IsAny<MenuCategory>()), Times.Once);
        }


        [Fact]
        public async Task BulkCreateAsync_NoCategoryName_SetsNullCategoryId()
        {
            var businessId = "biz-5";

            _businessRepoMock
                .Setup(r => r.GetByIdAsync(businessId))
                .ReturnsAsync(FakeBusiness(businessId));

            _categoryRepoMock
                .Setup(r => r.GetByBusinessIdAsync(businessId))
                .ReturnsAsync(new List<MenuCategory>());

            _uowMock.Setup(u => u.CompleteAsync()).ReturnsAsync(1);
            _aiSyncMock.Setup(a => a.SyncBusinessAsync(businessId)).Returns(Task.CompletedTask);

            var dto = new MenuItemBulkCreateDTO
            {
                BusinessId = businessId,
                Items      = [new MenuItemBulkEntryDTO { Name = "Water", Price = 5m }]
            };

            var service = CreateService();
            var results = (await service.BulkCreateAsync(dto)).ToList();

            Assert.Single(results);
            Assert.Null(results[0].MenuCategoryId);
        }

        [Fact]
        public async Task BulkCreateAsync_EmptyItems_ThrowsArgumentException()
        {
            var businessId = "biz-6";
            _businessRepoMock
                .Setup(r => r.GetByIdAsync(businessId))
                .ReturnsAsync(FakeBusiness(businessId));

            var service = CreateService();

            await Assert.ThrowsAsync<ArgumentException>(() =>
                service.BulkCreateAsync(new MenuItemBulkCreateDTO
                {
                    BusinessId = businessId,
                    Items      = []
                }));
        }

        // ── DeleteAsync ──────────────────────────────────────────────────────

        [Fact]
        public async Task DeleteAsync_ExistingItem_ReturnsTrueAndDeletes()
        {
            var item = new MenuItem
            {
                MenuItemId = "item-1",
                BusinessId = "biz-7",
                Name       = "Burger"
            };

            _itemRepoMock.Setup(r => r.GetByIdAsync("item-1")).ReturnsAsync(item);
            _uowMock.Setup(u => u.CompleteAsync()).ReturnsAsync(1);
            _aiSyncMock.Setup(a => a.SyncBusinessAsync(item.BusinessId)).Returns(Task.CompletedTask);

            var service = CreateService();
            var result  = await service.DeleteAsync("item-1");

            Assert.True(result);
            _itemRepoMock.Verify(r => r.Delete(item), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ItemNotFound_ReturnsFalse()
        {
            _itemRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<string>())).ReturnsAsync((MenuItem?)null);

            var service = CreateService();
            var result  = await service.DeleteAsync("ghost");

            Assert.False(result);
            _itemRepoMock.Verify(r => r.Delete(It.IsAny<MenuItem>()), Times.Never);
        }
    }
}
