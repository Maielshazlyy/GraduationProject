using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_layer.DTOS.Subscription
{
    public class SubscriptionRenewDTO
    {
        public string SubscriptionId { get; set; }          // Required
        public DateTime NewStartDate { get; set; }       // Usually = Today
        public DateTime? NewEndDate { get; set; }        // Optional (monthly / yearly)

        public string? NewPlanName { get; set; }         // optional: change plan
        public decimal? NewPrice { get; set; }           // optional: updated price

        public bool AutoRenewEnabled { get; set; }       // if business wants auto renew
    }
}
