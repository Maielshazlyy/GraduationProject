using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_layer.Models
{
  public class Feedback
    {
        public string FeedbackId { get; set; } 
        public string Comment { get; set; } = string.Empty;
        public int Rating { get; set; } //1 : 5
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
     
        public string? TicketId { get; set; }
        public Ticket? Ticket { get; set; }

        /// <summary>
        /// Optional link to the Interaction where feedback was collected.
        /// </summary>
        public string? InteractionId { get; set; }
        public Interaction? Interaction { get; set; }

        public string CustomerId { get; set; }
        public Customer Customer { get; set; }

        /// <summary>
        /// Sentiment score derived from feedback (1-5 rating mapped to sentiment).
        /// </summary>
        public double? SentimentScore { get; set; }
    }
}
