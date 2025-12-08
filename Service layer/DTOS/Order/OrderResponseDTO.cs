using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_layer.DTOS.Order
{
    public class OrderResponseDTO
    {
        public int OrderId { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public int BusinessId { get; set; }

        public string BusinessName { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }

        public List<OrderItemDetailDTO> Items { get; set; }
    }
}
