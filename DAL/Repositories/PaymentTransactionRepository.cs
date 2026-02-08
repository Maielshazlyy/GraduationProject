using DAL.Context;
using Domain_layer.Interfaces;
using Domain_layer.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class PaymentTransactionRepository : Repository<PaymentTransaction>, IPaymentTransactionRepository
    {
        private readonly AppDbContext _context;
        
        public PaymentTransactionRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<PaymentTransaction>> GetBySubscriptionIdAsync(string subscriptionId)
        {
            return await FindAsync(pt => pt.SubscriptionId == subscriptionId);
        }
        
        public async Task<IEnumerable<PaymentTransaction>> GetByBusinessIdAsync(string businessId)
        {
            return await _context.PaymentTransactions
                .Where(pt => pt.Subscription != null && pt.Subscription.BusinessId == businessId)
                .ToListAsync();
        }
    }
}

