using Domain_layer.Models;
using Service_layer.DTOS.KnowledgeBase;

namespace Service_layer.Services_Interfaces
{
    public interface IKnowledgeBaseService
    {
        Task<IEnumerable<KnowledgeBase>> GetAllAsync();
        Task<IEnumerable<KnowledgeBase>> GetByBusinessIdAsync(string businessId);
        Task<IEnumerable<KnowledgeBase>> GetFAQsByBusinessIdAsync(string businessId); // Only FAQs (public)
        Task<IEnumerable<KnowledgeBase>> GetKnowledgeBaseByBusinessIdAsync(string businessId); // Only KnowledgeBase (internal)
        Task<KnowledgeBase?> GetByIdAsync(string id);
        Task<KnowledgeBase> CreateAsync(KnowledgeBaseCreateDTO dto);
        Task<KnowledgeBase?> UpdateAsync(string id, KnowledgeBaseCreateDTO dto);
        Task<bool> DeleteAsync(string id);
    }
}

