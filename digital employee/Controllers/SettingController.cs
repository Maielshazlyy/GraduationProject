using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service_layer.DTOS.Setting;
using Service_layer.Mapping;
using Service_layer.Services_Interfaces;

namespace digital_employee.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SettingController : ControllerBase
    {
        private readonly ISettingService _settingService;

        public SettingController(ISettingService settingService)
        {
            _settingService = settingService;
        }

        // GET: api/Setting/business/{businessId}
        [HttpGet("business/{businessId}")]
        [Authorize(Policy = "OwnerOrAdmin")]
        public async Task<IActionResult> GetByBusinessId(string businessId)
        {
            var setting = await _settingService.GetByBusinessIdAsync(businessId);
            if (setting == null)
                return NotFound(new { Message = $"Settings not found for business '{businessId}'." });

            return Ok(setting.ToDto());
        }

        // PUT: api/Setting/business/{businessId}
        [HttpPut("business/{businessId}")]
        [Authorize(Policy = "OwnerOrAdmin")]
        public async Task<IActionResult> Update(string businessId, [FromBody] SettingUpdateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var setting = await _settingService.UpdateAsync(businessId, dto);
                return Ok(setting.ToDto());
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}

