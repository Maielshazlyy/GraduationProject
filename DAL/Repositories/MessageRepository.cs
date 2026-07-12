using DAL.Context;
using Domain_layer.Interfaces;
using Domain_layer.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class MessageRepository : Repository<Message>, IMessageRepository
    {
        private readonly AppDbContext _context;
        
        public MessageRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        
        
        public async Task<IEnumerable<Message>> GetByInteractionIdAsync(string interactionId)
        {
            return await FindAsync(m => m.InteractionId == interactionId);
        }
        
        public async Task<IEnumerable<Message>> GetByUserIdAsync(string userId)
        {
            return await FindAsync(m => m.UserId == userId);
        }
    }
}

