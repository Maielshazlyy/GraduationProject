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

        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        //business relations
        public int BusinessId { get; set; }
        public Business Business { get; set; }

        //user relations
        public int UserId { get; set; }
        public User User { get; set; }


        
    }
}
