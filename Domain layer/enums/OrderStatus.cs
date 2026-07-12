using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_layer.enums
{
    public enum OrderStatus
    {
        Pending,
        PendingConfirmation, // For abandoned orders that need follow-up
        Paid,
        DelayedRisk, // System detected potential delivery delay
        Delivered,
        Cancelled
    }
}
