using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_layer.DTOS.Ticket
{
  public  class TicketResponseDTO
    {
        public string Id { get; set; }
        public string TicketId { get; set; }
        public string BusinessId { get; set; }
        public string BusinessName { get; set; }

        public string Subject { get; set; }
        public string Status { get; set; }
        public bool IsEnded { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ClosedAt { get; set; }

        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int TotalFeedback { get; set; }
       

        public string? AssignedToUserId { get; set; }
        public string AssignedToUserName { get; set; }

        }
}
