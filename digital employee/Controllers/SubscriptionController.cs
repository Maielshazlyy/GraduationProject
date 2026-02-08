using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service_layer.DTOS.Subscription;
using Service_layer.Mapping;
using Service_layer.Services_Interfaces;

namespace digital_employee.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SubscriptionController : ControllerBase
    {
        private readonly ISubscriptionService _subscriptionService;

        public SubscriptionController(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        // GET: api/Subscription
        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> GetAll()
        {
            var subscriptions = await _subscriptionService.GetAllAsync();
            return Ok(subscriptions.ToDtoList());
        }

        // GET: api/Subscription/business/{businessId}
        [HttpGet("business/{businessId}")]
        [Authorize(Policy = "OwnerOrAdmin")]
        public async Task<IActionResult> GetByBusinessId(string businessId)
        {
            var subscriptions = await _subscriptionService.GetByBusinessIdAsync(businessId);
            return Ok(subscriptions.ToDtoList());
        }

        // GET: api/Subscription/business/{businessId}/active
        [HttpGet("business/{businessId}/active")]
        [Authorize(Policy = "OwnerOrAdmin")]
        public async Task<IActionResult> GetActiveSubscription(string businessId)
        {
            var subscription = await _subscriptionService.GetActiveSubscriptionAsync(businessId);
            if (subscription == null)
                return NotFound(new { Message = $"No active subscription found for business '{businessId}'." });

            return Ok(subscription.ToDto());
        }

        // GET: api/Subscription/{id}
        [HttpGet("{id}")]
        [Authorize(Policy = "OwnerOrAdmin")]
        public async Task<IActionResult> GetById(string id)
        {
            var subscription = await _subscriptionService.GetByIdAsync(id);
            if (subscription == null)
                return NotFound(new { Message = $"Subscription with id '{id}' not found." });

            return Ok(subscription.ToDto());
        }

        // POST: api/Subscription
        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Create([FromBody] SubscriptionCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var subscription = await _subscriptionService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = subscription.Id }, subscription.ToDto());
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // POST: api/Subscription/{id}/renew
        [HttpPost("{id}/renew")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Renew(string id, [FromBody] SubscriptionRenewDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var subscription = await _subscriptionService.RenewAsync(id, dto);
            if (subscription == null)
                return NotFound(new { Message = $"Subscription with id '{id}' not found." });

            return Ok(subscription.ToDto());
        }

        // DELETE: api/Subscription/{id}
        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Delete(string id)
        {
            var deleted = await _subscriptionService.DeleteAsync(id);
            if (!deleted)
                return NotFound(new { Message = $"Subscription with id '{id}' not found." });

            return NoContent();
        }
    }
}

