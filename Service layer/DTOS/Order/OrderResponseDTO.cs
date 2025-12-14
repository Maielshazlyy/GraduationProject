using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_layer.DTOS.Order
{
    public class OrderResponseDTO
    {
        public string OrderId { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public string BusinessId { get; set; }

        public string BusinessName { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }

        public List<OrderItemDetailDTO> Items { get; set; }
    }
}
