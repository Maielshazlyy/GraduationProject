using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_layer.DTOS.Order
{
    public class OrderItemDetailDTO
    {
        public string MenuItemId { get; set; }
        public string MenuItemName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
    }
}
