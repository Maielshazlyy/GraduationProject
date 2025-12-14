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
        public string TicketId {  get; set; } = Guid.NewGuid().ToString();
        public string Subject { get; set; } = string.Empty;
        public string Status { get; set; } = "Open"; //open,in progress ,closed
        public bool IsEnded { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? ClosedAt {  get; set; }

        //Feedbackes
        public ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();


        //business relations
        public string BusinessId { get; set; }
        public Business Business { get; set; }

        //user relations
        public string? AssignedToUserId { get; set; }
        public User? AssignedToUser { get; set; }

        //customer relations
        public string CustomerId { get; set; }
        public Customer Customer { get; set; }

        

    }
}
