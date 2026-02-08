using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service_layer.DTOS.Feedback;
using Service_layer.Mapping;
using Service_layer.Services_Interfaces;

namespace digital_employee.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService _feedbackService;

        public FeedbackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        // GET: api/Feedback
        [HttpGet]
        [Authorize(Policy = "AgentOrOwnerOrAdmin")]
        public async Task<IActionResult> GetAll()
        {
            var feedbacks = await _feedbackService.GetAllAsync();
            return Ok(feedbacks.ToDtoList());
        }

        // GET: api/Feedback/customer/{customerId}
        [HttpGet("customer/{customerId}")]
        [Authorize(Policy = "AgentOrOwnerOrAdmin")]
        public async Task<IActionResult> GetByCustomerId(string customerId)
        {
            var feedbacks = await _feedbackService.GetByCustomerIdAsync(customerId);
            return Ok(feedbacks.ToDtoList());
        }

        // GET: api/Feedback/{id}
        [HttpGet("{id}")]
        [Authorize(Policy = "AgentOrOwnerOrAdmin")]
        public async Task<IActionResult> GetById(string id)
        {
            var feedback = await _feedbackService.GetByIdAsync(id);
            if (feedback == null)
                return NotFound(new { Message = $"Feedback with id '{id}' not found." });

            return Ok(feedback.ToDto());
        }

        // POST: api/Feedback
        [HttpPost]
        [AllowAnonymous] // Customers can submit feedback without authentication
        public async Task<IActionResult> Create([FromBody] FeedbackCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var feedback = await _feedbackService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = feedback.FeedbackId }, feedback.ToDto());
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // PUT: api/Feedback/{id}
        [HttpPut("{id}")]
        [Authorize(Policy = "OwnerOrAdmin")]
        public async Task<IActionResult> Update(string id, [FromBody] FeedbackUpdateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var feedback = await _feedbackService.UpdateAsync(id, dto);
                if (feedback == null)
                    return NotFound(new { Message = $"Feedback with id '{id}' not found." });

                return Ok(feedback.ToDto());
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // DELETE: api/Feedback/{id}
        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Delete(string id)
        {
            var deleted = await _feedbackService.DeleteAsync(id);
            if (!deleted)
                return NotFound(new { Message = $"Feedback with id '{id}' not found." });

            return NoContent();
        }
    }
}

