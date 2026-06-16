using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service_layer.DTOS.AiOwnerChat;
using Service_layer.Services_Interfaces;

namespace digital_employee.Controllers
{
    /// <summary>
    /// AI analytics assistant for the owner dashboard.
    /// Powered by IRIS /api/v1/owner/* endpoints.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "OwnerOrAdmin")]
    public class OwnerChatController : ControllerBase
    {
        private readonly IAiOwnerChatService _aiOwnerChatService;

        public OwnerChatController(IAiOwnerChatService aiOwnerChatService)
        {
            _aiOwnerChatService = aiOwnerChatService;
        }

        /// <summary>
        /// Send an analytics question to the AI owner assistant.
        /// </summary>
        [HttpPost("message")]
        [ProducesResponseType(typeof(AiOwnerChatResponseDTO), 200)]
        public async Task<IActionResult> SendMessage([FromBody] OwnerChatMessageDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Message))
                return BadRequest(new { Message = "Message is required." });

            var businessId = User.FindFirstValue("BusinessId") ?? "unknown";
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "unknown";

            try
            {
                var request = new AiOwnerChatRequestDTO
                {
                    SessionId = $"owner-{businessId}-{userId}",
                    Message = dto.Message
                };

                var result = await _aiOwnerChatService.SendMessageAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Owner chat failed.", Error = ex.Message });
            }
        }

        /// <summary>
        /// Get the AI-generated analytics report for this business.
        /// </summary>
        [HttpGet("report")]
        [ProducesResponseType(typeof(AiOwnerReportResponseDTO), 200)]
        public async Task<IActionResult> GetReport()
        {
            try
            {
                var result = await _aiOwnerChatService.GetReportAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Failed to get report.", Error = ex.Message });
            }
        }

        /// <summary>
        /// Trigger IRIS to reload its analytics data.
        /// Call this after significant data changes.
        /// </summary>
        [HttpPost("reload")]
        [ProducesResponseType(typeof(AiOwnerReloadResponseDTO), 200)]
        public async Task<IActionResult> Reload()
        {
            try
            {
                var result = await _aiOwnerChatService.ReloadAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Failed to reload analytics.", Error = ex.Message });
            }
        }
    }

    public class OwnerChatMessageDTO
    {
        public string Message { get; set; } = string.Empty;
    }
}
