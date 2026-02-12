using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_layer.Models
{
    public class Message
    {
        public string MessageId { get; set; }

        /// <summary>
        /// "Customer", "Agent", "AI"
        /// </summary>
        public string SenderType { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public DateTime SentAt { get; set; } = DateTime.UtcNow;

        // Interaction relation 
        public string InteractionId { get; set; }
        public Interaction Interaction { get; set; }

        // Optional sender user (if sender is an agent)
        public string? UserId { get; set; }
        public User? User { get; set; }

        /// <summary>
        /// Detected intent label for this message (if any), e.g. CreateOrder, Complaint, etc.
        /// Stored after intent detection.
        /// </summary>
        public string? Intent { get; set; }

        /// <summary>
        /// Optional JSON blob containing structured AI metadata (entities, confidence, etc.).
        /// </summary>
        public string? AiMetadataJson { get; set; }

        // Optional sentiment navigation (1:1)
        public Sentiment? Sentiment { get; set; }
    }
}
