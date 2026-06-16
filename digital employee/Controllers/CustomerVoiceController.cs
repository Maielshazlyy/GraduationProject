using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service_layer.DTOS.Chat;
using Service_layer.DTOS.Voice;
using Service_layer.Services_Interfaces;

namespace digital_employee.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerVoiceController : ControllerBase
    {
        private readonly ICustomerVoiceService _customerVoiceService;

        public CustomerVoiceController(ICustomerVoiceService customerVoiceService)
        {
            _customerVoiceService = customerVoiceService;
        }

        // ── 1. Start Call ─────────────────────────────────────────────────────
        // Frontend calls this when the customer clicks "Call".
        // Backend creates an Interaction, notifies AI, and returns the meeting URL.
        //
        // POST api/CustomerVoice/call/start
        [HttpPost("call/start")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(StartVoiceCallResponseDTO), 200)]
        public async Task<IActionResult> StartCall([FromBody] StartVoiceCallRequestDTO request)
        {
            if (string.IsNullOrWhiteSpace(request.BusinessId))
                return BadRequest(new { Message = "BusinessId is required." });

            if (string.IsNullOrWhiteSpace(request.CustomerId))
                return BadRequest(new { Message = "CustomerId is required." });

            try
            {
                var result = await _customerVoiceService.StartCallAsync(request);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Failed to start voice call.", Error = ex.Message });
            }
        }

    }

    // ── 5. Call Completed (AI → Backend) ──────────────────────────────────────
    // Separate controller (no auth) — the AI posts here after every call.
    //
    // POST api/voice/call-completed
    [Route("api/voice")]
    [ApiController]
    public class VoiceWebhookController : ControllerBase
    {
        private readonly ICustomerVoiceService _customerVoiceService;

        public VoiceWebhookController(ICustomerVoiceService customerVoiceService)
        {
            _customerVoiceService = customerVoiceService;
        }

        [HttpPost("call-completed")]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        public async Task<IActionResult> CallCompleted([FromBody] VoiceCallCompletedDTO payload)
        {
            if (string.IsNullOrWhiteSpace(payload.CallData?.CallId))
                return BadRequest(new { Message = "call_data.call_id is required." });

            try
            {
                await _customerVoiceService.HandleCallCompletedAsync(payload);
                return Ok(new { Message = "Call summary stored." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Failed to store call summary.", Error = ex.Message });
            }
        }
    }
}
