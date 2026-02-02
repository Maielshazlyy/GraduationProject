using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service_layer.DTOS.PaymentTranscation;
using Service_layer.Mapping;
using Service_layer.Services_Interfaces;
using PaymentTransactionMapping = Service_layer.Mapping.PaymentTransactionMapping;

namespace digital_employee.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentTransactionController : ControllerBase
    {
        private readonly IPaymentTransactionService _paymentService;

        public PaymentTransactionController(IPaymentTransactionService paymentService)
        {
            _paymentService = paymentService;
        }

        // GET: api/PaymentTransaction
        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> GetAll()
        {
            var payments = await _paymentService.GetAllAsync();
            return Ok(PaymentTransactionMapping.ToDtoList(payments));
        }

        // GET: api/PaymentTransaction/subscription/{subscriptionId}
        [HttpGet("subscription/{subscriptionId}")]
        [Authorize(Policy = "OwnerOrAdmin")]
        public async Task<IActionResult> GetBySubscriptionId(string subscriptionId)
        {
            var payments = await _paymentService.GetBySubscriptionIdAsync(subscriptionId);
            return Ok(PaymentTransactionMapping.ToDtoList(payments));
        }

        // GET: api/PaymentTransaction/business/{businessId}
        [HttpGet("business/{businessId}")]
        [Authorize(Policy = "OwnerOrAdmin")]
        public async Task<IActionResult> GetByBusinessId(string businessId)
        {
            var payments = await _paymentService.GetByBusinessIdAsync(businessId);
            return Ok(PaymentTransactionMapping.ToDtoList(payments));
        }

        // GET: api/PaymentTransaction/{id}
        [HttpGet("{id}")]
        [Authorize(Policy = "OwnerOrAdmin")]
        public async Task<IActionResult> GetById(string id)
        {
            var payment = await _paymentService.GetByIdAsync(id);
            if (payment == null)
                return NotFound(new { Message = $"PaymentTransaction with id '{id}' not found." });

            return Ok(PaymentTransactionMapping.ToDto(payment));
        }

        // POST: api/PaymentTransaction
        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Create([FromBody] PaymentTransactionCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var payment = await _paymentService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = payment.Id }, PaymentTransactionMapping.ToDto(payment));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // DELETE: api/PaymentTransaction/{id}
        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Delete(string id)
        {
            var deleted = await _paymentService.DeleteAsync(id);
            if (!deleted)
                return NotFound(new { Message = $"PaymentTransaction with id '{id}' not found." });

            return NoContent();
        }
    }
}

