using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service_layer.DTOS.Order;
using Service_layer.Mapping;
using Service_layer.Services_Interfaces;
using System.Security.Claims;

namespace digital_employee.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IAuditLogService _auditLogService;

        public OrderController(IOrderService orderService, IAuditLogService auditLogService)
        {
            _orderService = orderService;
            _auditLogService = auditLogService;
        }

        // GET: api/Order
        [HttpGet]
        [Authorize(Policy = "AgentOrOwnerOrAdmin")]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _orderService.GetAllAsync();
            return Ok(orders.Select(o => o.ToDto()));
        }

        // GET: api/Order/business/{businessId}
        [HttpGet("business/{businessId}")]
        [Authorize(Policy = "AgentOrOwnerOrAdmin")]
        public async Task<IActionResult> GetByBusinessId(string businessId)
        {
            var orders = await _orderService.GetByBusinessIdAsync(businessId);
            return Ok(orders.Select(o => o.ToDto()));
        }

        // GET: api/Order/customer/{customerId}
        [HttpGet("customer/{customerId}")]
        [Authorize(Policy = "AgentOrOwnerOrAdmin")]
        public async Task<IActionResult> GetByCustomerId(string customerId)
        {
            var orders = await _orderService.GetByCustomerIdAsync(customerId);
            return Ok(orders.Select(o => o.ToDto()));
        }

        // GET: api/Order/{id}
        [HttpGet("{id}")]
        [Authorize(Policy = "AgentOrOwnerOrAdmin")]
        public async Task<IActionResult> GetById(string id)
        {
            var order = await _orderService.GetByIdAsync(id);
            if (order == null)
                return NotFound(new { Message = $"Order with id '{id}' not found." });

            return Ok(order.ToDto());
        }

        // POST: api/Order
        [HttpPost]
        [Authorize(Policy = "AgentOrOwnerOrAdmin")]
        public async Task<IActionResult> Create([FromBody] OrderCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var order = await _orderService.CreateAsync(dto);
                
                // Log order creation
                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!string.IsNullOrWhiteSpace(dto.BusinessId) && !string.IsNullOrWhiteSpace(currentUserId))
                {
                    await _auditLogService.LogOrderActionAsync(
                        businessId: dto.BusinessId,
                        action: "CreateOrder",
                        orderId: order.OrderId,
                        userId: currentUserId
                    );
                }
                
                return CreatedAtAction(nameof(GetById), new { id = order.OrderId }, order.ToDto());
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // PUT: api/Order/{id}/status
        [HttpPut("{id}/status")]
        [Authorize(Policy = "OwnerOrAdmin")]
        public async Task<IActionResult> UpdateStatus(string id, [FromBody] UpdateOrderStatusDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var order = await _orderService.UpdateStatusAsync(id, dto);
                if (order == null)
                    return NotFound(new { Message = $"Order with id '{id}' not found." });

                // Log order status update
                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!string.IsNullOrWhiteSpace(order.BusinessId) && !string.IsNullOrWhiteSpace(currentUserId))
                {
                    await _auditLogService.LogOrderActionAsync(
                        businessId: order.BusinessId,
                        action: $"UpdateOrderStatus_{dto.Status}",
                        orderId: order.OrderId,
                        userId: currentUserId
                    );
                }

                return Ok(order.ToDto());
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // DELETE: api/Order/{id}
        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Delete(string id)
        {
            // Get order before deletion to log it
            var order = await _orderService.GetByIdAsync(id);
            if (order == null)
                return NotFound(new { Message = $"Order with id '{id}' not found." });

            var deleted = await _orderService.DeleteAsync(id);
            if (!deleted)
                return NotFound(new { Message = $"Order with id '{id}' not found." });

            // Log order deletion
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!string.IsNullOrWhiteSpace(order.BusinessId) && !string.IsNullOrWhiteSpace(currentUserId))
            {
                await _auditLogService.LogOrderActionAsync(
                    businessId: order.BusinessId,
                    action: "DeleteOrder",
                    orderId: id,
                    userId: currentUserId
                );
            }

            return NoContent();
        }
    }
}

