using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain_layer.Models;
using Service_layer.DTOS.Feedback;

namespace Service_layer.Mapping
{
    public static class FeedbackMapping
    {
        public static FeedbackResponseDTO ToDto(this Feedback f)
        {
            return new FeedbackResponseDTO
            {
                FeedbackId = f.FeedbackId,
                Rating = f.Rating,
                Comment = f.Comment,
                CreatedAt = f.CreatedAt,

                CustomerId = f.CustomerId,
                CustomerName = f.Customer?.FullName ?? "",

                TicketId = f.TicketId,
                TickerSubject = f.Ticket?.Subject ?? ""
            };
        }

        public static IEnumerable<FeedbackResponseDTO> ToDtoList(this IEnumerable<Feedback> list)
            => list.Select(f => f.ToDto());
    }
}
