using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain_layer.Models;
using Service_layer.DTOS.Customer;

namespace Service_layer.Mapping
{
    public static class CustomerMapping
    {
        public static CustomerResponseDTO ToDto(this Customer c)
        {
            return new CustomerResponseDTO
            {
                CustomerId = c.CustomerId,
                FullName = c.FullName,
                Email = c.Email,
                Phone = c.Phone,
                CreatedAt = c.CreatedAt,

                BusinessId = c.BusinessId,
                BusinessName = c.Business?.Name ?? "",

                TotalOrders = c.Orders?.Count ?? 0,
                TotalTickets = c.Tickets?.Count ?? 0
            };
        }

        public static IEnumerable<CustomerResponseDTO> ToDtoList(this IEnumerable<Customer> list)
            => list.Select(c => c.ToDto());
    }
}
