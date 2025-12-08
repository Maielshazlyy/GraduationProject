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
        public int InteractionId { get; set; }
        public int BusinessId { get; set; }
        public string BusinessName { get; set; }
        
        public string AgentName { get; set; }
        public string Channel { get; set; }
        public string Status { get; set; }
        public bool? IsEnded { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? EndedAt { get; set; }

        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int MessageCount{ get; set; }
        

        public int? HandledByUserId { get; set; }
        public string HandledByUserName { get; set; }

        public List<MessageResponseDTO> Messages { get; set; }
    }
}
