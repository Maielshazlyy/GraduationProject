using Domain_layer.Models;
using Service_layer.DTOS.KnowledgeBase;

namespace Service_layer.Services_Interfaces
{
    public interface IKnowledgeBaseService
    {
        Task<IEnumerable<KnowledgeBase>> GetAllAsync();
        Task<IEnumerable<KnowledgeBase>> GetByBusinessIdAsync(string businessId);
        Task<KnowledgeBase?> GetByIdAsync(string id);
        Task<KnowledgeBase> CreateAsync(KnowledgeBaseCreateDTO dto);
        Task<KnowledgeBase?> UpdateAsync(string id, KnowledgeBaseCreateDTO dto);
        Task<bool> DeleteAsync(string id);
    }
}

