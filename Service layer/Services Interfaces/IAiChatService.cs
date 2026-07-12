using System.Threading.Tasks;
using Service_layer.DTOS.AiChat;

namespace Service_layer.Services_Interfaces
{
    /// <summary>
    /// Sends customer chat messages to the external AI service and returns its reply.
    ///
    /// Knowledge base priming is handled separately by IAiKnowledgeSyncService —
    /// the AI is pre-loaded with business data before any conversation starts.
    /// </summary>
    public interface IAiChatService
    {
        /// <summary>Sends a text message (WebChat) to the AI.</summary>
        Task<AiChatResponseDTO> SendMessageAsync(AiChatRequestDTO request);
    }
}
