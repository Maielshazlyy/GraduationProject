using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service_layer.DTOS.Message;

namespace Service_layer.DTOS.Interaction
{
    public class InteractionResponseDTO
    {
        public string InteractionId { get; set; }
        public string BusinessId { get; set; }
        public string BusinessName { get; set; }
        
        public string AgentName { get; set; }
        public string Channel { get; set; }
        public string Status { get; set; }
        public bool? IsEnded { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? EndedAt { get; set; }

        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int MessageCount{ get; set; }
        

        public string? HandledByUserId { get; set; }
        public string HandledByUserName { get; set; }

        public List<MessageResponseDTO> Messages { get; set; }
    }
}
