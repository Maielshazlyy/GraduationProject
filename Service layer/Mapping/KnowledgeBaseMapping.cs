using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain_layer.Models;
using Service_layer.DTOS.KnowledgeBase;

namespace Service_layer.Mapping
{
    public static class KnowledgeBaseMapping
    {
        public static KnowledgeBaseResponseDTO ToDto(this KnowledgeBase k)
        {
            return new KnowledgeBaseResponseDTO
            {
                KnowledgeBaseId = k.KnowledgeBaseId,
                Question = k.Question,
                Answer = k.Answer,
                CreatedAt = k.CreatedAt,

                BusinessId = k.BusinessId,
                BusinessName = k.Business?.Name ?? "",
                IsFAQ = k.IsFAQ,
                DisplayOrder = k.DisplayOrder,
                IsActive = k.IsActive
            };
        }

        public static IEnumerable<KnowledgeBaseResponseDTO> ToDtoList(this IEnumerable<KnowledgeBase> list)
            => list.Select(k => k.ToDto());
    }
}
