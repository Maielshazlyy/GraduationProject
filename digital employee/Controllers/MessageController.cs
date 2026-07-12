using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service_layer.DTOS.Message;
using Service_layer.Mapping;
using Service_layer.Services_Interfaces;

namespace digital_employee.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        // GET: api/Message
        [HttpGet]
        [Authorize(Policy = "AgentOrOwnerOrAdmin")]
        public async Task<IActionResult> GetAll()
        {
            var messages = await _messageService.GetAllAsync();
            return Ok(messages.ToDtoList());
        }

        // GET: api/Message/interaction/{interactionId}
        [HttpGet("interaction/{interactionId}")]
        [Authorize(Policy = "AgentOrOwnerOrAdmin")]
        public async Task<IActionResult> GetByInteractionId(string interactionId)
        {
            var messages = await _messageService.GetByInteractionIdAsync(interactionId);
            return Ok(messages.ToDtoList());
        }

        // GET: api/Message/{id}
        [HttpGet("{id}")]
        [Authorize(Policy = "AgentOrOwnerOrAdmin")]
        public async Task<IActionResult> GetById(string id)
        {
            var message = await _messageService.GetByIdAsync(id);
            if (message == null)
                return NotFound(new { Message = $"Message with id '{id}' not found." });

            return Ok(message.ToDto());
        }

        // POST: api/Message
        [HttpPost]
        [Authorize(Policy = "AgentOrOwnerOrAdmin")]
        public async Task<IActionResult> Create([FromBody] MessageCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var message = await _messageService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = message.MessageId }, message.ToDto());
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // DELETE: api/Message/{id}
        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Delete(string id)
        {
            var deleted = await _messageService.DeleteAsync(id);
            if (!deleted)
                return NotFound(new { Message = $"Message with id '{id}' not found." });

            return NoContent();
        }
    }
}

