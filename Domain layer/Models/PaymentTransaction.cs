using Domain_layer.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_layer.Models
{
   public class PaymentTransaction
    {
        public string Id { get; set; }
        public string PaymentId { get; set; } = Guid.NewGuid().ToString();

        public int SubscriptionId { get; set; }

        public decimal Amount { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public DateTime TransactionDate { get; set; }

        public PaymentStatus Status { get; set; }

        public Subscription Subscription { get; set; }
       
    }
}
