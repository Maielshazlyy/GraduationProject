using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_layer.Models
{
   public class User
    {
        public int UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = "User"; // Admin, Agent, business owner
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        //business relations
        public int BusinessId { get; set; }
        public Business Business { get; set; }
        
        public ICollection<Interaction> InteractionsHandled { get; set; } = new List<Interaction>();
        public ICollection<Ticket> TicketsAssigned { get; set; } = new List<Ticket>();
        public ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
        public ICollection<Message> Messages { get; set; } = new List<Message>();




    }
}
