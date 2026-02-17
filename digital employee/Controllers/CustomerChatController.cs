using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Service_layer.DTOS.Chat;
using Service_layer.Services_Interfaces;

namespace digital_employee.Controllers
{
    /// <summary>
    /// Customer-facing text chat API (WebChat channel).
    /// This controller is typically called from the website/mobile widget.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerChatController : ControllerBase
    {
        private readonly ICustomerChatService _customerChatService;

        public CustomerChatController(ICustomerChatService customerChatService)
        {
            _customerChatService = customerChatService;
        }

        /// <summary>
        /// Get available communication capabilities (Chat, Voice) for a business.
        /// </summary>
        [HttpGet("capabilities/{businessId}")]
        [ProducesResponseType(typeof(CustomerCapabilitiesDTO), 200)]
        public async Task<IActionResult> GetCapabilities(string businessId)
        {
            if (string.IsNullOrWhiteSpace(businessId))
                return BadRequest(new { Message = "BusinessId is required." });

            try
            {
                var capabilities = await _customerChatService.GetCapabilitiesAsync(businessId);
                return Ok(capabilities);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Failed to get capabilities.", Error = ex.Message });
            }
        }

        /// <summary>
        /// Send a text message from a customer (Chat channel).
        /// </summary>
        [HttpPost("message")]
        [ProducesResponseType(typeof(CustomerChatResponseDTO), 200)]
        public async Task<IActionResult> SendMessage([FromBody] CustomerChatRequestDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (string.IsNullOrWhiteSpace(request.BusinessId))
                return BadRequest(new { Message = "BusinessId is required." });

            if (string.IsNullOrWhiteSpace(request.Message))
                return BadRequest(new { Message = "Message is required for Chat." });

            // Ensure channel is WebChat
            request.Channel = "WebChat";

            try
            {
                var response = await _customerChatService.HandleMessageAsync(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Chat handling failed.", Error = ex.Message });
            }
        }

        /// <summary>
        /// Get backend-driven recommendations for a main menu item
        /// (e.g., suggest fries and drink with burger) for the public order page.
        /// </summary>
        [HttpPost("recommendations")]
        [ProducesResponseType(typeof(System.Collections.Generic.List<RecommendationItemDTO>), 200)]
        public async Task<IActionResult> GetOrderRecommendations(
            [FromBody] CustomerOrderRecommendationRequestDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (string.IsNullOrWhiteSpace(request.BusinessId))
                return BadRequest(new { Message = "BusinessId is required." });

            if (string.IsNullOrWhiteSpace(request.MainMenuItemId))
                return BadRequest(new { Message = "MainMenuItemId is required." });

            try
            {
                var recs = await _customerChatService.GetOrderRecommendationsAsync(request);
                return Ok(recs);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Failed to get recommendations.", Error = ex.Message });
            }
        }
    }
}


