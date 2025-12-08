using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_layer.DTOS.Feedback
{
    public class FeedbackResponseDTO
    {
        public int FeedbackId { get; set; }
        public int? TicketId { get; set; }
        public string TickerSubject { get; set; }
        public int CustomerId { get; set; }

        public string CustomerName { get; set; }

        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
