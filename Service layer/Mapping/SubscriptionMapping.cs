using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain_layer.Models;
using Service_layer.DTOS.Subscription;

namespace Service_layer.Mapping
{
    public static class SubscriptionMapping
    {
        public static SubscriptionResponseDTO ToDto(this Subscription s)
        {
            return new SubscriptionResponseDTO
            {
                Id = s.Id,
                SubscriptionId = s.SubscriptionId,
                PlanName = s.PlanName,
                Price = s.Price,
                StartDate = s.StartDate,
                EndDate = s.EndDate,
                IsActive = s.IsActive,

                BusinessId = s.BusinessId,
                BusinessName = s.Business?.Name ?? ""
            };
        }

        public static IEnumerable<SubscriptionResponseDTO> ToDtoList(this IEnumerable<Subscription> list)
            => list.Select(s => s.ToDto());
    }
}
