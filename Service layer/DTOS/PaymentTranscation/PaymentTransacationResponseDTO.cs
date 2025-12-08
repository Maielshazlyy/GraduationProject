using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_layer.DTOS.PaymentTranscation
{
    public class PaymentTransacationResponseDTO
    {
        public int Id { get; set; }
        public string PaymentId { get; set; }

        public int SubscriptionId { get; set; }
        public string SubscriptionPlanName { get; set; }

        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Status { get; set; }
    }
}
