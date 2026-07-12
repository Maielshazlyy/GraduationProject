using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_layer.DTOS.KnowledgeBase
{
    public class KnowledgeBaseResponseDTO
    {
        public string KnowledgeBaseId { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public DateTime CreatedAt { get; set; }
        public string BusinessId { get; set; }
        public string BusinessName { get; set; }
        public bool IsFAQ { get; set; } // true = FAQ (public), false = KnowledgeBase (internal)
        public int DisplayOrder { get; set; } // For FAQs ordering
        public bool IsActive { get; set; } // Can be disabled
    }
}
