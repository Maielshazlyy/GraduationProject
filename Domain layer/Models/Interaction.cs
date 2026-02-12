using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_layer.Models
{
    public class Interaction
    {
        public string InteractionId { get; set; }

        /// <summary>
        /// Channel of the interaction, e.g. WebChat, WhatsApp, Voice.
        /// </summary>
        public string Channel { get; set; } = string.Empty;

        /// <summary>
        /// High-level interaction type: Informational, Order, Ticket, Mixed, etc.
        /// </summary>
        public string? InteractionType { get; set; }

        /// <summary>
        /// Status of the interaction: Open, InProgress, Escalated, Closed.
        /// </summary>
        public string Status { get; set; } = "Open";

        public bool? IsEnded { get; set; }

        public DateTime StartedAt { get; set; } = DateTime.UtcNow;
        public DateTime? EndedAt { get; set; }

        public string BusinessId { get; set; }
        public Business Business { get; set; }

        public string? HandledByUserId { get; set; }
        public User? HandledByUser { get; set; }

        public string CustomerId { get; set; }
        public Customer Customer { get; set; }

        /// <summary>
        /// Optional link to an order created within this interaction.
        /// </summary>
        public string? RelatedOrderId { get; set; }

        /// <summary>
        /// Optional link to a support ticket created within this interaction.
        /// </summary>
        public string? RelatedTicketId { get; set; }

        public ICollection<Message> Messages { get; set; } = new List<Message>();
    }
    }
