using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service_layer.DTOS.BusinessReport;
using Service_layer.Services_Interfaces;

namespace digital_employee.Controllers
{
    [Route("api/businesses/{businessId}/reports")]
    [ApiController]
    [Authorize(Policy = "OwnerOrAdmin")]
    public class BusinessReportController : ControllerBase
    {
        private readonly IBusinessReportGenerationService _reportService;

        public BusinessReportController(IBusinessReportGenerationService reportService)
        {
            _reportService = reportService;
        }

        /// <summary>
        /// Generate an AI report from stored interaction analyses for a date range.
        /// </summary>
        [HttpPost("generate")]
        [ProducesResponseType(typeof(AiReportResponseDTO), 200)]
        public async Task<IActionResult> GenerateReport(
            string businessId,
            [FromBody] ReportGenerateRequestDTO dto)
        {
            // Owners can only generate reports for their own business
            var claimBusinessId = User.FindFirstValue("BusinessId");
            var isAdmin = User.IsInRole("Admin");
            if (!isAdmin && claimBusinessId != businessId)
                return Forbid();

            if (dto.From >= dto.To)
                return BadRequest(new { Message = "Invalid date range. 'from' must be before 'to'." });

            try
            {
                var report = await _reportService.GenerateReportAsync(businessId, dto);
                return Ok(report);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (InvalidOperationException ex) when (ex.Message == "NO_DATA")
            {
                return Ok(new { Message = "No analysis data available for this period.", Report = (object?)null });
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(502, new { Message = "Report generation failed. Please try again.", Details = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Report generation failed.", Error = ex.Message });
            }
        }
    }
}
