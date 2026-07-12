using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_layer.DTOS.Order
{
    public class UpdateOrderStatusDTO
    {
        public string OrderId { get; set; }

        // Must match OrderStatus enum exactly:
        // Pending | PendingConfirmation | Paid | DelayedRisk | Delivered | Cancelled
        public string Status { get; set; }
    }
}
