using Microsoft.AspNetCore.Mvc;
using Domain_layer.Interfaces;
using Service_layer.DTOS.CallSummary;
using System.Text.Json;

namespace digital_employee.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CallSummaryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CallSummaryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>Get call summary by interactionId or callId.</summary>
        [HttpGet("{callId}")]
        public async Task<IActionResult> GetByCallId(string callId)
        {
            var summary = await _unitOfWork.CallSummaries.GetByCallIdAsync(callId)
                       ?? await _unitOfWork.CallSummaries.GetByInteractionIdAsync(callId);

            if (summary == null)
                return NotFound(new { message = "Call summary not found." });

            AudioUrlsResponseDTO? audioUrls = null;
            if (!string.IsNullOrEmpty(summary.AudioUrlsJson) && summary.AudioUrlsJson != "{}")
            {
                try
                {
                    audioUrls = JsonSerializer.Deserialize<AudioUrlsResponseDTO>(
                        summary.AudioUrlsJson,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                catch { }
            }

            return Ok(new CallSummaryResponseDTO
            {
                Id              = summary.Id,
                InteractionId   = summary.InteractionId,
                BusinessId      = summary.BusinessId,
                CallId          = summary.CallId,
                StartTime       = summary.StartTime,
                EndTime         = summary.EndTime,
                DurationSeconds = summary.DurationSeconds,
                MessagesCount   = summary.MessagesCount,
                FullTranscript  = summary.FullTranscript,
                Summary         = summary.Summary,
                SummaryAr       = summary.SummaryAr,
                SentimentScore  = summary.SentimentScore,
                SentimentLabel  = summary.SentimentLabel,
                EscalationRequired = summary.EscalationRequired,
                EscalationReason   = summary.EscalationReason,
                AnalyzedAt      = summary.AnalyzedAt,
                CreatedAt       = summary.CreatedAt,
                AudioUrls       = audioUrls
            });
        }

        /// <summary>Get all call summaries for a business.</summary>
        [HttpGet("business/{businessId}")]
        public async Task<IActionResult> GetByBusiness(string businessId)
        {
            var summaries = await _unitOfWork.CallSummaries.GetByBusinessIdAsync(businessId);

            var result = summaries.Select(summary =>
            {
                AudioUrlsResponseDTO? audioUrls = null;
                if (!string.IsNullOrEmpty(summary.AudioUrlsJson) && summary.AudioUrlsJson != "{}")
                {
                    try
                    {
                        audioUrls = JsonSerializer.Deserialize<AudioUrlsResponseDTO>(
                            summary.AudioUrlsJson,
                            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    }
                    catch { }
                }

                return new CallSummaryResponseDTO
                {
                    Id              = summary.Id,
                    InteractionId   = summary.InteractionId,
                    BusinessId      = summary.BusinessId,
                    CallId          = summary.CallId,
                    StartTime       = summary.StartTime,
                    EndTime         = summary.EndTime,
                    DurationSeconds = summary.DurationSeconds,
                    MessagesCount   = summary.MessagesCount,
                    FullTranscript  = summary.FullTranscript,
                    Summary         = summary.Summary,
                    SummaryAr       = summary.SummaryAr,
                    SentimentScore  = summary.SentimentScore,
                    SentimentLabel  = summary.SentimentLabel,
                    EscalationRequired = summary.EscalationRequired,
                    EscalationReason   = summary.EscalationReason,
                    AnalyzedAt      = summary.AnalyzedAt,
                    CreatedAt       = summary.CreatedAt,
                    AudioUrls       = audioUrls
                };
            });

            return Ok(result);
        }
    }
}
