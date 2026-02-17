using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service_layer.DTOS.MenuCategory;
using Service_layer.Mapping;
using Service_layer.Services_Interfaces;

namespace digital_employee.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MenuCategoryController : ControllerBase
    {
        private readonly IMenuCategoryService _menuCategoryService;

        public MenuCategoryController(IMenuCategoryService menuCategoryService)
        {
            _menuCategoryService = menuCategoryService;
        }

        // GET: api/MenuCategory
        [HttpGet]
        [Authorize(Policy = "AgentOrOwnerOrAdmin")]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _menuCategoryService.GetAllAsync();
            return Ok(categories.ToDtoList());
        }

        // GET: api/MenuCategory/business/{businessId}
        [HttpGet("business/{businessId}")]
        [Authorize(Policy = "AgentOrOwnerOrAdmin")]
        public async Task<IActionResult> GetByBusinessId(string businessId)
        {
            var categories = await _menuCategoryService.GetByBusinessIdAsync(businessId);
            return Ok(categories.OrderBy(c => c.DisplayOrder).ToDtoList());
        }

        // GET: api/MenuCategory/business/{businessId}/active
        [HttpGet("business/{businessId}/active")]
        [AllowAnonymous] // للعملاء يمكنهم رؤية الفئات النشطة فقط
        public async Task<IActionResult> GetActiveByBusinessId(string businessId)
        {
            var categories = await _menuCategoryService.GetActiveByBusinessIdAsync(businessId);
            return Ok(categories.OrderBy(c => c.DisplayOrder).ToDtoList());
        }

        // GET: api/MenuCategory/{id}
        [HttpGet("{id}")]
        [Authorize(Policy = "AgentOrOwnerOrAdmin")]
        public async Task<IActionResult> GetById(string id)
        {
            var category = await _menuCategoryService.GetByIdAsync(id);
            if (category == null)
                return NotFound(new { Message = $"MenuCategory with id '{id}' not found." });

            return Ok(category.ToDto());
        }

        // POST: api/MenuCategory
        [HttpPost]
        [Authorize(Policy = "OwnerOrAdmin")]
        public async Task<IActionResult> Create([FromBody] MenuCategoryCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var category = await _menuCategoryService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = category.MenuCategoryId }, category.ToDto());
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // PUT: api/MenuCategory/{id}
        [HttpPut("{id}")]
        [Authorize(Policy = "OwnerOrAdmin")]
        public async Task<IActionResult> Update(string id, [FromBody] MenuCategoryUpdateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var category = await _menuCategoryService.UpdateAsync(id, dto);
            if (category == null)
                return NotFound(new { Message = $"MenuCategory with id '{id}' not found." });

            return Ok(category.ToDto());
        }

        // DELETE: api/MenuCategory/{id}
        [HttpDelete("{id}")]
        [Authorize(Policy = "OwnerOrAdmin")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var deleted = await _menuCategoryService.DeleteAsync(id);
                if (!deleted)
                    return NotFound(new { Message = $"MenuCategory with id '{id}' not found." });

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // PUT: api/MenuCategory/business/{businessId}/reorder
        [HttpPut("business/{businessId}/reorder")]
        [Authorize(Policy = "OwnerOrAdmin")]
        public async Task<IActionResult> ReorderCategories(string businessId, [FromBody] ReorderCategoriesDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _menuCategoryService.ReorderCategoriesAsync(businessId, dto);
                return Ok(new { Message = "Categories reordered successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}

