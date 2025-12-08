using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_layer.DTOS.Ticket
{
   public class CloseTicketDTO
    {
        public int TicketId { get; set; }
        public int ClosedByUserId { get; set; }
    }
}
