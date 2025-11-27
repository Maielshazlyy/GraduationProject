using Domain_layer.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_layer.Models
{
    public class Order
    {

        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public decimal TotalPrice { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Customer Customer { get; set; }
        public int BusinessId { get; set; }
        public Business Business { get; set; }

        // Navigation property: One Order → Many OrderItems
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    }
}
