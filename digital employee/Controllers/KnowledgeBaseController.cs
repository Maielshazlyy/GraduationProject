using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service_layer.DTOS.KnowledgeBase;
using Service_layer.Mapping;
using Service_layer.Services_Interfaces;
using System.Security.Claims;

namespace digital_employee.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class KnowledgeBaseController : ControllerBase
    {
        private readonly IKnowledgeBaseService _knowledgeBaseService;
        private readonly IAuditLogService _auditLogService;
        private readonly IAiKnowledgeSyncService _aiSync;
        private readonly IBusinessService _businessService;

        public KnowledgeBaseController(
            IKnowledgeBaseService knowledgeBaseService,
            IAuditLogService auditLogService,
            IAiKnowledgeSyncService aiSync,
            IBusinessService businessService)
        {
            _knowledgeBaseService = knowledgeBaseService;
            _auditLogService      = auditLogService;
            _aiSync               = aiSync;
            _businessService      = businessService;
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
            // Get only internal KnowledgeBase items (not FAQs)
            var kb = await _knowledgeBaseService.GetKnowledgeBaseByBusinessIdAsync(businessId);
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
                
                // Log knowledge base creation
                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!string.IsNullOrWhiteSpace(dto.BusinessId) && !string.IsNullOrWhiteSpace(currentUserId))
                {
                    await _auditLogService.LogKnowledgeBaseActionAsync(
                        businessId: dto.BusinessId,
                        action: "CreateKnowledgeBase",
                        knowledgeBaseId: kb.KnowledgeBaseId,
                        userId: currentUserId
                    );
                }
                
                return CreatedAtAction(nameof(GetById), new { id = kb.KnowledgeBaseId }, kb.ToDto());
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // POST: api/KnowledgeBase/bulk
        [HttpPost("bulk")]
        [Authorize(Policy = "OwnerOrAdmin")]
        public async Task<IActionResult> BulkCreate([FromBody] KnowledgeBaseBulkCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var entries = await _knowledgeBaseService.BulkCreateAsync(dto);

                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!string.IsNullOrWhiteSpace(dto.BusinessId) && !string.IsNullOrWhiteSpace(currentUserId))
                {
                    await _auditLogService.LogKnowledgeBaseActionAsync(
                        businessId: dto.BusinessId,
                        action: "BulkCreateKnowledgeBase",
                        knowledgeBaseId: $"{entries.Count()} entries",
                        userId: currentUserId
                    );
                }

                return Ok(entries.ToDtoList());
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

            // Log knowledge base update
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!string.IsNullOrWhiteSpace(kb.BusinessId) && !string.IsNullOrWhiteSpace(currentUserId))
            {
                await _auditLogService.LogKnowledgeBaseActionAsync(
                    businessId: kb.BusinessId,
                    action: "UpdateKnowledgeBase",
                    knowledgeBaseId: id,
                    userId: currentUserId
                );
            }

            return Ok(kb.ToDto());
        }

        // POST: api/KnowledgeBase/sync
        // Pushes menu + KB for every business to the AI server in one shot.
        // Use after seeding or any bulk DB change that bypassed the API.
        [HttpPost("sync")]
        [Authorize(Policy = "OwnerOrAdmin")]
        public async Task<IActionResult> SyncAll()
        {
            var businesses = await _businessService.GetAllAsync();
            var results    = new List<object>();

            foreach (var biz in businesses)
            {
                try
                {
                    await _aiSync.SyncBusinessAsync(biz.Id);
                    results.Add(new { businessId = biz.Id, name = biz.Name, status = "synced" });
                }
                catch (Exception ex)
                {
                    results.Add(new { businessId = biz.Id, name = biz.Name, status = "failed", error = ex.Message });
                }
            }

            return Ok(new { total = results.Count, results });
        }

        // DELETE: api/KnowledgeBase/{id}
        [HttpDelete("{id}")]
        [Authorize(Policy = "OwnerOrAdmin")]
        public async Task<IActionResult> Delete(string id)
        {
            // Get knowledge base before deletion to log it
            var kb = await _knowledgeBaseService.GetByIdAsync(id);
            if (kb == null)
                return NotFound(new { Message = $"KnowledgeBase with id '{id}' not found." });

            var deleted = await _knowledgeBaseService.DeleteAsync(id);
            if (!deleted)
                return NotFound(new { Message = $"KnowledgeBase with id '{id}' not found." });

            // Log knowledge base deletion
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!string.IsNullOrWhiteSpace(kb.BusinessId) && !string.IsNullOrWhiteSpace(currentUserId))
            {
                await _auditLogService.LogKnowledgeBaseActionAsync(
                    businessId: kb.BusinessId,
                    action: "DeleteKnowledgeBase",
                    knowledgeBaseId: id,
                    userId: currentUserId
                );
            }

            return NoContent();
        }
    }
}

