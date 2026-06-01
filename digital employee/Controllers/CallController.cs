using Microsoft.AspNetCore.Mvc;

namespace digital_employee.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CallController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<CallController> _logger;

        public CallController(IHttpClientFactory httpClientFactory, ILogger<CallController> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        [HttpPost("startcall")]
        public IActionResult StartCall()
        {
            var response = new { linkmeeting = "https://example.com" };

            _ = Task.Run(async () =>
            {
                try
                {
                    var client = _httpClientFactory.CreateClient();
                    await client.PostAsync("https://ai.backend/triggercall", null);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to trigger call at ai.backend/triggercall");
                }
            });

            return Ok(response);
        }
    }
}
