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
            return await _context.Sentiments
                .Include(s => s.Message)
                    .ThenInclude(m => m.Interaction)
                        .ThenInclude(i => i.Customer)
                .Where(s => s.MessageId == messageId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Sentiment>> GetByBusinessIdAsync(string businessId)
        {
            return await _context.Sentiments
                .Include(s => s.Message)
                    .ThenInclude(m => m.Interaction)
                        .ThenInclude(i => i.Customer)
                .Where(s => s.Message != null &&
                           s.Message.Interaction != null &&
                           s.Message.Interaction.BusinessId == businessId)
                .ToListAsync();
        }
    }
}

