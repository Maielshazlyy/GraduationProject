using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain_layer.Models;
using Service_layer.DTOS.Ticket;

namespace Service_layer.Mapping
{
    public static class TicketMapping
    {
        public static TicketResponseDTO ToDto(this Ticket t)
        {
            return new TicketResponseDTO
            {
                Id = t.Id,
                TicketId = t.TicketId,
                Subject = t.Subject,
                Status = t.Status,
                IsEnded = t.IsEnded,
                CreatedAt = t.CreatedAt,
                ClosedAt = t.ClosedAt,

                BusinessId = t.BusinessId,
                BusinessName = t.Business?.Name ?? "",

                CustomerId = t.CustomerId,
                CustomerName = t.Customer?.FullName ?? "",

                AssignedToUserId = t.AssignedToUserId,
                AssignedToUserName = t.AssignedToUser?.FullName ?? "",

                TotalFeedback = t.Feedbacks?.Count ?? 0
            };
        }

        public static IEnumerable<TicketResponseDTO> ToDtoList(this IEnumerable<Ticket> list)
            => list.Select(t => t.ToDto());
    }
}
