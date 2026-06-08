using System.Text.Json;
using Domain_layer.Interfaces;
using Domain_layer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service_layer.DTOS.CallSummary;

namespace digital_employee.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class CallSummaryController : ControllerBase
    {
        private readonly IUnitOfWork _uow;

        public CallSummaryController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpPost("summary")]
        public async Task<IActionResult> ReceiveSummary([FromBody] CallSummaryPayloadDTO payload)
        {
            if (payload?.CallData == null || payload.Analysis == null)
                return BadRequest(new { message = "Invalid payload." });

            var options = new JsonSerializerOptions { WriteIndented = false };

            var summary = new CallSummary
            {
                CallId            = payload.CallData.CallId,
                StartTime         = payload.CallData.StartTime,
                EndTime           = payload.CallData.EndTime,
                DurationSeconds   = payload.CallData.DurationSeconds,
                MessagesCount     = payload.CallData.MessagesCount,
                FullTranscript    = payload.CallData.FullTranscript,
                MessagesJson      = JsonSerializer.Serialize(payload.CallData.Messages, options),
                AudioFilesJson    = JsonSerializer.Serialize(payload.CallData.AudioFiles, options),
                AudioInfoJson     = JsonSerializer.Serialize(payload.CallData.AudioInfo, options),
                Summary           = payload.Analysis.Summary,
                SummaryAr         = payload.Analysis.SummaryAr,
                SentimentScore    = payload.Analysis.OverallSentiment?.Score ?? 0,
                SentimentLabel    = payload.Analysis.OverallSentiment?.Label ?? string.Empty,
                MainTopicsJson    = JsonSerializer.Serialize(payload.Analysis.MainTopics, options),
                IntentsDetectedJson  = JsonSerializer.Serialize(payload.Analysis.IntentsDetected, options),
                ActionsPerformedJson = JsonSerializer.Serialize(payload.Analysis.ActionsPerformed, options),
                KeyMomentsJson    = JsonSerializer.Serialize(payload.Analysis.KeyMoments, options),
                EscalationRequired = payload.Analysis.EscalationRequired,
                EscalationReason  = payload.Analysis.EscalationReason,
                AnalyzedAt        = payload.Analysis.AnalyzedAt,
                QueuedAt          = payload.QueuedAt,
            };

            await _uow.CallSummaries.AddAsync(summary);
            await _uow.CompleteAsync();

            return Ok(new { message = "Summary saved.", id = summary.Id });
        }
    }
}
