using System;
using System.Linq;
using Domain_layer.Interfaces;
using Domain_layer.Models;
using Service_layer.DTOS.MenuCategory;
using Service_layer.Services_Interfaces;

namespace Service_layer.Services
{
    public class MenuCategoryService : IMenuCategoryService
    {
        private readonly IMenuCategoryRepository _menuCategoryRepository;
        private readonly IBusinessRepository _businessRepository;
        private readonly IUnitOfWork _unitOfWork;

        public MenuCategoryService(
            IMenuCategoryRepository menuCategoryRepository,
            IBusinessRepository businessRepository,
            IUnitOfWork unitOfWork)
        {
            _menuCategoryRepository = menuCategoryRepository;
            _businessRepository = businessRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<MenuCategory>> GetAllAsync()
        {
            return await _menuCategoryRepository.GetAllAsync();
        }

        public async Task<IEnumerable<MenuCategory>> GetByBusinessIdAsync(string businessId)
        {
            return await _menuCategoryRepository.GetByBusinessIdAsync(businessId);
        }

        public async Task<IEnumerable<MenuCategory>> GetActiveByBusinessIdAsync(string businessId)
        {
            return await _menuCategoryRepository.GetActiveByBusinessIdAsync(businessId);
        }

        public async Task<MenuCategory?> GetByIdAsync(string id)
        {
            return await _menuCategoryRepository.GetByIdAsync(id);
        }

        public async Task<MenuCategory> CreateAsync(MenuCategoryCreateDTO dto)
        {
            var business = await _businessRepository.GetByIdAsync(dto.BusinessId);
            if (business == null)
                throw new ArgumentException($"Business with id '{dto.BusinessId}' not found.");

            var category = new MenuCategory
            {
                MenuCategoryId = Guid.NewGuid().ToString(),
                Name = dto.Name,
                Description = dto.Description,
                DisplayOrder = dto.DisplayOrder,
                IsActive = true,
                BusinessId = dto.BusinessId,
                CreatedAt = DateTime.UtcNow
            };

            await _menuCategoryRepository.AddAsync(category);
            await _unitOfWork.CompleteAsync();
            return category;
        }

        public async Task<MenuCategory?> UpdateAsync(string id, MenuCategoryUpdateDTO dto)
        {
            var category = await _menuCategoryRepository.GetByIdAsync(id);
            if (category == null) return null;

            category.Name = dto.Name;
            category.Description = dto.Description;
            category.DisplayOrder = dto.DisplayOrder;
            category.IsActive = dto.IsActive;

            _menuCategoryRepository.Update(category);
            await _unitOfWork.CompleteAsync();
            return category;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var category = await _menuCategoryRepository.GetByIdAsync(id);
            if (category == null) return false;

            // التحقق من وجود menu items مرتبطة بهذه الفئة
            if (category.MenuItems != null && category.MenuItems.Any())
            {
                throw new InvalidOperationException($"Cannot delete category '{category.Name}' because it has {category.MenuItems.Count} menu items. Please remove or reassign menu items first.");
            }

            _menuCategoryRepository.Delete(category);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<bool> ReorderCategoriesAsync(string businessId, ReorderCategoriesDTO dto)
        {
            var categories = await _menuCategoryRepository.GetByBusinessIdAsync(businessId);
            var categoryDict = categories.ToDictionary(c => c.MenuCategoryId);

            foreach (var item in dto.Categories)
            {
                if (categoryDict.TryGetValue(item.MenuCategoryId, out var category))
                {
                    category.DisplayOrder = item.DisplayOrder;
                    _menuCategoryRepository.Update(category);
                }
            }

            await _unitOfWork.CompleteAsync();
            return true;
        }
    }
}

