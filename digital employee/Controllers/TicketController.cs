using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service_layer.DTOS.Ticket;
using Service_layer.Mapping;
using Service_layer.Services_Interfaces;

namespace digital_employee.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public TicketController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        // GET: api/Ticket
        [HttpGet]
        [Authorize(Policy = "AgentOrOwnerOrAdmin")]
        public async Task<IActionResult> GetAll()
        {
            var tickets = await _ticketService.GetAllAsync();
            return Ok(tickets.ToDtoList());
        }

        // GET: api/Ticket/business/{businessId}
        [HttpGet("business/{businessId}")]
        [Authorize(Policy = "AgentOrOwnerOrAdmin")]
        public async Task<IActionResult> GetByBusinessId(string businessId)
        {
            var tickets = await _ticketService.GetByBusinessIdAsync(businessId);
            return Ok(tickets.ToDtoList());
        }

        // GET: api/Ticket/{id}
        [HttpGet("{id}")]
        [Authorize(Policy = "AgentOrOwnerOrAdmin")]
        public async Task<IActionResult> GetById(string id)
        {
            var ticket = await _ticketService.GetByIdAsync(id);
            if (ticket == null)
                return NotFound(new { Message = $"Ticket with id '{id}' not found." });

            return Ok(ticket.ToDto());
        }

        // POST: api/Ticket
        [HttpPost]
        [Authorize(Policy = "AgentOrOwnerOrAdmin")]
        public async Task<IActionResult> Create([FromBody] TicketCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var ticket = await _ticketService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = ticket.Id }, ticket.ToDto());
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // PUT: api/Ticket/{id}
        [HttpPut("{id}")]
        [Authorize(Policy = "AgentOrOwnerOrAdmin")]
        public async Task<IActionResult> Update(string id, [FromBody] TicketUpdateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var ticket = await _ticketService.UpdateAsync(id, dto);
            if (ticket == null)
                return NotFound(new { Message = $"Ticket with id '{id}' not found." });

            return Ok(ticket.ToDto());
        }

        // POST: api/Ticket/{id}/assign
        [HttpPost("{id}/assign")]
        [Authorize(Policy = "OwnerOrAdmin")]
        public async Task<IActionResult> AssignTicket(string id, [FromBody] AssignTicketDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var ticket = await _ticketService.AssignTicketAsync(id, dto);
                if (ticket == null)
                    return NotFound(new { Message = $"Ticket with id '{id}' not found." });

                return Ok(ticket.ToDto());
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // POST: api/Ticket/{id}/close
        [HttpPost("{id}/close")]
        [Authorize(Policy = "AgentOrOwnerOrAdmin")]
        public async Task<IActionResult> CloseTicket(string id, [FromBody] CloseTicketDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var ticket = await _ticketService.CloseTicketAsync(id, dto);
            if (ticket == null)
                return NotFound(new { Message = $"Ticket with id '{id}' not found." });

            return Ok(ticket.ToDto());
        }

        // DELETE: api/Ticket/{id}
        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Delete(string id)
        {
            var deleted = await _ticketService.DeleteAsync(id);
            if (!deleted)
                return NotFound(new { Message = $"Ticket with id '{id}' not found." });

            return NoContent();
        }
    }
}

