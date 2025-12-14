using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_layer.Models
{
  public class OrderItem
    {
        public string OrderItemId { get; set; }

        public string OrderId { get; set; }
        public string MenuItemId { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public Order Order { get; set; }
        public MenuItem MenuItem { get; set; }
    }
}
