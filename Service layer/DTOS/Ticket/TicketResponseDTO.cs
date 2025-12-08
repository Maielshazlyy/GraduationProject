using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_layer.DTOS.Ticket
{
  public  class TicketResponseDTO
    {
        public int Id { get; set; }
        public string TicketId { get; set; }
        public int BusinessId { get; set; }
        public string BusinessName { get; set; }

        public string Subject { get; set; }
        public string Status { get; set; }
        public bool IsEnded { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ClosedAt { get; set; }

        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int TotalFeedback { get; set; }
       

        public int? AssignedToUserId { get; set; }
        public string AssignedToUserName { get; set; }

        }
}
