using System.Threading.Tasks;
using Service_layer.DTOS.AiOwnerChat;

namespace Service_layer.Services_Interfaces
{
    public interface IAiOwnerChatService
    {
        Task<AiOwnerChatResponseDTO> SendMessageAsync(AiOwnerChatRequestDTO request);
        Task<AiOwnerReportSyncResponseDTO> SyncReportAsync(AiOwnerReportSyncRequestDTO request);
    }
}
