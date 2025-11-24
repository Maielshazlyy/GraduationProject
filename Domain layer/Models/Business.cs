using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_layer.Models
{
   public class Business
    {
      
        public int Id { get; set; }

        
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
        public Setting? Settings { get; set; }
        public Subscription? Subscriptions { get; set; }



    }
}
