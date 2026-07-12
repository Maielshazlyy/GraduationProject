using Domain_layer.Models;
using Service_layer.DTOS.Message;

namespace Service_layer.Services_Interfaces
{
    public interface IMessageService
    {
        Task<IEnumerable<Message>> GetAllAsync();
        Task<IEnumerable<Message>> GetByInteractionIdAsync(string interactionId);
        Task<Message?> GetByIdAsync(string id);
        Task<Message> CreateAsync(MessageCreateDTO dto);
        Task<bool> DeleteAsync(string id);
    }
}

