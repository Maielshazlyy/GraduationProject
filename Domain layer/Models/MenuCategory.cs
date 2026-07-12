using System;
using System.Collections.Generic;

namespace Domain_layer.Models
{
    public class MenuCategory
    {
        public string MenuCategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int DisplayOrder { get; set; } = 0; // ترتيب العرض
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Business relation
        public string BusinessId { get; set; }
        public Business Business { get; set; }

        // MenuItems relation
        public ICollection<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
    }
}

