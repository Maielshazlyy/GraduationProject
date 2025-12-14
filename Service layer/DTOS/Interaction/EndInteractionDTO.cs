using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_layer.DTOS.Interaction
{
    public class EndInteractionDTO
    {
        public string InteractionId { get; set; }
        public int? HandledByUserId { get; set; } // ممكن يكون Agent أو Null لو AI
    }
}
