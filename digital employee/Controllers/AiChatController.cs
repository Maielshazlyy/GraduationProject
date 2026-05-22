using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Service_layer.DTOS.AiChat;
using Service_layer.Services_Interfaces;

namespace digital_employee.Controllers
{
    /// <summary>
    /// Proxy endpoint that forwards customer chat messages to the external AI API.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AiChatController : ControllerBase
    {
        private readonly IAiChatService _aiChatService;

        public AiChatController(IAiChatService aiChatService)
        {
            _aiChatService = aiChatService;
        }

        /// <summary>
        /// Send a chat message to the AI and return its reply.
        /// </summary>
        [HttpPost("chat")]
        [ProducesResponseType(typeof(AiChatResponseDTO), 200)]
        public async Task<IActionResult> Chat([FromBody] AiChatRequestDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var response = await _aiChatService.SendMessageAsync(request);
                return Ok(response);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(502, new { Message = "AI service unavailable.", Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Failed to process chat.", Error = ex.Message });
            }
        }
    }
}
