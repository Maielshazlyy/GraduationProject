using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service_layer.DTOS.AuditLog;
using Service_layer.Mapping;
using Service_layer.Services_Interfaces;

namespace digital_employee.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuditLogController : ControllerBase
    {
        private readonly IAuditLogService _auditLogService;

        public AuditLogController(IAuditLogService auditLogService)
        {
            _auditLogService = auditLogService;
        }

        // GET: api/AuditLog
        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> GetAll()
        {
            var auditLogs = await _auditLogService.GetAllAsync();
            return Ok(auditLogs.ToDtoList());
        }

        // GET: api/AuditLog/business/{businessId}
        [HttpGet("business/{businessId}")]
        [Authorize(Policy = "OwnerOrAdmin")]
        public async Task<IActionResult> GetByBusinessId(string businessId)
        {
            var auditLogs = await _auditLogService.GetByBusinessIdAsync(businessId);
            return Ok(auditLogs.ToDtoList());
        }

        // GET: api/AuditLog/user/{userId}
        [HttpGet("user/{userId}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> GetByUserId(string userId)
        {
            var auditLogs = await _auditLogService.GetByUserIdAsync(userId);
            return Ok(auditLogs.ToDtoList());
        }

        // GET: api/AuditLog/{id}
        [HttpGet("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> GetById(string id)
        {
            var auditLog = await _auditLogService.GetByIdAsync(id);
            if (auditLog == null)
                return NotFound(new { Message = $"AuditLog with id '{id}' not found." });

            return Ok(auditLog.ToDto());
        }
    }
}

