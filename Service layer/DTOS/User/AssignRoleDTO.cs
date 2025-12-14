using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_layer.DTOS.User
{
   public class AssignRoleDTO
    {
        public string UserId { get; set; }
        public string NewRole { get; set; } // "Admin", "Owner", "Agent"
    }
}
