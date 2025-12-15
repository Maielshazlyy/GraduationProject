using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service_layer.DTOS.Auth;

namespace Service_layer.ServicesInterfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDTO> RegisterAsync(RegisterDTO model);
        Task<AuthResponseDTO> LoginAsync(LoginDTO model);
        Task<AuthResponseDTO> GoogleLoginAsync(string googleToken);
    }
}
