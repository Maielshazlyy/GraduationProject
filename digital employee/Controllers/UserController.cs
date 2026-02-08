using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service_layer.DTOS.User;
using Service_layer.Mapping;
using Service_layer.Services_Interfaces;

namespace digital_employee.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/User
        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users.ToDtoList());
        }

        // GET: api/User/business/{businessId}
        [HttpGet("business/{businessId}")]
        [Authorize(Policy = "OwnerOrAdmin")]
        public async Task<IActionResult> GetByBusinessId(string businessId)
        {
            var users = await _userService.GetByBusinessIdAsync(businessId);
            return Ok(users.ToDtoList());
        }

        // GET: api/User/{id}
        [HttpGet("{id}")]
        [Authorize(Policy = "AgentOrOwnerOrAdmin")]
        public async Task<IActionResult> GetById(string id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
                return NotFound(new { Message = $"User with id '{id}' not found." });

            return Ok(user.ToDto());
        }

        // GET: api/User/email/{email}
        [HttpGet("email/{email}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> GetByEmail(string email)
        {
            var user = await _userService.GetByEmailAsync(email);
            if (user == null)
                return NotFound(new { Message = $"User with email '{email}' not found." });

            return Ok(user.ToDto());
        }

        // PUT: api/User/{id}
        [HttpPut("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Update(string id, [FromBody] UserUpdateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var user = await _userService.UpdateAsync(id, dto);
                if (user == null)
                    return NotFound(new { Message = $"User with id '{id}' not found." });

                return Ok(user.ToDto());
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // POST: api/User/{id}/assign-role
        [HttpPost("{id}/assign-role")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> AssignRole(string id, [FromBody] AssignRoleDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != dto.UserId)
                return BadRequest(new { Message = "User ID in URL does not match User ID in body." });

            try
            {
                var result = await _userService.AssignRoleAsync(dto.UserId, dto.NewRole);
                if (!result)
                    return NotFound(new { Message = $"User with id '{dto.UserId}' not found." });

                return Ok(new { Message = $"User role has been updated to '{dto.NewRole}'. Please login again to get a new token." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // DELETE: api/User/{id}
        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Delete(string id)
        {
            var deleted = await _userService.DeleteAsync(id);
            if (!deleted)
                return NotFound(new { Message = $"User with id '{id}' not found." });

            return NoContent();
        }
    }
}

