using Domain_layer.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_layer.Models
{
  public  class Integration
    {
        public int Id { get; set; }
        public string IntegrationId { get; set; } = Guid.NewGuid().ToString();

        public int BusinessId { get; set; }

        public string PlatformName { get; set; }

        public string ApiKeyOrConfig { get; set; }

        public IntegrationStatus Status { get; set; }

        public DateTime? LastSyncDate { get; set; }

        public Business Business { get; set; }
    }
}
