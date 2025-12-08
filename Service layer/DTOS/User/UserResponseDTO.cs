using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_layer.DTOS.User
{
   public class UserResponseDTO
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public int BusinessId { get; set; }
        public string BusinessName { get; set; }
        public int TotalHandledInteractions { get; set; }
        public int TotalAssignedTickets { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
