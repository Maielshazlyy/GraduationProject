using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain_layer.Models;
using Service_layer.DTOS.Auth;

namespace Service_layer.Mapping
{
    public static class AuthMapping
    {
        public static User ToEntity(this RegisterDTO dto)
        {
            return new User
            {
                // في Identity، الـ UserName ضروري جداً
                UserName = dto.Email,
                Email = dto.Email,
                FullName = dto.FullName,

                // الـ BusinessId بيجي string من الـ DTO
                BusinessId = dto.BusinessId,

                Role = dto.Role,
                CreatedAt = DateTime.UtcNow
            };
        }
    }
}
