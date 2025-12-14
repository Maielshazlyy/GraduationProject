using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_layer.DTOS.Interaction
{
    public class StartInteractionDTO
    {
        public string CustomerId { get; set; }
        public string BusinessId { get; set; }
        public string Channel { get; set; } // WhatsApp, Voice, Web
    }
}
