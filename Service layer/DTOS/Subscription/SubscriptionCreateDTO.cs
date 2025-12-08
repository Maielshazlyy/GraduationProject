using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_layer.DTOS.Subscription
{
    public class SubscriptionCreateDTO
    {
        public string PlanName { get; set; }
        public decimal Price { get; set; }
        public int BusinessId { get; set; }
    }
}
