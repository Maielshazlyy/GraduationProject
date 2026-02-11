using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_layer.Models
{
  public class KnowledgeBase
    {
        public string KnowledgeBaseId { get; set; }
        public string Question { get; set; } = string.Empty;
        public string Answer { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Distinguish between FAQ (public, for customers) and KnowledgeBase (internal, for chatbot)
        public bool IsFAQ { get; set; } = false; // true = FAQ (public), false = KnowledgeBase (internal)
        public int DisplayOrder { get; set; } = 0; // For FAQs ordering
        public bool IsActive { get; set; } = true; // Can be disabled

        public string BusinessId { get; set; }
        public Business Business { get; set; }

    }
}
