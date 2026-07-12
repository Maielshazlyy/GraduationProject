using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_layer.DTOS.User
{
   public class UserUpdateDTO
   {
        public string FullName { get; set; }
        public string Email { get; set; }
    }

    /// <summary>
    /// DTO used by a Business Owner to create a new human employee (Agent)
    /// under their business.
    /// </summary>
    public class CreateHumanEmployeeDTO
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
