using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_layer.DTOS.Order
{
    public class OrderItemDTO
    {
        public int MenuItemId { get; set; }
        public int Quantity { get; set; }
    }
}
