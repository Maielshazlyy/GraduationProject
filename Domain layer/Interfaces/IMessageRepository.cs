using Domain_layer.Models;

namespace Domain_layer.Interfaces
{
    public interface IMessageRepository : IRepository<Message>
    {
        Task<IEnumerable<Message>> GetByInteractionIdAsync(string interactionId);
        Task<IEnumerable<Message>> GetByUserIdAsync(string userId);
    }
}

