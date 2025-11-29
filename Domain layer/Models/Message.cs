using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_layer.Models
{
    public class Message
    {
        public int MessageId { get; set; }
        public string SenderType { get; set; } = string.Empty; // "Customer", "Agent", "AI"
        public string Content { get; set; } = string.Empty;
        public DateTime SentAt { get; set; } = DateTime.UtcNow;

        // Interaction relation (required)
        public int InteractionId { get; set; }
        public Interaction Interaction { get; set; }

        // Optional sender user (if sender is an agent)
        public int? UserId { get; set; }
        public User? User { get; set; }

        // Optional sentiment navigation (1:1)
        public Sentiment? Sentiment { get; set; }


    }
}
