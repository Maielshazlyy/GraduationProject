using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_layer.Models
{
   public class AuditLog
    {

        public string AuditLogId { get; set; }

        public string BusinessId { get; set; }
        public string Action { get; set; }

        public string Entity { get; set; }

        public string EntityId { get; set; }

        public DateTime CreatedAt { get; set; }

        public Business Business { get; set; }

        public string? UserId { get; set; }
        public User? User { get; set; }
    }
}
