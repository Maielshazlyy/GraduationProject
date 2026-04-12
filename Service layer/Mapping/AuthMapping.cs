using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain_layer.Constants;
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
                UserName = dto.Email,
                Email = dto.Email,
                FullName = dto.FullName,
                BusinessId = dto.BusinessId,
                Role = Roles.Agent,
                CreatedAt = DateTime.UtcNow
            };
        }

        public static User ToEntity(this RegisterBootstrapDTO dto)
        {
            return new User
            {
                UserName = dto.Email,
                Email = dto.Email,
                FullName = dto.FullName,
                BusinessId = null,
                Role = Roles.Agent,
                CreatedAt = DateTime.UtcNow
            };
        }
    }
}
