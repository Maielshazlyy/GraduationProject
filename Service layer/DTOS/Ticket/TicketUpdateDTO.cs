using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_layer.DTOS.Ticket
{
   public class TicketUpdateDTO
    {
        public string Subject { get; set; }
        public string Status { get; set; }  // Open, InProgress, Closed
    }
}
