using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service_layer.DTOS.Integration;
using Service_layer.Mapping;
using Service_layer.Services_Interfaces;

namespace digital_employee.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class IntegrationController : ControllerBase
    {
        private readonly IIntegrationService _integrationService;

        public IntegrationController(IIntegrationService integrationService)
        {
            _integrationService = integrationService;
        }

        // GET: api/Integration
        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> GetAll()
        {
            var integrations = await _integrationService.GetAllAsync();
            return Ok(integrations.ToDtoList());
        }

        // GET: api/Integration/business/{businessId}
        [HttpGet("business/{businessId}")]
        [Authorize(Policy = "OwnerOrAdmin")]
        public async Task<IActionResult> GetByBusinessId(string businessId)
        {
            var integrations = await _integrationService.GetByBusinessIdAsync(businessId);
            return Ok(integrations.ToDtoList());
        }

        // GET: api/Integration/{id}
        [HttpGet("{id}")]
        [Authorize(Policy = "OwnerOrAdmin")]
        public async Task<IActionResult> GetById(string id)
        {
            var integration = await _integrationService.GetByIdAsync(id);
            if (integration == null)
                return NotFound(new { Message = $"Integration with id '{id}' not found." });

            return Ok(integration.ToDto());
        }

        // POST: api/Integration/connect
        [HttpPost("connect")]
        [Authorize(Policy = "OwnerOrAdmin")]
        public async Task<IActionResult> Connect([FromBody] IntegrationConnectDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var integration = await _integrationService.ConnectAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = integration.Id }, integration.ToDto());
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // POST: api/Integration/{id}/sync
        [HttpPost("{id}/sync")]
        [Authorize(Policy = "OwnerOrAdmin")]
        public async Task<IActionResult> Sync(string id, [FromBody] IntegrationSyncDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var integration = await _integrationService.SyncAsync(id, dto);
            if (integration == null)
                return NotFound(new { Message = $"Integration with id '{id}' not found." });

            return Ok(integration.ToDto());
        }

        // DELETE: api/Integration/{id}
        [HttpDelete("{id}")]
        [Authorize(Policy = "OwnerOrAdmin")]
        public async Task<IActionResult> Delete(string id)
        {
            var deleted = await _integrationService.DeleteAsync(id);
            if (!deleted)
                return NotFound(new { Message = $"Integration with id '{id}' not found." });

            return NoContent();
        }
    }
}

