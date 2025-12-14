using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_layer.Models
{
   public class Customer
    {
        public string CustomerId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        //business relations
        public string BusinessId { get; set; }
        public Business Business { get; set; }

        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
        public ICollection<Interaction> Interactions { get; set; } = new List<Interaction>();
        public ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();


    }
}
