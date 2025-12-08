using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_layer.DTOS.Notification
{
    public class NotificationResponseDTO
    {
        public int NotificationId { get; set; }

        public string Title { get; set; }
        public string Message { get; set; }

        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }

        // Who sent it / who it belongs to
        public int? UserId { get; set; }
        public string? UserName { get; set; }

        // Linked business
        public int BusinessId { get; set; }
        public string BusinessName { get; set; }

        // Optional: notification category (system - ticket - payment - ai alert)
        public string? Type { get; set; }
    }
}
