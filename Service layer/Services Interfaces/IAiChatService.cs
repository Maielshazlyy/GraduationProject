using System.Threading.Tasks;
using Service_layer.DTOS.AiChat;

namespace Service_layer.Services_Interfaces
{
    public interface IAiChatService
    {
        Task<AiChatResponseDTO> SendMessageAsync(AiChatRequestDTO request);
    }
}
