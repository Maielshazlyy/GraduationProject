using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_layer.Models
{
   public class Subscription
    {
        public int Id { get; set; }
        public string SubscriptionId { get; set; } = Guid.NewGuid().ToString();
        public string PlanName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; }
        //business relations
        public int BusinessId { get; set; }
        public Business Business { get; set; }
        public ICollection<PaymentTransaction> PaymentTransactions { get; set; } = new List<PaymentTransaction>();

    }
}
