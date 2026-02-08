using Domain_layer.Interfaces;
using Domain_layer.Models;
using Service_layer.Services_Interfaces;

namespace Service_layer.Services
{
    public class SentimentService : ISentimentService
    {
        private readonly ISentimentRepository _sentimentRepository;

        public SentimentService(ISentimentRepository sentimentRepository)
        {
            _sentimentRepository = sentimentRepository;
        }

        public async Task<IEnumerable<Sentiment>> GetAllAsync()
        {
            return await _sentimentRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Sentiment>> GetByMessageIdAsync(string messageId)
        {
            return await _sentimentRepository.GetByMessageIdAsync(messageId);
        }

        public async Task<IEnumerable<Sentiment>> GetByBusinessIdAsync(string businessId)
        {
            return await _sentimentRepository.GetByBusinessIdAsync(businessId);
        }

        public async Task<Sentiment?> GetByIdAsync(string id)
        {
            return await _sentimentRepository.GetByIdAsync(id);
        }
    }
}

