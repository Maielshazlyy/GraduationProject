using Domain_layer.Models;

namespace Domain_layer.Interfaces
{
    public interface IOwnerChatMessageRepository : IRepository<OwnerChatMessage>
    {
        Task<IEnumerable<OwnerChatMessage>> GetByBusinessIdAsync(string businessId);
        Task DeleteAllByBusinessIdAsync(string businessId);
    }
}
