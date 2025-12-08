using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_layer.DTOS.Order
{
    public class UpdateOrderStatusDTO
    {
        public int OrderId { get; set; }
        public string Status { get; set; }   // Pending, InProgress, Completed, Cancelled
    }
}
