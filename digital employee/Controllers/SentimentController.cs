using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service_layer.DTOS.Sentiment;
using Service_layer.Mapping;
using Service_layer.Services_Interfaces;

namespace digital_employee.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SentimentController : ControllerBase
    {
        private readonly ISentimentService _sentimentService;

        public SentimentController(ISentimentService sentimentService)
        {
            _sentimentService = sentimentService;
        }

        // GET: api/Sentiment
        [HttpGet]
        [Authorize(Policy = "AgentOrOwnerOrAdmin")]
        public async Task<IActionResult> GetAll()
        {
            var sentiments = await _sentimentService.GetAllAsync();
            return Ok(sentiments.ToDtoList());
        }

        // GET: api/Sentiment/message/{messageId}
        [HttpGet("message/{messageId}")]
        [Authorize(Policy = "AgentOrOwnerOrAdmin")]
        public async Task<IActionResult> GetByMessageId(string messageId)
        {
            var sentiments = await _sentimentService.GetByMessageIdAsync(messageId);
            return Ok(sentiments.ToDtoList());
        }

        // GET: api/Sentiment/business/{businessId}
        [HttpGet("business/{businessId}")]
        [Authorize(Policy = "OwnerOrAdmin")]
        public async Task<IActionResult> GetByBusinessId(string businessId)
        {
            var sentiments = await _sentimentService.GetByBusinessIdAsync(businessId);
            return Ok(sentiments.ToDtoList());
        }

        // GET: api/Sentiment/{id}
        [HttpGet("{id}")]
        [Authorize(Policy = "AgentOrOwnerOrAdmin")]
        public async Task<IActionResult> GetById(string id)
        {
            var sentiment = await _sentimentService.GetByIdAsync(id);
            if (sentiment == null)
                return NotFound(new { Message = $"Sentiment with id '{id}' not found." });

            return Ok(sentiment.ToDto());
        }
    }
}

