using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain_layer.Models;
using Service_layer.DTOS.menuItem;

namespace Service_layer.Mapping
{
    public static class MenuItemMapping
    {
        public static MenuItemResponseDTO ToDto(this MenuItem m)
        {
            return new MenuItemResponseDTO
            {
                MenuItemId = m.MenuItemId,
                Name = m.Name,
                Description = m.Description,
                Price = m.Price,
                Category = m.Category,
                IsAvailable = m.IsAvailable,

                BusinessId = m.BusinessId,
                BusinessName = m.Business?.Name ?? ""
            };
        }

        public static IEnumerable<MenuItemResponseDTO> ToDtoList(this IEnumerable<MenuItem> list)
            => list.Select(m => m.ToDto());
    }
}
