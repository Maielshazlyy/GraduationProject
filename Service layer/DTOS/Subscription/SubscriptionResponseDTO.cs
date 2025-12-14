using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_layer.DTOS.Subscription
{
    public class SubscriptionResponseDTO
    {
        public string Id { get; set; }
        public string SubscriptionId { get; set; }
        public string PlanName { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
        public string BusinessId { get; set; }
        public string BusinessName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        }
    }

