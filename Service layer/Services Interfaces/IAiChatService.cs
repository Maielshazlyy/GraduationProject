using System.Threading.Tasks;
using Service_layer.DTOS.AiChat;

namespace Service_layer.Services_Interfaces
{
    public interface IAiChatService
    {
        Task<AiChatResponseDTO> SendMessageAsync(AiChatRequestDTO request);

        /// <summary>
        /// Primes a new AI session with the business's menu + knowledge base.
        /// Called once when a conversation starts.
        /// </summary>
        Task InitSessionAsync(AiSessionInitDTO init);
    }
}
