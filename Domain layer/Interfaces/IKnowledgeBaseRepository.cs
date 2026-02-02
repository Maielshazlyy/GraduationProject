using Domain_layer.Models;

namespace Domain_layer.Interfaces
{
    public interface IKnowledgeBaseRepository : IRepository<KnowledgeBase>
    {
        Task<IEnumerable<KnowledgeBase>> GetByBusinessIdAsync(string businessId);
        Task<IEnumerable<KnowledgeBase>> SearchAsync(string businessId, string searchTerm);
    }
}

