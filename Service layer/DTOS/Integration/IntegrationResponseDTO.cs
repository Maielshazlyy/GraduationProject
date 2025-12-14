using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_layer.DTOS.Integration
{
    public class IntegrationResponseDTO
    {
        public string Id { get; set; }
        public string IntegrationId { get; set; }

        public string PlatformName { get; set; }
        public string Status { get; set; }        // Active / Error / Disabled

        public DateTime? LastSyncDate { get; set; }
        public string BusinessId { get; set; }
        public string BusinessName { get; set; }
    }
}
