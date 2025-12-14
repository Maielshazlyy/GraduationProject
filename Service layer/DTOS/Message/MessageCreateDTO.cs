using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_layer.DTOS.Message
{
    public class MessageCreateDTO
    {
        public string InteractionId { get; set; }
        public string SenderType { get; set; } // AI, Customer, Agent
        public string Content { get; set; }
        public string? UserId { get; set; } // Only if Agent is the sender
    }
}
