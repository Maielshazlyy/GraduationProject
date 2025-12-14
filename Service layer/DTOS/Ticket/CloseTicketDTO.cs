using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_layer.DTOS.Ticket
{
   public class CloseTicketDTO
    {
        public string TicketId { get; set; }
        public string ClosedByUserId { get; set; }
    }
}
