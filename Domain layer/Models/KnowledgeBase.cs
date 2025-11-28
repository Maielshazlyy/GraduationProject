using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_layer.Models
{
  public class KnowledgeBase
    {

        public int KnowledgeBaseId { get; set; }

        public string Question { get; set; }
        public string Answer { get; set; }
        
        public DateTime CreatedAt { get; set; }

        public int BusinessId { get; set; }
        public Business Business { get; set; }

    }
}
