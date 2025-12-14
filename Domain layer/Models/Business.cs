using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_layer.Models
{
   public class Business
    {
      
        public string Id { get; set; }

        
        public string BusinessId { get; set; } = Guid.NewGuid().ToString();

        
        public string Name { get; set; } = string.Empty;

        public string Type { get; set; } = string.Empty;

        
        public string Address { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

       
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<User> Users { get; set; } = new List<User>();
        public ICollection<Customer> Customers { get; set; } = new List<Customer>();
        public ICollection<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
        public ICollection<Interaction> Interactions { get; set; } = new List<Interaction>();
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
        public ICollection<Report> Reports { get; set; } = new List<Report>();
        public ICollection<KnowledgeBase> KnowledgeBases { get; set; } = new List<KnowledgeBase>();

        public Setting? Setting { get; set; }
        public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();


        public ICollection<Integration> Integrations  { get; set; } = new List<Integration>();
        public ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();
    }
}
