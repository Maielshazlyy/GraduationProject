using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service_layer.DTOS.Notification;
using Service_layer.Mapping;
using Service_layer.Services_Interfaces;

namespace digital_employee.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        // GET: api/Notification
        [HttpGet]
        [Authorize(Policy = "AgentOrOwnerOrAdmin")]
        public async Task<IActionResult> GetAll()
        {
            var notifications = await _notificationService.GetAllAsync();
            return Ok(notifications.ToDtoList());
        }

        // GET: api/Notification/business/{businessId}
        [HttpGet("business/{businessId}")]
        [Authorize(Policy = "AgentOrOwnerOrAdmin")]
        public async Task<IActionResult> GetByBusinessId(string businessId)
        {
            var notifications = await _notificationService.GetByBusinessIdAsync(businessId);
            return Ok(notifications.ToDtoList());
        }

        // GET: api/Notification/user/{userId}
        [HttpGet("user/{userId}")]
        [Authorize(Policy = "AgentOrOwnerOrAdmin")]
        public async Task<IActionResult> GetByUserId(string userId)
        {
            var notifications = await _notificationService.GetByUserIdAsync(userId);
            return Ok(notifications.ToDtoList());
        }

        // GET: api/Notification/{id}
        [HttpGet("{id}")]
        [Authorize(Policy = "AgentOrOwnerOrAdmin")]
        public async Task<IActionResult> GetById(string id)
        {
            var notification = await _notificationService.GetByIdAsync(id);
            if (notification == null)
                return NotFound(new { Message = $"Notification with id '{id}' not found." });

            return Ok(notification.ToDto());
        }

        // POST: api/Notification
        [HttpPost]
        [Authorize(Policy = "OwnerOrAdmin")]
        public async Task<IActionResult> Create([FromBody] NotificationCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var notification = await _notificationService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = notification.NotificationId }, notification.ToDto());
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // PUT: api/Notification/{id}/read
        [HttpPut("{id}/read")]
        [Authorize(Policy = "AgentOrOwnerOrAdmin")]
        public async Task<IActionResult> MarkAsRead(string id)
        {
            var notification = await _notificationService.MarkAsReadAsync(id);
            if (notification == null)
                return NotFound(new { Message = $"Notification with id '{id}' not found." });

            return Ok(notification.ToDto());
        }

        // DELETE: api/Notification/{id}
        [HttpDelete("{id}")]
        [Authorize(Policy = "OwnerOrAdmin")]
        public async Task<IActionResult> Delete(string id)
        {
            var deleted = await _notificationService.DeleteAsync(id);
            if (!deleted)
                return NotFound(new { Message = $"Notification with id '{id}' not found." });

            return NoContent();
        }
    }
}

