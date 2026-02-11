using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_layer.DTOS.menuItem
{
    public class MenuItemCreateDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string? MenuCategoryId { get; set; } // Optional - يمكن إنشاء menu item بدون category
        public string BusinessId { get; set; }
    }
}
