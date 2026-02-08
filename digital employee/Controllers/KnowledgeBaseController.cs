using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service_layer.DTOS.KnowledgeBase;
using Service_layer.Mapping;
using Service_layer.Services_Interfaces;

namespace digital_employee.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class KnowledgeBaseController : ControllerBase
    {
        private readonly IKnowledgeBaseService _knowledgeBaseService;

        public KnowledgeBaseController(IKnowledgeBaseService knowledgeBaseService)
        {
            _knowledgeBaseService = knowledgeBaseService;
        }

        // GET: api/KnowledgeBase
        [HttpGet]
        [Authorize(Policy = "AgentOrOwnerOrAdmin")]
        public async Task<IActionResult> GetAll()
        {
            var kb = await _knowledgeBaseService.GetAllAsync();
            return Ok(kb.ToDtoList());
        }

        // GET: api/KnowledgeBase/business/{businessId}
        [HttpGet("business/{businessId}")]
        [Authorize(Policy = "AgentOrOwnerOrAdmin")]
        public async Task<IActionResult> GetByBusinessId(string businessId)
        {
            var kb = await _knowledgeBaseService.GetByBusinessIdAsync(businessId);
            return Ok(kb.ToDtoList());
        }

        // GET: api/KnowledgeBase/{id}
        [HttpGet("{id}")]
        [Authorize(Policy = "AgentOrOwnerOrAdmin")]
        public async Task<IActionResult> GetById(string id)
        {
            var kb = await _knowledgeBaseService.GetByIdAsync(id);
            if (kb == null)
                return NotFound(new { Message = $"KnowledgeBase with id '{id}' not found." });

            return Ok(kb.ToDto());
        }

        // POST: api/KnowledgeBase
        [HttpPost]
        [Authorize(Policy = "OwnerOrAdmin")]
        public async Task<IActionResult> Create([FromBody] KnowledgeBaseCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var kb = await _knowledgeBaseService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = kb.KnowledgeBaseId }, kb.ToDto());
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // PUT: api/KnowledgeBase/{id}
        [HttpPut("{id}")]
        [Authorize(Policy = "OwnerOrAdmin")]
        public async Task<IActionResult> Update(string id, [FromBody] KnowledgeBaseCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var kb = await _knowledgeBaseService.UpdateAsync(id, dto);
            if (kb == null)
                return NotFound(new { Message = $"KnowledgeBase with id '{id}' not found." });

            return Ok(kb.ToDto());
        }

        // DELETE: api/KnowledgeBase/{id}
        [HttpDelete("{id}")]
        [Authorize(Policy = "OwnerOrAdmin")]
        public async Task<IActionResult> Delete(string id)
        {
            var deleted = await _knowledgeBaseService.DeleteAsync(id);
            if (!deleted)
                return NotFound(new { Message = $"KnowledgeBase with id '{id}' not found." });

            return NoContent();
        }
    }
}

