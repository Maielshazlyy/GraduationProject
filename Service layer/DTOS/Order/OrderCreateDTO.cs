using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_layer.DTOS.Order
{
    public class OrderCreateDTO
    {
        public int CustomerId { get; set; }
        public int BusinessId { get; set; }

        public List<OrderItemDTO> Items { get; set; }
    }
}
