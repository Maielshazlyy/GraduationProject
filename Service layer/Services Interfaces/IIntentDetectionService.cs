using System.Collections.Generic;
using System.Threading.Tasks;
using Service_layer.DTOS.Chat;

namespace Service_layer.Services_Interfaces
{
    /// <summary>
    /// Abstraction for AI-based intent detection.
    /// Backend sends recent messages, service returns structured intent result.
    /// </summary>
    public interface IIntentDetectionService
    {
        Task<DetectedIntentResultDTO> DetectIntentAsync(
            string businessId,
            string interactionId,
            IEnumerable<string> recentMessages);
    }
}


