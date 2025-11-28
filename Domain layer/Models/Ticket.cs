using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_layer.Models
{
    public class Ticket
    {

        public int Id { get; set; }
        public string TicketId {  get; set; } = Guid.NewGuid().ToString();
        public string Subject { get; set; }
        public string Status { get; set; }
        public bool IsEnded { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime ClosedAt {  get; set; }

        //Feedbackes
        public int FeedbackId { get; set; }
        public List<Feedback> Feedbacks { get; set; }


        //business relations
        public int BusinessId { get; set; }
        public Business Business { get; set; }

        //user relations
        public int? AssignedToUserId { get; set; }
        public User? AssignedToUser { get; set; }

        //customer relations
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        

    }
}
