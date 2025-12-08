using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_layer.DTOS.Integration
{
    public class IntegrationSyncDTO
    {
        public int IntegrationId { get; set; }
        public string SyncType { get; set; } = "Full";
        // Full, OrdersOnly, MenuItemsOnly, MessagesOnly, etc.
    }
}
