using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_layer.Models
{
    public class Interaction
    {
        public int InteractionId { get; set; }
        public string Channel { get; set; } = string.Empty; // e.g. WhatsApp, Voice, Web

        public string Status { get; set; } = "Open"; //open,in progress, closed

        public bool? IsEnded { get; set; }

        public DateTime StartedAt { get; set; } = DateTime.UtcNow;
        public DateTime? EndedAt { get; set; }

        public int BusinessId { get; set; }
        public Business Business { get; set; }

        public int? HandledByUserId { get; set; }
        public User? HandledByUser { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public ICollection<Message> Messages { get; set; } = new List<Message>();

    }
    }
