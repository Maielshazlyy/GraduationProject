using Domain_layer.Models;
using Service_layer.DTOS.PaymentTranscation;

namespace Service_layer.Services_Interfaces
{
    public interface IPaymentTransactionService
    {
        Task<IEnumerable<PaymentTransaction>> GetAllAsync();
        Task<IEnumerable<PaymentTransaction>> GetBySubscriptionIdAsync(string subscriptionId);
        Task<IEnumerable<PaymentTransaction>> GetByBusinessIdAsync(string businessId);
        Task<PaymentTransaction?> GetByIdAsync(string id);
        Task<PaymentTransaction> CreateAsync(PaymentTransactionCreateDTO dto);
        Task<bool> DeleteAsync(string id);
    }
}

