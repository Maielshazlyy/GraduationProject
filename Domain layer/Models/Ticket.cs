using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_layer.Models
{
    public class Ticket
    {
        public string Id { get; set; }

        public string TicketId { get; set; } = Guid.NewGuid().ToString();

        public string Subject { get; set; } = string.Empty;

        /// <summary>
        /// Ticket status: Open, InProgress, Escalated, Closed.
        /// </summary>
        public string Status { get; set; } = "Open";

        public bool IsEnded { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? ClosedAt { get; set; }

        /// <summary>
        /// Ticket type for reporting and routing:
        /// LateDelivery, WrongOrder, MissingItem, PaymentIssue, QualityIssue, HumanEscalation, etc.
        /// </summary>
        public string? TicketType { get; set; }

        /// <summary>
        /// Optional link to the interaction that created this ticket.
        /// </summary>
        public string? InteractionId { get; set; }

        /// <summary>
        /// Optional link to a related order.
        /// </summary>
        public string? RelatedOrderId { get; set; }

        // Feedbacks
        public ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

        // Business relations
        public string BusinessId { get; set; }
        public Business Business { get; set; }

        // User relations
        public string? AssignedToUserId { get; set; }
        public User? AssignedToUser { get; set; }

        // Customer relations
        public string CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}
