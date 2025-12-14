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
    }
}
