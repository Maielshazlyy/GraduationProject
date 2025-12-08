using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_layer.DTOS.Feedback
{
    public class FeedbackCreateDTO
    {
        public int TicketId { get; set; }
        public int CustomerId { get; set; }
        public int Rating { get; set; }    // 1–5
        public string Comment { get; set; }
    }
}
