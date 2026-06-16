using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace digital_employee.Controllers
{
    /// <summary>
    /// Receives raw call/chat audio recordings (.wav) from the AI backend.
    /// The AI uploads the recorded conversation as multipart/form-data so the
    /// actual audio bytes are stored on the API (not just JSON references).
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class CallRecordingController : ControllerBase
    {
        // Allow files up to 50 MB (a few minutes of WAV audio).
        private const long MaxFileSizeBytes = 50L * 1024 * 1024;

        private static readonly string[] AllowedExtensions = { ".wav" };
        private static readonly string[] AllowedContentTypes =
        {
            "audio/wav", "audio/wave", "audio/x-wav", "application/octet-stream"
        };

        private static readonly string[] AllowedPhotoExtensions = { ".jpg", ".jpeg", ".png", ".webp" };
        private static readonly string[] AllowedPhotoContentTypes =
        {
            "image/jpeg", "image/png", "image/webp", "application/octet-stream"
        };

        private readonly IWebHostEnvironment _env;
        private readonly ILogger<CallRecordingController> _logger;

        public CallRecordingController(IWebHostEnvironment env, ILogger<CallRecordingController> logger)
        {
            _env = env;
            _logger = logger;
        }

        /// <summary>
        /// Upload a call recording as multipart/form-data.
        /// Form fields:
        ///   - file     : the .wav file (required)
        ///   - callId   : the call/session id this recording belongs to (required)
        ///   - speaker  : optional ("customer" | "agent" | "stereo")
        ///   - photo    : optional image snapshot (jpg/png/webp); its URL is returned as photoUrl
        /// </summary>
        [HttpPost("upload")]
        [RequestSizeLimit(MaxFileSizeBytes)]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Upload(
            [FromForm] IFormFile file,
            [FromForm] string callId,
            [FromForm] string? speaker = null,
            [FromForm] IFormFile? photo = null)
        {
            if (file == null || file.Length == 0)
                return BadRequest(new { message = "A non-empty 'file' is required." });

            if (string.IsNullOrWhiteSpace(callId))
                return BadRequest(new { message = "'callId' is required." });

            if (file.Length > MaxFileSizeBytes)
                return BadRequest(new { message = $"File exceeds the {MaxFileSizeBytes / (1024 * 1024)} MB limit." });

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!AllowedExtensions.Contains(extension))
                return BadRequest(new { message = "Only .wav files are accepted." });

            if (!AllowedContentTypes.Contains(file.ContentType.ToLowerInvariant()))
                return BadRequest(new { message = $"Unsupported content type '{file.ContentType}'." });

            // Sanitize callId so it can't escape the storage folder.
            var safeCallId = string.Concat(callId.Where(c => char.IsLetterOrDigit(c) || c is '-' or '_'));
            if (string.IsNullOrEmpty(safeCallId))
                return BadRequest(new { message = "'callId' contains no valid characters." });

            var contentRoot = _env.ContentRootPath;
            var storageDir = Path.Combine(contentRoot, "Recordings", safeCallId);
            Directory.CreateDirectory(storageDir);

            var prefix = string.IsNullOrWhiteSpace(speaker) ? "recording" : speaker.Trim().ToLowerInvariant();
            var storedFileName = $"{prefix}_{DateTime.UtcNow:yyyyMMddHHmmss}_{Guid.NewGuid():N}.wav";
            var fullPath = Path.Combine(storageDir, storedFileName);

            try
            {
                await using var stream = System.IO.File.Create(fullPath);
                await file.CopyToAsync(stream);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to store call recording for call {CallId}", safeCallId);
                return StatusCode(500, new { message = "Failed to store the recording." });
            }

            _logger.LogInformation("Stored recording {File} ({Bytes} bytes) for call {CallId}",
                storedFileName, file.Length, safeCallId);

            var downloadUrl = Url.Action(nameof(Download), "CallRecording",
                new { callId = safeCallId, fileName = storedFileName }, Request.Scheme);

            // Optional photo snapshot.
            string? photoFileName = null;
            string? photoUrl = null;
            if (photo != null && photo.Length > 0)
            {
                var photoExt = Path.GetExtension(photo.FileName).ToLowerInvariant();
                if (!AllowedPhotoExtensions.Contains(photoExt))
                    return BadRequest(new { message = "Photo must be a .jpg, .jpeg, .png or .webp file." });

                if (!AllowedPhotoContentTypes.Contains(photo.ContentType.ToLowerInvariant()))
                    return BadRequest(new { message = $"Unsupported photo content type '{photo.ContentType}'." });

                photoFileName = $"photo_{DateTime.UtcNow:yyyyMMddHHmmss}_{Guid.NewGuid():N}{photoExt}";
                var photoPath = Path.Combine(storageDir, photoFileName);
                try
                {
                    await using var photoStream = System.IO.File.Create(photoPath);
                    await photo.CopyToAsync(photoStream);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to store photo for call {CallId}", safeCallId);
                    return StatusCode(500, new { message = "Failed to store the photo." });
                }

                photoUrl = Url.Action(nameof(Download), "CallRecording",
                    new { callId = safeCallId, fileName = photoFileName }, Request.Scheme);
            }

            return Ok(new
            {
                message = "Recording uploaded.",
                callId = safeCallId,
                speaker,
                fileName = storedFileName,
                sizeBytes = file.Length,
                downloadUrl,
                photoFileName,
                photoUrl
            });
        }

        /// <summary>
        /// Download a previously uploaded recording.
        /// </summary>
        [HttpGet("{callId}/{fileName}")]
        public IActionResult Download(string callId, string fileName)
        {
            // Reject any traversal attempt in the path segments.
            if (fileName.Contains("..") || callId.Contains("..") ||
                fileName.Contains('/') || fileName.Contains('\\'))
                return BadRequest(new { message = "Invalid file path." });

            var safeCallId = string.Concat(callId.Where(c => char.IsLetterOrDigit(c) || c is '-' or '_'));
            var fullPath = Path.Combine(_env.ContentRootPath, "Recordings", safeCallId, fileName);

            if (!System.IO.File.Exists(fullPath))
                return NotFound(new { message = "Recording not found." });

            var contentType = Path.GetExtension(fileName).ToLowerInvariant() switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".webp" => "image/webp",
                _ => "audio/wav"
            };

            var stream = System.IO.File.OpenRead(fullPath);
            return File(stream, contentType, fileName);
        }
    }
}
