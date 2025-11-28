using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_layer.Models
{
   public class Notification
    {

        public int NotificationId { get; set; }

        public string Message { get; set; }
        public string Title { get; set; }
        public bool IsRead { get; set; }


        //business relations
        public int BusinessId { get; set; }
        public Business Business { get; set; }

        //user relations
        public int UserId { get; set; }
        public User User { get; set; }


        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
    }
}
