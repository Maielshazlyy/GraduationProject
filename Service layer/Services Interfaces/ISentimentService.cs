using Domain_layer.Models;
using Service_layer.DTOS.Sentiment;

namespace Service_layer.Services_Interfaces
{
    public interface ISentimentService
    {
        Task<IEnumerable<Sentiment>> GetAllAsync();
        Task<IEnumerable<Sentiment>> GetByMessageIdAsync(string messageId);
        Task<IEnumerable<Sentiment>> GetByBusinessIdAsync(string businessId);
        Task<Sentiment?> GetByIdAsync(string id);
    }
}

