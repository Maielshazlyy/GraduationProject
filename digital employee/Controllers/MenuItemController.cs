using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service_layer.DTOS.menuItem;
using Service_layer.Mapping;
using Service_layer.Services_Interfaces;

namespace digital_employee.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MenuItemController : ControllerBase
    {
        private readonly IMenuItemService _menuItemService;

        public MenuItemController(IMenuItemService menuItemService)
        {
            _menuItemService = menuItemService;
        }

        // GET: api/MenuItem
        [HttpGet]
        [Authorize(Policy = "AgentOrOwnerOrAdmin")]
        public async Task<IActionResult> GetAll()
        {
            var items = await _menuItemService.GetAllAsync();
            return Ok(items.ToDtoList());
        }

        // GET: api/MenuItem/business/{businessId}
        [HttpGet("business/{businessId}")]
        [Authorize(Policy = "AgentOrOwnerOrAdmin")]
        public async Task<IActionResult> GetByBusinessId(string businessId)
        {
            var items = await _menuItemService.GetByBusinessIdAsync(businessId);
            return Ok(items.ToDtoList());
        }

        // GET: api/MenuItem/{id}
        [HttpGet("{id}")]
        [Authorize(Policy = "AgentOrOwnerOrAdmin")]
        public async Task<IActionResult> GetById(string id)
        {
            var item = await _menuItemService.GetByIdAsync(id);
            if (item == null)
                return NotFound(new { Message = $"MenuItem with id '{id}' not found." });

            return Ok(item.ToDto());
        }

        // POST: api/MenuItem
        [HttpPost]
        [Authorize(Policy = "OwnerOrAdmin")]
        public async Task<IActionResult> Create([FromBody] MenuItemCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var item = await _menuItemService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = item.MenuItemId }, item.ToDto());
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // PUT: api/MenuItem/{id}
        [HttpPut("{id}")]
        [Authorize(Policy = "OwnerOrAdmin")]
        public async Task<IActionResult> Update(string id, [FromBody] MenuItemUpdateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var item = await _menuItemService.UpdateAsync(id, dto);
            if (item == null)
                return NotFound(new { Message = $"MenuItem with id '{id}' not found." });

            return Ok(item.ToDto());
        }

        // DELETE: api/MenuItem/{id}
        [HttpDelete("{id}")]
        [Authorize(Policy = "OwnerOrAdmin")]
        public async Task<IActionResult> Delete(string id)
        {
            var deleted = await _menuItemService.DeleteAsync(id);
            if (!deleted)
                return NotFound(new { Message = $"MenuItem with id '{id}' not found." });

            return NoContent();
        }
    }
}

