using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_layer.Models
{
  public class Feedback
    {
        public int FeedbackId { get; set; } 
     
        public string TicketId { get; set; }
        public Ticket? Ticket { get; set; }

        public string CustomerId { get; set; }
        public Customer Customer { get; set; }

        public int Rating { get; set; }
        public string Comment { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
