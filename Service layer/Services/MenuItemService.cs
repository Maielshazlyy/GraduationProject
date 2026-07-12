using System;
using Domain_layer.Interfaces;
using Domain_layer.Models;
using Service_layer.DTOS.menuItem;
using Service_layer.Services_Interfaces;

namespace Service_layer.Services
{
    public class MenuItemService : IMenuItemService
    {
        private readonly IMenuItemRepository _menuItemRepository;
        private readonly IBusinessRepository _businessRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAiKnowledgeSyncService? _aiSync;

        public MenuItemService(
            IMenuItemRepository menuItemRepository,
            IBusinessRepository businessRepository,
            IUnitOfWork unitOfWork,
            IAiKnowledgeSyncService? aiSync = null)
        {
            _menuItemRepository = menuItemRepository;
            _businessRepository = businessRepository;
            _unitOfWork         = unitOfWork;
            _aiSync             = aiSync;
        }

        public async Task<IEnumerable<MenuItem>> GetAllAsync()
        {
            return await _menuItemRepository.GetAllAsync();
        }

        public async Task<IEnumerable<MenuItem>> GetByBusinessIdAsync(string businessId)
        {
            return await _menuItemRepository.GetByBusinessIdAsync(businessId);
        }

        public async Task<MenuItem?> GetByIdAsync(string id)
        {
            return await _menuItemRepository.GetByIdAsync(id);
        }

        public async Task<MenuItem> CreateAsync(MenuItemCreateDTO dto)
        {
            var business = await _businessRepository.GetByIdAsync(dto.BusinessId);
            if (business == null)
                throw new ArgumentException($"Business with id '{dto.BusinessId}' not found.");

            var menuItem = new MenuItem
            {
                MenuItemId = Guid.NewGuid().ToString(),
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                MenuCategoryId = dto.MenuCategoryId, // يمكن أن يكون null
                IsAvailable = true, // Default to available when creating
                BusinessId = dto.BusinessId
            };

            await _menuItemRepository.AddAsync(menuItem);
            await _unitOfWork.CompleteAsync();

            await TrySyncAsync(menuItem.BusinessId);
            return menuItem;
        }

        public async Task<IEnumerable<MenuItem>> BulkCreateAsync(MenuItemBulkCreateDTO dto)
        {
            var business = await _businessRepository.GetByIdAsync(dto.BusinessId);
            if (business == null)
                throw new ArgumentException($"Business with id '{dto.BusinessId}' not found.");

            if (dto.Items == null || !dto.Items.Any())
                throw new ArgumentException("Items list cannot be empty.");

            // Cache categories by name to avoid repeated DB lookups
            var existingCategories = (await _unitOfWork.MenuCategories.GetByBusinessIdAsync(dto.BusinessId))
                .ToDictionary(c => c.Name.ToLower());

            var items = new List<MenuItem>();
            foreach (var i in dto.Items)
            {
                string? categoryId = null;

                if (!string.IsNullOrWhiteSpace(i.CategoryName))
                {
                    var key = i.CategoryName.Trim().ToLower();
                    if (!existingCategories.TryGetValue(key, out var category))
                    {
                        // Create category if it doesn't exist
                        category = new Domain_layer.Models.MenuCategory
                        {
                            MenuCategoryId = Guid.NewGuid().ToString(),
                            Name           = i.CategoryName.Trim(),
                            BusinessId     = dto.BusinessId,
                            IsActive       = true
                        };
                        await _unitOfWork.MenuCategories.AddAsync(category);
                        existingCategories[key] = category;
                    }
                    categoryId = category.MenuCategoryId;
                }

                items.Add(new MenuItem
                {
                    MenuItemId     = Guid.NewGuid().ToString(),
                    Name           = i.Name,
                    Description    = i.Description,
                    Price          = i.Price,
                    MenuCategoryId = categoryId,
                    IsAvailable    = i.IsAvailable,
                    BusinessId     = dto.BusinessId
                });
            }

            foreach (var item in items)
                await _menuItemRepository.AddAsync(item);

            await _unitOfWork.CompleteAsync();
            await TrySyncAsync(dto.BusinessId);

            return items;
        }

        public async Task<MenuItem?> UpdateAsync(string id, MenuItemUpdateDTO dto)
        {
            var menuItem = await _menuItemRepository.GetByIdAsync(id);
            if (menuItem == null) return null;

            menuItem.Name           = dto.Name;
            menuItem.Description    = dto.Description;
            menuItem.Price          = dto.Price;
            menuItem.MenuCategoryId = dto.MenuCategoryId;
            menuItem.IsAvailable    = dto.IsAvailable;

            _menuItemRepository.Update(menuItem);
            await _unitOfWork.CompleteAsync();

            await TrySyncAsync(menuItem.BusinessId);
            return menuItem;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var menuItem = await _menuItemRepository.GetByIdAsync(id);
            if (menuItem == null) return false;

            var businessId = menuItem.BusinessId;
            _menuItemRepository.Delete(menuItem);
            await _unitOfWork.CompleteAsync();

            await TrySyncAsync(businessId);
            return true;
        }

        // ── Helpers ───────────────────────────────────────────────────────────

        /// <summary>
        /// Pushes the updated knowledge base to the AI after a menu change.
        /// Non-fatal — a sync failure must never block the business operation.
        /// </summary>
        private async Task TrySyncAsync(string businessId)
        {
            if (_aiSync != null)
            {
                try   { await _aiSync.SyncBusinessAsync(businessId); }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[AiSync] MenuItemService sync failed for {businessId}: {ex.Message}");
                }
            }
        }
    }
}

