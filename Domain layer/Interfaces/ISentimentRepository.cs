using Domain_layer.Models;

namespace Domain_layer.Interfaces
{
    public interface ISentimentRepository : IRepository<Sentiment>
    {
        Task<IEnumerable<Sentiment>> GetByMessageIdAsync(string messageId);
        Task<IEnumerable<Sentiment>> GetByBusinessIdAsync(string businessId);
    }
}

