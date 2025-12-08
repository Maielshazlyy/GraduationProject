using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain_layer.Models;
using Service_layer.DTOS.Notification;

namespace Service_layer.Mapping
{
    public static class NotificationMapping
    {
        public static NotificationResponseDTO ToDto(this Notification n)
        {
            return new NotificationResponseDTO
            {
                NotificationId = n.NotificationId,
                Title = n.Title,
                Message = n.Message,
                IsRead = n.IsRead,
                CreatedAt = n.CreatedAt,

                UserId = n.UserId,
                UserName = n.User?.FullName ?? "",

                BusinessId = n.BusinessId,
                BusinessName = n.Business?.Name ?? ""
            };
        }

        public static IEnumerable<NotificationResponseDTO> ToDtoList(this IEnumerable<Notification> list)
            => list.Select(n => n.ToDto());
    }
}
