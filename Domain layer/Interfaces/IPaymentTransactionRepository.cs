using Domain_layer.Models;

namespace Domain_layer.Interfaces
{
    public interface IPaymentTransactionRepository : IRepository<PaymentTransaction>
    {
        Task<IEnumerable<PaymentTransaction>> GetBySubscriptionIdAsync(string subscriptionId);
        Task<IEnumerable<PaymentTransaction>> GetByBusinessIdAsync(string businessId);
    }
}

