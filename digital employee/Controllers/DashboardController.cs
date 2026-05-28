using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service_layer.DTOS.Dashboard;
using Service_layer.Services_Interfaces;
using System.Security.Claims;

namespace digital_employee.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;
        private readonly IBusinessAnalyticsService _analyticsService;

        public DashboardController(
            IDashboardService dashboardService,
            IBusinessAnalyticsService analyticsService)
        {
            _dashboardService = dashboardService;
            _analyticsService = analyticsService;
        }

        // GET: api/Dashboard/summary
        [HttpGet("summary")]
        [Authorize(Policy = "OwnerOrAdmin")]
        public async Task<IActionResult> GetDashboardSummary()
        {
            try
            {
                // Get BusinessId from token
                var businessId = User.FindFirstValue("BusinessId");
                if (string.IsNullOrEmpty(businessId))
                    return BadRequest(new { Message = "BusinessId not found in token. Please ensure you are linked to a business." });

                var summary = await _dashboardService.GetDashboardSummaryAsync(businessId);
                return Ok(summary);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // GET: api/Dashboard/analytics
        [HttpGet("analytics")]
        [Authorize(Policy = "OwnerOrAdmin")]
        public async Task<IActionResult> GetAnalytics()
        {
            try
            {
                var businessId = User.FindFirstValue("BusinessId");
                if (string.IsNullOrEmpty(businessId))
                    return BadRequest(new { Message = "BusinessId not found in token." });

                var analytics = await _analyticsService.GetBusinessAnalyticsAsync(businessId);
                return Ok(analytics);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // GET: api/Dashboard/full
        [HttpGet("full")]
        [Authorize(Policy = "OwnerOrAdmin")]
        public async Task<IActionResult> GetFullDashboard()
        {
            try
            {
                var businessId = User.FindFirstValue("BusinessId");
                if (string.IsNullOrEmpty(businessId))
                    return BadRequest(new { Message = "BusinessId not found in token." });

                var summary = await _dashboardService.GetDashboardSummaryAsync(businessId);
                var analytics = await _analyticsService.GetBusinessAnalyticsAsync(businessId);

                return Ok(new
                {
                    Summary = summary,
                    Analytics = analytics
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // GET: api/Dashboard/overview?period=30d
        // Each section can be filtered independently, e.g.
        //   ?period=30d&insightsPeriod=today&productsPeriod=7d&channelsPeriod=all
        // Allowed values: today | 7d | 30d | all  (general insights & revenue trend do not support "all")
        // Any section param left out falls back to "period" (or "30d" if that's omitted too).
        [HttpGet("overview")]
        [Authorize(Policy = "OwnerOrAdmin")]
        public async Task<IActionResult> GetOverview(
            [FromQuery] string period = "30d",
            [FromQuery] string? insightsPeriod = null,
            [FromQuery] string? channelsPeriod = null,
            [FromQuery] string? sentimentPeriod = null,
            [FromQuery] string? revenuePeriod = null,
            [FromQuery] string? productsPeriod = null,
            [FromQuery] string? leadsPeriod = null)
        {
            var allowed = new[] { "today", "7d", "30d", "all" };
            string? defaultPeriod = string.IsNullOrWhiteSpace(period) ? "30d" : period.Trim().ToLowerInvariant();
            if (!allowed.Contains(defaultPeriod))
                return BadRequest(new { Message = "period must be one of: today, 7d, 30d, all" });

            var filter = new DashboardFilterDTO
            {
                InsightsPeriod  = insightsPeriod  ?? defaultPeriod,
                ChannelsPeriod  = channelsPeriod  ?? defaultPeriod,
                SentimentPeriod = sentimentPeriod ?? defaultPeriod,
                RevenuePeriod   = revenuePeriod   ?? defaultPeriod,
                ProductsPeriod  = productsPeriod  ?? defaultPeriod,
                LeadsPeriod     = leadsPeriod     ?? defaultPeriod
            };

            try
            {
                var businessId = User.FindFirstValue("BusinessId");
                if (string.IsNullOrEmpty(businessId))
                    return BadRequest(new { Message = "BusinessId not found in token." });

                var result = await _dashboardService.GetFullDashboardAsync(businessId, filter);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // GET: api/Dashboard/audit-logs/recent
        [HttpGet("audit-logs/recent")]
        [Authorize(Policy = "OwnerOrAdmin")]
        public async Task<IActionResult> GetRecentAuditLogs([FromQuery] int count = 20)
        {
            try
            {
                var businessId = User.FindFirstValue("BusinessId");
                if (string.IsNullOrEmpty(businessId))
                    return BadRequest(new { Message = "BusinessId not found in token." });

                var auditLogs = await _dashboardService.GetRecentAuditLogsAsync(businessId, count);
                return Ok(auditLogs);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // GET: api/Dashboard/audit-logs/statistics
        [HttpGet("audit-logs/statistics")]
        [Authorize(Policy = "OwnerOrAdmin")]
        public async Task<IActionResult> GetAuditLogStatistics()
        {
            try
            {
                var businessId = User.FindFirstValue("BusinessId");
                if (string.IsNullOrEmpty(businessId))
                    return BadRequest(new { Message = "BusinessId not found in token." });

                var statistics = await _dashboardService.GetAuditLogStatisticsAsync(businessId);
                return Ok(statistics);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // GET: api/Dashboard/audit-logs/customer/{customerId}
        [HttpGet("audit-logs/customer/{customerId}")]
        [Authorize(Policy = "OwnerOrAdmin")]
        public async Task<IActionResult> GetCustomerAuditLogs(string customerId)
        {
            try
            {
                var businessId = User.FindFirstValue("BusinessId");
                if (string.IsNullOrEmpty(businessId))
                    return BadRequest(new { Message = "BusinessId not found in token." });

                var auditLogs = await _dashboardService.GetCustomerAuditLogsAsync(businessId, customerId);
                return Ok(auditLogs);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}

