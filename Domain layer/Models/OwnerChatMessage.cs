using System;

namespace Domain_layer.Models
{
    /// <summary>
    /// One exchange (owner's question + AI's reply) in the Owner Chat ("Chat with IRIS"
    /// analytics assistant). Persisted so an owner's chat history survives across sessions
    /// and can be cleared on demand.
    /// </summary>
    public class OwnerChatMessage
    {
        public string OwnerChatMessageId { get; set; } = Guid.NewGuid().ToString();

        public string BusinessId { get; set; } = string.Empty;
        public Business? Business { get; set; }

        /// <summary>The Owner/Admin user who sent the message.</summary>
        public string? UserId { get; set; }
        public User? User { get; set; }

        public string Message { get; set; } = string.Empty;
        public string Reply { get; set; } = string.Empty;

        /// <summary>Confidence label returned by the AI (high/medium/low), if any.</summary>
        public string? Confidence { get; set; }

        public DateTime SentAt { get; set; } = DateTime.UtcNow;
    }
}
