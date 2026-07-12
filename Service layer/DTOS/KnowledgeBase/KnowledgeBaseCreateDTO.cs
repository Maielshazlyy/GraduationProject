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
        public bool IsFAQ { get; set; } = false;
        public int DisplayOrder { get; set; } = 0;
        public bool IsActive { get; set; } = true;
    }

    public class KnowledgeBulkEntryDTO
    {
        public string Question { get; set; }
        public string Answer { get; set; }
        public bool IsFAQ { get; set; } = false;
        public int DisplayOrder { get; set; } = 0;
        public bool IsActive { get; set; } = true;
    }

    public class KnowledgeBaseBulkCreateDTO
    {
        public string BusinessId { get; set; }
        public List<KnowledgeBulkEntryDTO> Entries { get; set; } = new();
    }
}
