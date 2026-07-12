using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service_layer.DTOS.Interaction;
using Service_layer.Mapping;
using Service_layer.Services_Interfaces;

namespace digital_employee.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class InteractionController : ControllerBase
    {
        private readonly IInteractionService _interactionService;

        public InteractionController(IInteractionService interactionService)
        {
            _interactionService = interactionService;
        }

        // GET: api/Interaction
        [HttpGet]
        [Authorize(Policy = "AgentOrOwnerOrAdmin")]
        public async Task<IActionResult> GetAll()
        {
            var interactions = await _interactionService.GetAllAsync();
            return Ok(interactions.ToDtoList());
        }

        // GET: api/Interaction/business/{businessId}
        [HttpGet("business/{businessId}")]
        [Authorize(Policy = "AgentOrOwnerOrAdmin")]
        public async Task<IActionResult> GetByBusinessId(string businessId)
        {
            var interactions = await _interactionService.GetByBusinessIdAsync(businessId);
            return Ok(interactions.ToDtoList());
        }

        // GET: api/Interaction/customer/{customerId}
        [HttpGet("customer/{customerId}")]
        [Authorize(Policy = "AgentOrOwnerOrAdmin")]
        public async Task<IActionResult> GetByCustomerId(string customerId)
        {
            var interactions = await _interactionService.GetByCustomerIdAsync(customerId);
            return Ok(interactions.ToDtoList());
        }

        // GET: api/Interaction/user/{userId}
        [HttpGet("user/{userId}")]
        [Authorize(Policy = "AgentOrOwnerOrAdmin")]
        public async Task<IActionResult> GetByUserId(string userId)
        {
            var interactions = await _interactionService.GetByUserIdAsync(userId);
            return Ok(interactions.ToDtoList());
        }

        // GET: api/Interaction/{id}
        [HttpGet("{id}")]
        [Authorize(Policy = "AgentOrOwnerOrAdmin")]
        public async Task<IActionResult> GetById(string id)
        {
            var interaction = await _interactionService.GetByIdAsync(id);
            if (interaction == null)
                return NotFound(new { Message = $"Interaction with id '{id}' not found." });

            return Ok(interaction.ToDto());
        }

        // POST: api/Interaction/start
        [HttpPost("start")]
        [Authorize(Policy = "AgentOrOwnerOrAdmin")]
        public async Task<IActionResult> StartInteraction([FromBody] StartInteractionDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var interaction = await _interactionService.StartInteractionAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = interaction.InteractionId }, interaction.ToDto());
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // POST: api/Interaction/{id}/end
        [HttpPost("{id}/end")]
        [Authorize(Policy = "AgentOrOwnerOrAdmin")]
        public async Task<IActionResult> EndInteraction(string id, [FromBody] EndInteractionDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var interaction = await _interactionService.EndInteractionAsync(id, dto);
                if (interaction == null)
                    return NotFound(new { Message = $"Interaction with id '{id}' not found." });

                return Ok(interaction.ToDto());
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // DELETE: api/Interaction/{id}
        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Delete(string id)
        {
            var deleted = await _interactionService.DeleteAsync(id);
            if (!deleted)
                return NotFound(new { Message = $"Interaction with id '{id}' not found." });

            return NoContent();
        }
    }
}

