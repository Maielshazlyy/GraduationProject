using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_layer.DTOS.menuItem
{
    public class MenuItemResponseDTO
    {
        public string MenuItemId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string? MenuCategoryId { get; set; }
        public string? MenuCategoryName { get; set; } // اسم الفئة للعرض
        public bool IsAvailable { get; set; }

        public string BusinessId { get; set; }
        public string BusinessName { get; set; }
    }
}
