using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_layer.DTOS.Setting
{
    public class SettingUpdateDTO
    {
        public bool AutoAssignTickets { get; set; }
        public bool EnableNotifications { get; set; }
        public string Language { get; set; }
    }
}
 