using Domain_layer.Models;
using Service_layer.DTOS.MenuCategory;

namespace Service_layer.Mapping
{
    public static class MenuCategoryMapping
    {
        public static MenuCategoryResponseDTO ToDto(this MenuCategory mc)
        {
            return new MenuCategoryResponseDTO
            {
                MenuCategoryId = mc.MenuCategoryId,
                Name = mc.Name,
                Description = mc.Description,
                DisplayOrder = mc.DisplayOrder,
                IsActive = mc.IsActive,
                CreatedAt = mc.CreatedAt,
                BusinessId = mc.BusinessId,
                BusinessName = mc.Business?.Name ?? "",
                MenuItemsCount = mc.MenuItems?.Count ?? 0
            };
        }

        public static IEnumerable<MenuCategoryResponseDTO> ToDtoList(this IEnumerable<MenuCategory> list)
            => list.Select(mc => mc.ToDto());
    }
}

