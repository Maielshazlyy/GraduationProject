using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_layer.Models
{
   public class AuditLog
    {

        public int AuditLogId { get; set; }

        public int BusinessId { get; set; }
        public string Action { get; set; }

        public string Entity { get; set; }

        public int EntityId { get; set; }

        public DateTime CreatedAt { get; set; }

        public Business Business { get; set; }

        public int? UserId { get; set; }
        public User? User { get; set; }
    }
}
