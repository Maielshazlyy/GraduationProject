using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_layer.DTOS.Ticket
{
   public class TicketCreateDTO
    {
        public string Subject { get; set; }
        public int CustomerId { get; set; }
        public int BusinessId { get; set; }
    }
}
