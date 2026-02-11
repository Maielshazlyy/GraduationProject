using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Domain_layer.Models;
using Domain_layer.Constants;
using Service_layer.Services_Interfaces;
using Service_layer.DTOS.Business;
using Service_layer.Mapping;

namespace digital_employee.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BusinessController : ControllerBase
    {
        private readonly IBusinessService _businessService;
        private readonly UserManager<User> _userManager;

        public BusinessController(IBusinessService businessService, UserManager<User> userManager)
        {
            _businessService = businessService;
            _userManager = userManager;
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
        [Authorize(Policy = "OwnerOrAdmin")] // فقط Admin أو Owner يمكنهم إنشاء Business
        public async Task<IActionResult> Create([FromBody] BusinessCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // الحصول على المستخدم الحالي
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { Message = "User not found." });

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return Unauthorized(new { Message = "User not found." });

            // التحقق من أن المستخدم ليس لديه Business بالفعل
            if (!string.IsNullOrEmpty(user.BusinessId))
                return BadRequest(new { Message = "User already has a business. One user can only have one business." });

            var business = new Domain_layer.Models.Business
            {
                Id = Guid.NewGuid().ToString(),
                Name = dto.Name,
                Type = dto.Type,
                Address = dto.Address,
                Phone = dto.Phone,
                // Contact Information
                Email = dto.Email,
                Website = dto.Website,
                FacebookUrl = dto.FacebookUrl,
                InstagramUrl = dto.InstagramUrl,
                // Location
                City = dto.City,
                Country = dto.Country,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                // Restaurant Information
                Description = dto.Description,
                CuisineType = dto.CuisineType,
                PriceRange = dto.PriceRange,
                LogoUrl = dto.LogoUrl,
                CoverImageUrl = dto.CoverImageUrl,
                // Features & Services
                HasDelivery = dto.HasDelivery,
                HasTakeout = dto.HasTakeout,
                HasParking = dto.HasParking,
                HasWiFi = dto.HasWiFi,
                HasOutdoorSeating = dto.HasOutdoorSeating,
                AcceptsReservations = dto.AcceptsReservations,
                // Payment Methods
                PaymentMethods = dto.PaymentMethods,
                // Status
                IsActive = true,
                IsVerified = false,
                CreatedAt = DateTime.UtcNow
            };

            // Create Working Hours if provided
            if (dto.WorkingHours != null && dto.WorkingHours.Any())
            {
                var workingHours = dto.WorkingHours.Select(wh => new WorkingHours
                {
                    WorkingHoursId = Guid.NewGuid().ToString(),
                    DayOfWeek = wh.DayOfWeek,
                    OpenTime = !string.IsNullOrWhiteSpace(wh.OpenTime) && TimeSpan.TryParse(wh.OpenTime, out var openTime) ? openTime : null,
                    CloseTime = !string.IsNullOrWhiteSpace(wh.CloseTime) && TimeSpan.TryParse(wh.CloseTime, out var closeTime) ? closeTime : null,
                    IsClosed = wh.IsClosed,
                    BusinessId = business.Id
                }).ToList();
                
                business.WorkingHours = workingHours;
            }

            var created = await _businessService.CreateAsync(business);

            // ربط المستخدم بالـ Business وتحديث role إلى Owner
            user.BusinessId = created.Id;
            user.Role = Roles.Owner;
            await _userManager.UpdateAsync(user);

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created.ToDto());
        }

        // POST: api/Business/onboard
        [HttpPost("onboard")]
        [AllowAnonymous]
        public async Task<IActionResult> Onboard([FromBody] BusinessOnboardingDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var business = await _businessService.OnboardRestaurantAsync(dto);
                
                // Optional: إذا كان المستخدم مسجل دخول، اربطه بالـ Business
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!string.IsNullOrEmpty(userId))
                {
                    var user = await _userManager.FindByIdAsync(userId);
                    if (user != null && string.IsNullOrEmpty(user.BusinessId))
                    {
                        user.BusinessId = business.Id;
                        user.Role = Roles.Owner;
                        await _userManager.UpdateAsync(user);
                    }
                }
                
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
        public async Task<IActionResult> Update(string id, [FromBody] BusinessUpdateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = await _businessService.UpdateAsync(id, dto);
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
