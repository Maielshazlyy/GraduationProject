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
    }
}
