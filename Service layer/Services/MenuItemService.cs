using System;
using Domain_layer.Interfaces;
using Domain_layer.Models;
using Service_layer.DTOS.menuItem;
using Service_layer.Services_Interfaces;

namespace Service_layer.Services
{
    public class MenuItemService : IMenuItemService
    {
        private readonly IRepository<MenuItem> _menuItemRepository;
        private readonly IRepository<Business> _businessRepository;
        private readonly IUnitOfWork _unitOfWork;

        public MenuItemService(
            IRepository<MenuItem> menuItemRepository,
            IRepository<Business> businessRepository,
            IUnitOfWork unitOfWork)
        {
            _menuItemRepository = menuItemRepository;
            _businessRepository = businessRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<MenuItem>> GetAllAsync()
        {
            return await _menuItemRepository.GetAllAsync();
        }

        public async Task<IEnumerable<MenuItem>> GetByBusinessIdAsync(string businessId)
        {
            var allItems = await _menuItemRepository.GetAllAsync();
            return allItems.Where(m => m.BusinessId == businessId);
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
                Category = dto.Category,
                IsAvailable = dto.IsAvailable,
                BusinessId = dto.BusinessId
            };

            await _menuItemRepository.AddAsync(menuItem);
            await _unitOfWork.CompleteAsync();
            return menuItem;
        }

        public async Task<MenuItem?> UpdateAsync(string id, MenuItemUpdateDTO dto)
        {
            var menuItem = await _menuItemRepository.GetByIdAsync(id);
            if (menuItem == null) return null;

            menuItem.Name = dto.Name;
            menuItem.Description = dto.Description;
            menuItem.Price = dto.Price;
            menuItem.Category = dto.Category;
            menuItem.IsAvailable = dto.IsAvailable;

            _menuItemRepository.Update(menuItem);
            await _unitOfWork.CompleteAsync();
            return menuItem;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var menuItem = await _menuItemRepository.GetByIdAsync(id);
            if (menuItem == null) return false;

            _menuItemRepository.Delete(menuItem);
            await _unitOfWork.CompleteAsync();
            return true;
        }
    }
}

