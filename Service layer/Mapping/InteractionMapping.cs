using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain_layer.Models;
using Service_layer.DTOS.Interaction;

namespace Service_layer.Mapping
{
    public static class InteractionMapping
    {
        public static InteractionResponseDTO ToDto(this Interaction i)
        {
            return new InteractionResponseDTO
            {
                InteractionId = i.InteractionId,
                Channel = i.Channel,
                Status = i.Status,
                IsEnded = i.IsEnded,
                StartedAt = i.StartedAt,
                EndedAt = i.EndedAt,

                BusinessId = i.BusinessId,
                BusinessName = i.Business?.Name ?? "",

                CustomerId = i.CustomerId,
                CustomerName = i.Customer?.FullName ?? "",

                HandledByUserId = i.HandledByUserId,
                AgentName = i.HandledByUser?.FullName ?? "",

                MessageCount = i.Messages?.Count ?? 0
            };
        }
        public static IEnumerable<InteractionResponseDTO> ToDtoList(this IEnumerable<Interaction> list)
       => list.Select(t => t.ToDto());
    }
}
