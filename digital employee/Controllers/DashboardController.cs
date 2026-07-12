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

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        // GET: api/Dashboard/overview?period=30d
        // Each section can be filtered independently, e.g.
        //   ?period=30d&insightsPeriod=today&productsPeriod=7d&channelsPeriod=all
        // Allowed values: today | 7d | 30d | all | custom
        // For "custom", also pass customFrom & customTo (ISO dates), e.g.
        //   ?period=custom&customFrom=2026-01-01&customTo=2026-03-31
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
            [FromQuery] string? leadsPeriod = null,
            [FromQuery] string? chatAnalysisPeriod = null,
            [FromQuery] string? feedbacksPeriod = null,
            [FromQuery] DateTime? customFrom = null,
            [FromQuery] DateTime? customTo = null)
        {
            var allowed = new[] { "today", "7d", "30d", "all", "custom" };
            string defaultPeriod = string.IsNullOrWhiteSpace(period) ? "30d" : period.Trim().ToLowerInvariant();
            if (!allowed.Contains(defaultPeriod))
                return BadRequest(new { Message = "period must be one of: today, 7d, 30d, all, custom" });

            var filter = new DashboardFilterDTO
            {
                InsightsPeriod     = insightsPeriod     ?? defaultPeriod,
                ChannelsPeriod     = channelsPeriod     ?? defaultPeriod,
                SentimentPeriod    = sentimentPeriod    ?? defaultPeriod,
                RevenuePeriod      = revenuePeriod      ?? defaultPeriod,
                ProductsPeriod     = productsPeriod     ?? defaultPeriod,
                LeadsPeriod        = leadsPeriod        ?? defaultPeriod,
                ChatAnalysisPeriod = chatAnalysisPeriod ?? defaultPeriod,
                FeedbacksPeriod    = feedbacksPeriod    ?? defaultPeriod,
                CustomFrom         = customFrom,
                CustomTo           = customTo
            };

            // If any section resolves to "custom", a valid date window is required
            var usesCustom = new[]
            {
                filter.InsightsPeriod, filter.ChannelsPeriod, filter.SentimentPeriod,
                filter.RevenuePeriod, filter.ProductsPeriod, filter.LeadsPeriod,
                filter.ChatAnalysisPeriod, filter.FeedbacksPeriod
            }.Any(p => string.Equals(p, "custom", StringComparison.OrdinalIgnoreCase));

            if (usesCustom)
            {
                if (!customFrom.HasValue || !customTo.HasValue)
                    return BadRequest(new { Message = "customFrom and customTo are required when period is 'custom'." });
                if (customFrom.Value > customTo.Value)
                    return BadRequest(new { Message = "customFrom must be earlier than or equal to customTo." });
            }

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

