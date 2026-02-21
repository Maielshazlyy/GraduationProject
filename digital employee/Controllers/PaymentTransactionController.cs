using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service_layer.DTOS.PaymentTranscation;
using Service_layer.Mapping;
using Service_layer.Services_Interfaces;
using System.Security.Claims;
using Domain_layer.Interfaces;
using PaymentTransactionMapping = Service_layer.Mapping.PaymentTransactionMapping;

namespace digital_employee.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentTransactionController : ControllerBase
    {
        private readonly IPaymentTransactionService _paymentService;
        private readonly IAuditLogService _auditLogService;
        private readonly ISubscriptionRepository _subscriptionRepository;

        public PaymentTransactionController(
            IPaymentTransactionService paymentService, 
            IAuditLogService auditLogService,
            ISubscriptionRepository subscriptionRepository)
        {
            _paymentService = paymentService;
            _auditLogService = auditLogService;
            _subscriptionRepository = subscriptionRepository;
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
                
                // Get BusinessId from subscription to log payment transaction creation
                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!string.IsNullOrWhiteSpace(currentUserId) && !string.IsNullOrWhiteSpace(dto.SubscriptionId))
                {
                    // Get subscription to access BusinessId
                    var subscription = await _subscriptionRepository.GetByIdAsync(dto.SubscriptionId);
                    if (subscription != null && !string.IsNullOrWhiteSpace(subscription.BusinessId))
                    {
                        await _auditLogService.LogPaymentTransactionActionAsync(
                            businessId: subscription.BusinessId,
                            action: "CreatePaymentTransaction",
                            paymentTransactionId: payment.Id,
                            userId: currentUserId
                        );
                    }
                }
                
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

