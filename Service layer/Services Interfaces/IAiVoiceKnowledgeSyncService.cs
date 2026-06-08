using System.Threading.Tasks;

namespace Service_layer.Services_Interfaces
{
    public interface IAiVoiceKnowledgeSyncService
    {
        Task SyncBusinessAsync(string businessId);
    }
}
