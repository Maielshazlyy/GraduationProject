using System;
using Domain_layer.Interfaces;
using Domain_layer.Models;
using Domain_layer.enums;
using Service_layer.DTOS.PaymentTranscation;
using Service_layer.Services_Interfaces;

namespace Service_layer.Services
{
    public class PaymentTransactionService : IPaymentTransactionService
    {
        private readonly IPaymentTransactionRepository _paymentRepository;
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentTransactionService(
            IPaymentTransactionRepository paymentRepository,
            ISubscriptionRepository subscriptionRepository,
            IUnitOfWork unitOfWork)
        {
            _paymentRepository = paymentRepository;
            _subscriptionRepository = subscriptionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<PaymentTransaction>> GetAllAsync()
        {
            return await _paymentRepository.GetAllAsync();
        }

        public async Task<IEnumerable<PaymentTransaction>> GetBySubscriptionIdAsync(string subscriptionId)
        {
            return await _paymentRepository.GetBySubscriptionIdAsync(subscriptionId);
        }

        public async Task<IEnumerable<PaymentTransaction>> GetByBusinessIdAsync(string businessId)
        {
            return await _paymentRepository.GetByBusinessIdAsync(businessId);
        }

        public async Task<PaymentTransaction?> GetByIdAsync(string id)
        {
            return await _paymentRepository.GetByIdAsync(id);
        }

        public async Task<PaymentTransaction> CreateAsync(PaymentTransactionCreateDTO dto)
        {
            var subscription = await _subscriptionRepository.GetByIdAsync(dto.SubscriptionId);
            if (subscription == null)
                throw new ArgumentException($"Subscription with id '{dto.SubscriptionId}' not found.");

            if (!Enum.TryParse<PaymentMethod>(dto.PaymentMethod, out var paymentMethod))
                throw new ArgumentException($"Invalid payment method: {dto.PaymentMethod}");

            var payment = new PaymentTransaction
            {
                Id = Guid.NewGuid().ToString(),
                PaymentId = Guid.NewGuid().ToString(),
                SubscriptionId = dto.SubscriptionId,
                Amount = dto.Amount,
                PaymentMethod = paymentMethod,
                TransactionDate = DateTime.UtcNow,
                Status = PaymentStatus.Pending // Default status
            };

            await _paymentRepository.AddAsync(payment);
            await _unitOfWork.CompleteAsync();
            return payment;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var payment = await _paymentRepository.GetByIdAsync(id);
            if (payment == null) return false;

            _paymentRepository.Delete(payment);
            await _unitOfWork.CompleteAsync();
            return true;
        }
    }
}

