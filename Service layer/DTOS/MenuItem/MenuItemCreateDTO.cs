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
        public string? MenuCategoryId { get; set; }
        public string BusinessId { get; set; }
    }

    public class MenuItemBulkEntryDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string? CategoryName { get; set; }
        public bool IsAvailable { get; set; } = true;
    }

    public class MenuItemBulkCreateDTO
    {
        public string BusinessId { get; set; }
        public List<MenuItemBulkEntryDTO> Items { get; set; } = new();
    }
}
