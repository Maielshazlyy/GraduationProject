using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_layer.Models
{
   public class MenuItem
    {

        public string MenuItemId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public string? MenuCategoryId { get; set; } // Foreign Key (nullable للتوافق مع البيانات القديمة)
        public MenuCategory? MenuCategory { get; set; }

        public bool IsAvailable { get; set; }

        public string BusinessId { get; set; }
        public Business Business { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    }
}
