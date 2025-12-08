using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain_layer.Models;
using Service_layer.DTOS.Integration;

namespace Service_layer.Mapping
{
    public static class IntegrationMapping
    {
        public static IntegrationResponseDTO ToDto(this Integration i)
        {
            return new IntegrationResponseDTO
            {
                Id = i.Id,
                IntegrationId = i.IntegrationId,
                PlatformName = i.PlatformName,
                Status = i.Status.ToString(),
                LastSyncDate = i.LastSyncDate,

                BusinessId = i.BusinessId,
                BusinessName = i.Business?.Name ?? ""
            };
        }

        public static IEnumerable<IntegrationResponseDTO> ToDtoList(this IEnumerable<Integration> list)
            => list.Select(i => i.ToDto());
    }
}
