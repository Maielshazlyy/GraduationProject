using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Domain_layer.Interfaces;
using Domain_layer.Models;
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
        private readonly IUnitOfWork _unitOfWork;

        public OwnerChatController(IAiOwnerChatService aiOwnerChatService, IUnitOfWork unitOfWork)
        {
            _aiOwnerChatService = aiOwnerChatService;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Send an analytics question to the AI owner assistant. The exchange (question +
        /// reply) is persisted so the owner's chat history survives across sessions.
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
                    BusinessId = businessId,
                    SessionId = $"owner-{businessId}-{userId}",
                    Message = dto.Message
                };

                var result = await _aiOwnerChatService.SendMessageAsync(request);

                // Best-effort: a history-save failure must never block the reply from reaching
                // the owner.
                try
                {
                    await _unitOfWork.OwnerChatMessages.AddAsync(new OwnerChatMessage
                    {
                        BusinessId = businessId,
                        UserId = userId,
                        Message = dto.Message,
                        Reply = result.Reply,
                        Confidence = result.Confidence
                    });
                    await _unitOfWork.CompleteAsync();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[OwnerChat] Failed to save chat history: {ex.Message}");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Owner chat failed.", Error = ex.Message });
            }
        }

        /// <summary>
        /// Get saved Owner Chat history for the current business, oldest first, so the
        /// frontend can repopulate the conversation when the owner reopens the page.
        /// </summary>
        [HttpGet("history")]
        [ProducesResponseType(typeof(List<OwnerChatHistoryItemDTO>), 200)]
        public async Task<IActionResult> GetHistory()
        {
            var businessId = User.FindFirstValue("BusinessId") ?? "unknown";

            var messages = await _unitOfWork.OwnerChatMessages.GetByBusinessIdAsync(businessId);
            var result = messages.Select(m => new OwnerChatHistoryItemDTO
            {
                Message = m.Message,
                Reply = m.Reply,
                Confidence = m.Confidence,
                SentAt = m.SentAt
            }).ToList();

            return Ok(result);
        }

        /// <summary>
        /// Clear all saved Owner Chat history for the current business.
        /// </summary>
        [HttpDelete("history")]
        public async Task<IActionResult> ClearHistory()
        {
            var businessId = User.FindFirstValue("BusinessId") ?? "unknown";

            await _unitOfWork.OwnerChatMessages.DeleteAllByBusinessIdAsync(businessId);
            return NoContent();
        }
    }

    public class OwnerChatMessageDTO
    {
        public string Message { get; set; } = string.Empty;
    }
}
