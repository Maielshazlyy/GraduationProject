using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Domain_layer.Models;
using Service_layer.DTOS.Chatbot;
using Service_layer.Services_Interfaces;

namespace digital_employee.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "OwnerOrAdmin")] // Only Business Owners and Admins can use chatbot
    public class ChatbotController : ControllerBase
    {
        private readonly IChatbotService _chatbotService;
        private readonly UserManager<User> _userManager;

        public ChatbotController(IChatbotService chatbotService, UserManager<User> userManager)
        {
            _chatbotService = chatbotService;
            _userManager = userManager;
        }

        // POST: api/Chatbot/ask
        [HttpPost("ask")]
        public async Task<IActionResult> AskQuestion([FromBody] ChatbotRequestDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                // Get BusinessId from the authenticated user
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized(new { Message = "User not found." });

                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    return Unauthorized(new { Message = "User not found." });

                if (string.IsNullOrEmpty(user.BusinessId))
                {
                    return BadRequest(new { Message = "User is not linked to a business. Please create or link to a business first." });
                }

                var response = await _chatbotService.AskQuestionAsync(user.BusinessId, request);
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while processing your question.", Error = ex.Message });
            }
        }

        // GET: api/Chatbot/suggestions
        [HttpGet("suggestions")]
        public IActionResult GetSuggestions()
        {
            var suggestions = new List<string>
            {
                "What is my business performance overview?",
                "What is my total revenue and sales?",
                "What is my customer satisfaction rate?",
                "What is my sentiment analysis?",
                "How many tickets do I have?",
                "What are recommendations to improve sales?",
                "How can I improve customer satisfaction?",
                "What is my average order value?",
                "How many new customers did I get?",
                "What is my ticket resolution time?"
            };

            return Ok(new { Suggestions = suggestions });
        }
    }
}

