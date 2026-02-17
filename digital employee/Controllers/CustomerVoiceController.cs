using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Service_layer.DTOS.Chat;
using Service_layer.Services_Interfaces;

namespace digital_employee.Controllers
{
    /// <summary>
    /// Customer-facing voice call API (Voice channel).
    /// Handles audio messages, speech-to-text, text-to-speech, call sessions,
    /// delivery delays, recovery, and feedback collection.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerVoiceController : ControllerBase
    {
        private readonly ICustomerVoiceService _customerVoiceService;

        public CustomerVoiceController(ICustomerVoiceService customerVoiceService)
        {
            _customerVoiceService = customerVoiceService;
        }

        /// <summary>
        /// Initialize a new voice call session.
        /// Creates Interaction with CallSessionId.
        /// </summary>
        [HttpPost("session/initialize")]
        [ProducesResponseType(typeof(Domain_layer.Models.Interaction), 200)]
        public async Task<IActionResult> InitializeSession([FromBody] InitializeVoiceSessionDTO request)
        {
            if (string.IsNullOrWhiteSpace(request.BusinessId))
                return BadRequest(new { Message = "BusinessId is required." });

            if (string.IsNullOrWhiteSpace(request.CallSessionId))
                return BadRequest(new { Message = "CallSessionId is required." });

            try
            {
                var interaction = await _customerVoiceService.InitializeVoiceSessionAsync(
                    request.BusinessId,
                    request.CustomerId,
                    request.CallSessionId);
                return Ok(interaction);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Failed to initialize voice session.", Error = ex.Message });
            }
        }

        /// <summary>
        /// Send a voice message from a customer (Voice channel).
        /// Accepts audio data and returns text reply + optional audio reply.
        /// </summary>
        [HttpPost("message")]
        [ProducesResponseType(typeof(CustomerChatResponseDTO), 200)]
        public async Task<IActionResult> SendVoiceMessage([FromBody] CustomerChatRequestDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (string.IsNullOrWhiteSpace(request.BusinessId))
                return BadRequest(new { Message = "BusinessId is required." });

            if (string.IsNullOrWhiteSpace(request.AudioData) && string.IsNullOrWhiteSpace(request.Message))
                return BadRequest(new { Message = "AudioData or Message is required for Voice." });

            // Force channel to Voice
            request.Channel = "Voice";

            try
            {
                var response = await _customerVoiceService.HandleVoiceMessageAsync(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Voice handling failed.", Error = ex.Message });
            }
        }

        /// <summary>
        /// Mark an interaction as interrupted (call dropped).
        /// Used for recovery and follow-up.
        /// </summary>
        [HttpPost("interaction/{interactionId}/interrupt")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> MarkInterrupted(string interactionId)
        {
            if (string.IsNullOrWhiteSpace(interactionId))
                return BadRequest(new { Message = "InteractionId is required." });

            try
            {
                await _customerVoiceService.MarkInteractionInterruptedAsync(interactionId);
                return Ok(new { Message = "Interaction marked as interrupted." });
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Failed to mark interaction as interrupted.", Error = ex.Message });
            }
        }

        /// <summary>
        /// Submit feedback at end of voice call.
        /// </summary>
        [HttpPost("feedback")]
        [ProducesResponseType(typeof(Domain_layer.Models.Feedback), 200)]
        public async Task<IActionResult> SubmitFeedback([FromBody] VoiceFeedbackDTO feedbackDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (string.IsNullOrWhiteSpace(feedbackDto.InteractionId))
                return BadRequest(new { Message = "InteractionId is required." });

            if (feedbackDto.Rating < 1 || feedbackDto.Rating > 5)
                return BadRequest(new { Message = "Rating must be between 1 and 5." });

            try
            {
                var feedback = await _customerVoiceService.SubmitFeedbackAsync(feedbackDto);
                return Ok(feedback);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Failed to submit feedback.", Error = ex.Message });
            }
        }

        /// <summary>
        /// Get voice settings for a business (for TTS configuration).
        /// </summary>
        [HttpGet("settings/{businessId}")]
        [ProducesResponseType(typeof(VoiceSettingsDTO), 200)]
        public async Task<IActionResult> GetVoiceSettings(string businessId)
        {
            if (string.IsNullOrWhiteSpace(businessId))
                return BadRequest(new { Message = "BusinessId is required." });

            try
            {
                var settings = await _customerVoiceService.GetVoiceSettingsAsync(businessId);
                if (settings == null)
                    return NotFound(new { Message = "Voice settings not found for this business." });
                return Ok(settings);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Failed to get voice settings.", Error = ex.Message });
            }
        }
    }

    /// <summary>
    /// DTO for initializing a voice session.
    /// </summary>
    public class InitializeVoiceSessionDTO
    {
        public string BusinessId { get; set; } = string.Empty;
        public string? CustomerId { get; set; }
        public string CallSessionId { get; set; } = string.Empty;
    }
}

