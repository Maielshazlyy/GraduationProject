using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Service_layer.Services_Interfaces;
using Service_layer.DTOS.Business;
using Service_layer.Mapping;

namespace digital_employee.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // default: only authenticated users can access business endpoints
    public class BusinessController : ControllerBase
    {
        private readonly IBusinessService _businessService;

        public BusinessController(IBusinessService businessService)
        {
            _businessService = businessService;
        }

        // GET: api/Business
        [HttpGet]
        [Authorize(Policy = "OwnerOrAdmin")]
        public async Task<ActionResult<IEnumerable<BusinessResponseDTO>>> GetAll()
        {
            var businesses = await _businessService.GetAllAsync();
            return Ok(businesses.ToDtoList());
        }

        // GET: api/Business/{id}
        [HttpGet("{id}")]
        [Authorize(Policy = "OwnerOrAdmin")]
        public async Task<ActionResult<BusinessResponseDTO>> GetById(string id)
        {
            var business = await _businessService.GetByIdAsync(id);
            if (business == null) return NotFound();
            return Ok(business.ToDto());
        }

        // POST: api/Business
        // Simple create (CRUD) using existing BusinessCreateDTO
        [HttpPost]
        [Authorize(Policy = "OwnerOrAdmin")]
        public async Task<ActionResult<BusinessResponseDTO>> Create([FromBody] BusinessCreateDTO dto)
        {
            var business = new Domain_layer.Models.Business
            {
                Name = dto.Name,
                Type = dto.Type,
                Address = dto.Address,
                Phone = dto.Phone
            };

            var created = await _businessService.CreateAsync(business);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created.ToDto());
        }

        // POST: api/Business/onboard
        // Full onboarding flow for restaurant owner (business + agent config + KB + subscription + payment)
        [HttpPost("onboard")]
        [AllowAnonymous] // or Authorize depending on your UX (e.g. after registration)
        public async Task<ActionResult<BusinessResponseDTO>> Onboard([FromBody] BusinessOnboardingDTO dto)
        {
            var business = await _businessService.OnboardRestaurantAsync(dto);
            return Ok(business.ToDto());
        }

        // PUT: api/Business/{id}
        [HttpPut("{id}")]
        [Authorize(Policy = "OwnerOrAdmin")]
        public async Task<ActionResult<BusinessResponseDTO>> Update(string id, [FromBody] BusinessCreateDTO dto)
        {
            var updatedBusiness = new Domain_layer.Models.Business
            {
                Name = dto.Name,
                Type = dto.Type,
                Address = dto.Address,
                Phone = dto.Phone
            };

            var result = await _businessService.UpdateAsync(id, updatedBusiness);
            if (result == null) return NotFound();

            return Ok(result.ToDto());
        }

        // DELETE: api/Business/{id}
        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Delete(string id)
        {
            var deleted = await _businessService.DeleteAsync(id);
            if (!deleted) return NotFound();

            return NoContent();
        }
    }
}


