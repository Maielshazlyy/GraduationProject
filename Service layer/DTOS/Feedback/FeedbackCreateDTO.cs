using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_layer.DTOS.Feedback
{
    public class FeedbackCreateDTO
    {
        public string? TicketId { get; set; }

        /// <summary>
        /// Optional link to the Interaction where feedback was collected. Lets feedback be
        /// submitted right after a chat/call ends without needing a support ticket to exist.
        /// </summary>
        public string? InteractionId { get; set; }
        public string CustomerId { get; set; }
        public int Rating { get; set; }    // 1–5
        public string Comment { get; set; }
    }
}
