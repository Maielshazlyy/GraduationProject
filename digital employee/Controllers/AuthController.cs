using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Domain_layer.Models;
using Domain_layer.Constants;
using Service_layer.DTOS.Auth;
using Service_layer.ServicesInterfaces;

namespace digital_employee.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController:ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly UserManager<User> _userManager;

        public AuthController(IAuthService authService, UserManager<User> userManager)
        {
            _authService = authService;
            _userManager = userManager;
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

        // POST: api/Auth/promote-to-owner
        [HttpPost("promote-to-owner")]
        [Authorize(Policy = "AdminOnly")] // فقط Admin يمكنه ترقية المستخدمين
        public async Task<IActionResult> PromoteToOwner([FromBody] string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    return NotFound(new { Message = "User not found." });

                user.Role = Roles.Owner;
                var result = await _userManager.UpdateAsync(user);

                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return BadRequest(new { Message = errors });
                }

                return Ok(new { Message = $"User {user.Email} has been promoted to Owner. Please login again to get a new token." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // POST: api/Auth/promote-to-admin
        [HttpPost("promote-to-admin")]
        [Authorize(Policy = "AdminOnly")] // فقط Admin يمكنه ترقية المستخدمين إلى Admin
        public async Task<IActionResult> PromoteToAdmin([FromBody] string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    return NotFound(new { Message = "User not found." });

                user.Role = Roles.Admin;
                var result = await _userManager.UpdateAsync(user);

                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return BadRequest(new { Message = errors });
                }

                return Ok(new { Message = $"User {user.Email} has been promoted to Admin. Please login again to get a new token." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}

