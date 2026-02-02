using System;
using Domain_layer.Interfaces;
using Domain_layer.Models;
using Service_layer.DTOS.Subscription;
using Service_layer.Services_Interfaces;

namespace Service_layer.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IBusinessRepository _businessRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SubscriptionService(
            ISubscriptionRepository subscriptionRepository,
            IBusinessRepository businessRepository,
            IUnitOfWork unitOfWork)
        {
            _subscriptionRepository = subscriptionRepository;
            _businessRepository = businessRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Subscription>> GetAllAsync()
        {
            return await _subscriptionRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Subscription>> GetByBusinessIdAsync(string businessId)
        {
            return await _subscriptionRepository.GetByBusinessIdAsync(businessId);
        }

        public async Task<Subscription?> GetByIdAsync(string id)
        {
            return await _subscriptionRepository.GetByIdAsync(id);
        }

        public async Task<Subscription?> GetActiveSubscriptionAsync(string businessId)
        {
            return await _subscriptionRepository.GetActiveSubscriptionAsync(businessId);
        }

        public async Task<Subscription> CreateAsync(SubscriptionCreateDTO dto)
        {
            var business = await _businessRepository.GetByIdAsync(dto.BusinessId);
            if (business == null)
                throw new ArgumentException($"Business with id '{dto.BusinessId}' not found.");

            var subscription = new Subscription
            {
                Id = Guid.NewGuid().ToString(),
                SubscriptionId = Guid.NewGuid().ToString(),
                PlanName = dto.PlanName,
                Price = dto.Price,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddMonths(1), // Default 1 month
                IsActive = true,
                BusinessId = dto.BusinessId
            };

            await _subscriptionRepository.AddAsync(subscription);
            await _unitOfWork.CompleteAsync();
            return subscription;
        }

        public async Task<Subscription?> RenewAsync(string id, SubscriptionRenewDTO dto)
        {
            var subscription = await _subscriptionRepository.GetByIdAsync(id);
            if (subscription == null) return null;

            subscription.EndDate = dto.NewEndDate;
            subscription.IsActive = true;

            _subscriptionRepository.Update(subscription);
            await _unitOfWork.CompleteAsync();
            return subscription;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var subscription = await _subscriptionRepository.GetByIdAsync(id);
            if (subscription == null) return false;

            _subscriptionRepository.Delete(subscription);
            await _unitOfWork.CompleteAsync();
            return true;
        }
    }
}

