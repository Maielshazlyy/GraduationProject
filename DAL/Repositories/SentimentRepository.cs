using DAL.Context;
using Domain_layer.Interfaces;
using Domain_layer.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class SentimentRepository : Repository<Sentiment>, ISentimentRepository
    {
        private readonly AppDbContext _context;
        
        public SentimentRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<Sentiment>> GetByMessageIdAsync(string messageId)
        {
            return await FindAsync(s => s.MessageId == messageId);
        }
        
        public async Task<IEnumerable<Sentiment>> GetByBusinessIdAsync(string businessId)
        {
            return await _context.Sentiments
                .Where(s => s.Message != null && 
                           s.Message.Interaction != null && 
                           s.Message.Interaction.BusinessId == businessId)
                .ToListAsync();
        }
    }
}

