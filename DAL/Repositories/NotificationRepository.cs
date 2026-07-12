using DAL.Context;
using Domain_layer.Interfaces;
using Domain_layer.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class NotificationRepository : Repository<Notification>, INotificationRepository
    {
        private readonly AppDbContext _context;
        
        public NotificationRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<Notification>> GetByBusinessIdAsync(string businessId)
        {
            return await FindAsync(n => n.BusinessId == businessId);
        }
        
        public async Task<IEnumerable<Notification>> GetByUserIdAsync(string userId)
        {
            return await FindAsync(n => n.UserId == userId);
        }
        
        public async Task<IEnumerable<Notification>> GetUnreadByUserIdAsync(string userId)
        {
            return await FindAsync(n => n.UserId == userId && !n.IsRead);
        }
    }
}

