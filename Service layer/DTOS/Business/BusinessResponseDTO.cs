using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_layer.DTOS.Business
{
    public class BusinessResponseDTO
    {
        public string Id { get; set; }
        public string BusinessId { get; set; }

        public string Name { get; set; }
        public string Type { get; set; }

        public string Address { get; set; }
        public string Phone { get; set; }

        public DateTime CreatedAt { get; set; }

        // Optional statistics
        public int TotalUsers { get; set; }
        public int TotalCustomers { get; set; }
        public int TotalTickets { get; set; }
    }
}
