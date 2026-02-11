using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_layer.DTOS.KnowledgeBase
{
    public class KnowledgeBaseCreateDTO
    {
        public string Question { get; set; }
        public string Answer { get; set; }
        public string BusinessId { get; set; }
        public bool IsFAQ { get; set; } = false; // true = FAQ (public), false = KnowledgeBase (internal)
        public int DisplayOrder { get; set; } = 0; // For FAQs ordering
        public bool IsActive { get; set; } = true; // Can be disabled
    }
}
