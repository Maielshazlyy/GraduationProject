using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service_layer.Services_Interfaces;
using System.Security.Claims;

namespace digital_employee.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "AdminOnly")]
    public class AdminDashboardController : ControllerBase
    {
        private readonly IAdminDashboardService _adminDashboardService;

        public AdminDashboardController(IAdminDashboardService adminDashboardService)
        {
            _adminDashboardService = adminDashboardService;
        }

        // GET: api/AdminDashboard/summary
        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary()
        {
            var summary = await _adminDashboardService.GetSummaryAsync();
            return Ok(summary);
        }

        // GET: api/AdminDashboard/top-businesses?count=10
        [HttpGet("top-businesses")]
        public async Task<IActionResult> GetTopBusinesses([FromQuery] int count = 10)
        {
            var businesses = await _adminDashboardService.GetTopBusinessesByRevenueAsync(count);
            return Ok(businesses);
        }

        // GET: api/AdminDashboard/full?topBusinessesCount=10
        [HttpGet("full")]
        public async Task<IActionResult> GetFull([FromQuery] int topBusinessesCount = 10)
        {
            var dashboard = await _adminDashboardService.GetFullDashboardAsync(topBusinessesCount);
            return Ok(dashboard);
        }

        // GET: api/AdminDashboard/alerts
        [HttpGet("alerts")]
        public async Task<IActionResult> GetAlerts()
        {
            var alerts = await _adminDashboardService.GetAlertsAsync();
            return Ok(alerts);
        }

        // GET: api/AdminDashboard/revenue-trend?months=12
        [HttpGet("revenue-trend")]
        public async Task<IActionResult> GetRevenueTrend([FromQuery] int months = 12)
        {
            var trend = await _adminDashboardService.GetRevenueTrendAsync(months);
            return Ok(trend);
        }

        // GET: api/AdminDashboard/orders-by-status
        [HttpGet("orders-by-status")]
        public async Task<IActionResult> GetOrdersByStatus()
        {
            var data = await _adminDashboardService.GetOrdersByStatusAsync();
            return Ok(data);
        }

        // GET: api/AdminDashboard/tickets-by-priority
        [HttpGet("tickets-by-priority")]
        public async Task<IActionResult> GetTicketsByPriority()
        {
            var data = await _adminDashboardService.GetTicketsByPriorityAsync();
            return Ok(data);
        }

        // GET: api/AdminDashboard/business-health?top=20&sort=desc
        [HttpGet("business-health")]
        public async Task<IActionResult> GetBusinessHealth([FromQuery] int top = 20, [FromQuery] string sort = "desc")
        {
            var ascending = string.Equals(sort, "asc", StringComparison.OrdinalIgnoreCase);
            var data = await _adminDashboardService.GetBusinessHealthAsync(top, ascending);
            return Ok(data);
        }

        // GET: api/AdminDashboard/sentiment-trend?days=30
        [HttpGet("sentiment-trend")]
        public async Task<IActionResult> GetSentimentTrend([FromQuery] int days = 30)
        {
            var trend = await _adminDashboardService.GetSentimentTrendAsync(days);
            return Ok(trend);
        }

        // POST: api/AdminDashboard/business/{businessId}/suspend
        [HttpPost("business/{businessId}/suspend")]
        public async Task<IActionResult> SuspendBusiness(string businessId)
        {
            var adminUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var updated = await _adminDashboardService.SetBusinessActiveStateAsync(businessId, false, adminUserId);
            if (!updated)
                return NotFound(new { Message = $"Business with id '{businessId}' not found." });

            return Ok(new { Message = "Business suspended successfully." });
        }

        // POST: api/AdminDashboard/business/{businessId}/activate
        [HttpPost("business/{businessId}/activate")]
        public async Task<IActionResult> ActivateBusiness(string businessId)
        {
            var adminUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var updated = await _adminDashboardService.SetBusinessActiveStateAsync(businessId, true, adminUserId);
            if (!updated)
                return NotFound(new { Message = $"Business with id '{businessId}' not found." });

            return Ok(new { Message = "Business activated successfully." });
        }

        // POST: api/AdminDashboard/business/{businessId}/verify
        [HttpPost("business/{businessId}/verify")]
        public async Task<IActionResult> VerifyBusiness(string businessId)
        {
            var adminUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var updated = await _adminDashboardService.SetBusinessVerificationStateAsync(businessId, true, adminUserId);
            if (!updated)
                return NotFound(new { Message = $"Business with id '{businessId}' not found." });

            return Ok(new { Message = "Business verified successfully." });
        }

        // POST: api/AdminDashboard/business/{businessId}/unverify
        [HttpPost("business/{businessId}/unverify")]
        public async Task<IActionResult> UnverifyBusiness(string businessId)
        {
            var adminUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var updated = await _adminDashboardService.SetBusinessVerificationStateAsync(businessId, false, adminUserId);
            if (!updated)
                return NotFound(new { Message = $"Business with id '{businessId}' not found." });

            return Ok(new { Message = "Business unverified successfully." });
        }
    }
}
