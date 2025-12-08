using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_layer.DTOS.Customer
{
  public  class CustomerResponseDTO
    {
        public int CustomerId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime CreatedAt { get; set; }
        public int TotalOrders { get; set; }
        public int TotalTickets { get; set; }

        public int BusinessId { get; set; }
        public string BusinessName { get; set; }
    }
}
