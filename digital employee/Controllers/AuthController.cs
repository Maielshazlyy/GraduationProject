using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Service_layer.DTOS.Auth;
using Service_layer.ServicesInterfaces;

namespace digital_employee.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController:ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // POST: api/Auth/register
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterDTO model)
        {
            // FluentValidation بيشتغل أوتوماتيك هنا ويرجع 400 لو الداتا ناقصة

            try
            {
                var result = await _authService.RegisterAsync(model);
                return Ok(result); // 200 OK + Token
            }
            catch (Exception ex)
            {
                // لو حصل خطأ (زي إيميل مكرر، أو BusinessId غلط)
                return BadRequest(new { Message = ex.Message });
            }
        }

        // POST: api/Auth/login
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {
            try
            {
                var result = await _authService.LoginAsync(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
        [HttpPost("google-login")]
        [AllowAnonymous]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginDTO model)
        {
            try
            {
                var result = await _authService.GoogleLoginAsync(model.IdToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}

