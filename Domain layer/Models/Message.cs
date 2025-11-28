using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_layer.Models
{
    public class Message
    {
        public int MessageId { get; set; }

        //InterAction Relationship 
        public string InteractionId { get; set; }
        public Interaction Interaction { get; set; }

        //Sentimen 
        public string SentimentId { get; set; }
        public Sentiment Sentiment { get; set; }

        public string SenderType { get; set; }
        public string ContentType { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

    }
}
