using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain_layer.Models;
using Service_layer.DTOS.Order;

namespace Service_layer.Mapping
{
    public static class OrderMapping
    {
        public static OrderResponseDTO ToDto(this Order o)
        {
            return new OrderResponseDTO
            {
                OrderId = o.OrderId,
                TotalPrice = o.TotalPrice,
                Status = o.Status.ToString(),
                CreatedAt = o.CreatedAt,

                BusinessId = o.BusinessId,
                BusinessName = o.Business?.Name ?? string.Empty,

                CustomerId = o.CustomerId,
                CustomerName = o.Customer?.FullName ?? string.Empty,

                Items = o.OrderItems?
                           .Select(i => i.ToDetailDto())   // <= هنا بنادي الـ extension
                           .ToList()
                        ?? new List<OrderItemDetailDTO>()
            };
        }
    }
}
