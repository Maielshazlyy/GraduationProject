using System;
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
        public async Task<IActionResult> GetAll()
        {
            var businesses = await _businessService.GetAllAsync();
            return Ok(businesses.ToDtoList());
        }

        // GET: api/Business/{id}
        [HttpGet("{id}")]
        [Authorize(Policy = "OwnerOrAdmin")]
        public async Task<IActionResult> GetById(string id)
        {
            var business = await _businessService.GetByIdAsync(id);
            if (business == null)
                return NotFound(new { Message = $"Business with id '{id}' not found." });

            return Ok(business.ToDto());
        }

        // POST: api/Business
        [HttpPost]
        [Authorize(Policy = "OwnerOrAdmin")]
        public async Task<IActionResult> Create([FromBody] BusinessCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

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
        [HttpPost("onboard")]
        [AllowAnonymous] // Allow public access for first-time business registration
        public async Task<IActionResult> Onboard([FromBody] BusinessOnboardingDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var business = await _businessService.OnboardRestaurantAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = business.Id }, business.ToDto());
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Onboarding failed.", Error = ex.Message });
            }
        }

        // PUT: api/Business/{id}
        [HttpPut("{id}")]
        [Authorize(Policy = "OwnerOrAdmin")]
        public async Task<IActionResult> Update(string id, [FromBody] BusinessCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var business = new Domain_layer.Models.Business
            {
                Name = dto.Name,
                Type = dto.Type,
                Address = dto.Address,
                Phone = dto.Phone
            };

            var updated = await _businessService.UpdateAsync(id, business);
            if (updated == null)
                return NotFound(new { Message = $"Business with id '{id}' not found." });

            return Ok(updated.ToDto());
        }

        // DELETE: api/Business/{id}
        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Delete(string id)
        {
            var deleted = await _businessService.DeleteAsync(id);
            if (!deleted)
                return NotFound(new { Message = $"Business with id '{id}' not found." });

            return NoContent();
        }
    }
}

