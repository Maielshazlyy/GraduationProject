using DAL.Context;
using Domain_layer.Interfaces;
using Domain_layer.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class OwnerChatMessageRepository : Repository<OwnerChatMessage>, IOwnerChatMessageRepository
    {
        private readonly AppDbContext _dbContext;

        public OwnerChatMessageRepository(AppDbContext context) : base(context)
        {
            _dbContext = context;
        }

        public async Task<IEnumerable<OwnerChatMessage>> GetByBusinessIdAsync(string businessId)
        {
            return await _dbContext.OwnerChatMessages
                .Where(m => m.BusinessId == businessId)
                .OrderBy(m => m.SentAt)
                .ToListAsync();
        }

        public async Task DeleteAllByBusinessIdAsync(string businessId)
        {
            await _dbContext.OwnerChatMessages
                .Where(m => m.BusinessId == businessId)
                .ExecuteDeleteAsync();
        }
    }
}
