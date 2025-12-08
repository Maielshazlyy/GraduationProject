using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_layer.DTOS.Integration
{
    public class IntegrationConnectDTO
    {
        public int BusinessId { get; set; }
        public string PlatformName { get; set; }       // WhatsApp, Foodics, Talabat, Shopify...
        public string ApiKeyOrConfig { get; set; }     // Token, API key, JSON config
    }
}
