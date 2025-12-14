using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_layer.DTOS.Customer
{
    public class CustomerCreateDTO 
    {
        public string FullName { get; set; }
        public string Email { get; set; }     // optional
        public string Phone { get; set; }     // optional
        public string BusinessId { get; set; }

    }
}
