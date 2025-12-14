using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_layer.DTOS.Notification
{
    public class NotificationCreateDTO
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public string UserId { get; set; }
        public string BusinessId { get; set; }
    }
}
