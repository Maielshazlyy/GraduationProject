using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_layer.DTOS.Auth
{
    public class RegisterDTO
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        /// <summary>Target business for the new agent account (<c>POST /api/Auth/register</c>). Must be non-empty (validated by FluentValidation).</summary>
        /// <remarks>Use <c>string?</c> so ASP.NET Core does not apply implicit &quot;required&quot; before FluentValidation runs.</remarks>
        public string? BusinessId { get; set; }
    }
}
