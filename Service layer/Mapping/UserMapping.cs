using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain_layer.Models;
using Service_layer.DTOS.User;

namespace Service_layer.Mapping
{
    public static class UserMapping
    {
        public static UserResponseDTO ToDto(this User u)
        {
            return new UserResponseDTO
            {
                UserId = u.UserId,
                FullName = u.FullName,
                Email = u.Email,
                Role = u.Role,
                CreatedAt = u.CreatedAt,

                BusinessId = u.BusinessId,
                BusinessName = u.Business?.Name ?? "",

                TotalHandledInteractions = u.InteractionsHandled?.Count ?? 0,
                TotalAssignedTickets = u.TicketsAssigned?.Count ?? 0
            };
        }

        public static IEnumerable<UserResponseDTO> ToDtoList(this IEnumerable<User> list)
            => list.Select(u => u.ToDto());
    }
}
