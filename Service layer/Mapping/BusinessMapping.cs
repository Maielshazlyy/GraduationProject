using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain_layer.Models;
using Service_layer.DTOS.Business;

namespace Service_layer.Mapping
{
    public static class BusinessMapping
    {
        public static BusinessResponseDTO ToDto(this Business b)
        {
            return new BusinessResponseDTO
            {
                Id = b.Id,
                BusinessId = b.BusinessId,
                Name = b.Name,
                Type = b.Type,
                Address = b.Address,
                Phone = b.Phone,
                CreatedAt = b.CreatedAt,

                TotalUsers = b.Users?.Count ?? 0,
                TotalCustomers = b.Customers?.Count ?? 0,
                TotalTickets = b.Tickets?.Count ?? 0
                
            };
        }
        public static IEnumerable<BusinessResponseDTO> ToDtoList(this IEnumerable<Business> list)
       => list.Select(b => b.ToDto());
    }
}