using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service_layer.DTOS.KnowledgeBase;
using Service_layer.Mapping;
using Service_layer.Services_Interfaces;

namespace digital_employee.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FAQController : ControllerBase
    {
        private readonly IKnowledgeBaseService _knowledgeBaseService;

        public FAQController(IKnowledgeBaseService knowledgeBaseService)
        {
            _knowledgeBaseService = knowledgeBaseService;
        }

        // GET: api/FAQ/business/{businessId}
        // للعملاء - عرض FAQs فقط (AllowAnonymous أو Authorize)
        [HttpGet("business/{businessId}")]
        [AllowAnonymous] // يمكن للعملاء رؤية FAQs بدون تسجيل دخول
        public async Task<IActionResult> GetFAQsByBusiness(string businessId)
        {
            var faqs = await _knowledgeBaseService.GetFAQsByBusinessIdAsync(businessId);
            return Ok(faqs.ToDtoList());
        }

        // GET: api/FAQ/business/{businessId}/manage
        // للإدارة - إدارة FAQs (OwnerOrAdmin)
        [HttpGet("business/{businessId}/manage")]
        [Authorize(Policy = "OwnerOrAdmin")]
        public async Task<IActionResult> ManageFAQs(string businessId)
        {
            var faqs = await _knowledgeBaseService.GetFAQsByBusinessIdAsync(businessId);
            return Ok(faqs.ToDtoList());
        }

        // POST: api/FAQ
        // إنشاء FAQ جديد (يستخدم KnowledgeBase مع IsFAQ = true)
        [HttpPost]
        [Authorize(Policy = "OwnerOrAdmin")]
        public async Task<IActionResult> CreateFAQ([FromBody] KnowledgeBaseCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                // Force IsFAQ = true for FAQs
                dto.IsFAQ = true;
                var faq = await _knowledgeBaseService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetFAQById), new { id = faq.KnowledgeBaseId }, faq.ToDto());
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // GET: api/FAQ/{id}
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetFAQById(string id)
        {
            var faq = await _knowledgeBaseService.GetByIdAsync(id);
            if (faq == null)
                return NotFound(new { Message = $"FAQ with id '{id}' not found." });

            return Ok(faq.ToDto());
        }

        // PUT: api/FAQ/{id}
        [HttpPut("{id}")]
        [Authorize(Policy = "OwnerOrAdmin")]
        public async Task<IActionResult> UpdateFAQ(string id, [FromBody] KnowledgeBaseCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var faq = await _knowledgeBaseService.UpdateAsync(id, dto);
            if (faq == null)
                return NotFound(new { Message = $"FAQ with id '{id}' not found." });

            return Ok(faq.ToDto());
        }

        // DELETE: api/FAQ/{id}
        [HttpDelete("{id}")]
        [Authorize(Policy = "OwnerOrAdmin")]
        public async Task<IActionResult> DeleteFAQ(string id)
        {
            var deleted = await _knowledgeBaseService.DeleteAsync(id);
            if (!deleted)
                return NotFound(new { Message = $"FAQ with id '{id}' not found." });

            return NoContent();
        }
    }
}

