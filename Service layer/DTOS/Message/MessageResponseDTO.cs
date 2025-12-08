using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service_layer.DTOS.Sentiment;

namespace Service_layer.DTOS.Message
{
    public class MessageResponseDTO
    {
        public int MessageId { get; set; }
        public string SenderType { get; set; }
        public string Content { get; set; }
        public DateTime SentAt { get; set; }
        public int InteractionId { get; set; }
        public int? AgentId { get; set; }
        public string AgentName { get; set; }

        public SentimentResponseDTO Sentiment { get; set; }
    }
}
