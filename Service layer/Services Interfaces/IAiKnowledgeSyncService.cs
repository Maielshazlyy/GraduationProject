using System.Threading.Tasks;

namespace Service_layer.Services_Interfaces
{
    /// <summary>
    /// Pushes a business's full knowledge base (menu + FAQs + policies) to the AI service.
    ///
    /// Called whenever business data changes so the AI keeps its index up to date.
    /// Triggers: business created, menu item added/updated/deleted, knowledge base changed.
    /// </summary>
    public interface IAiKnowledgeSyncService
    {
        /// <summary>
        /// Fetches the latest menu and knowledge base for the given business from the DB,
        /// then pushes it to the AI via POST /api/v1/business/knowledge-base/sync.
        /// </summary>
        Task SyncBusinessAsync(string businessId);
    }
}
