using DAL.Context;
using Domain_layer.Interfaces;
using Domain_layer.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class FeedbackRepository : Repository<Feedback>, IFeedbackRepository
    {
        private readonly AppDbContext _context;
        
        public FeedbackRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<Feedback>> GetByCustomerIdAsync(string customerId)
        {
            return await FindAsync(f => f.CustomerId == customerId);
        }
        
        public async Task<IEnumerable<Feedback>> GetByTicketIdAsync(string ticketId)
        {
            return await FindAsync(f => f.TicketId == ticketId);
        }
        
        public async Task<double> GetAverageRatingAsync(string businessId)
        {
            var feedbacks = await _context.Feedbacks
                .Where(f => f.Ticket != null && f.Ticket.BusinessId == businessId)
                .Select(f => f.Rating)
                .ToListAsync();
            
            if (feedbacks.Count == 0) return 0;
            
            return feedbacks.Average();
        }
    }
}

