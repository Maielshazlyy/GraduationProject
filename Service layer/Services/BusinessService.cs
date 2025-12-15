using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain_layer.Interfaces;
using Domain_layer.Models;
using Service_layer.DTOS.Business;
using Service_layer.Services_Interfaces;
using Service_layer.Mapping;

namespace Service_layer.Services
{
   public class BusinessService:IBusinessService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Setting> _settingsRepository;
        private readonly IRepository<KnowledgeBase> _knowledgeBaseRepository;
        private readonly IRepository<Subscription> _subscriptionRepository;
        private readonly IRepository<PaymentTransaction> _paymentRepository;

        public BusinessService(
            IUnitOfWork unitOfWork,
            IRepository<Setting> settingsRepository,
            IRepository<KnowledgeBase> knowledgeBaseRepository,
            IRepository<Subscription> subscriptionRepository,
            IRepository<PaymentTransaction> paymentRepository)
        {
            _unitOfWork = unitOfWork;
            _settingsRepository = settingsRepository;
            _knowledgeBaseRepository = knowledgeBaseRepository;
            _subscriptionRepository = subscriptionRepository;
            _paymentRepository = paymentRepository;
        }
        public async Task<IEnumerable<Business>> GetAllAsync()
        {
            return await _unitOfWork.Businesses.GetAllAsync();
        }

        public async Task<Business?> GetByIdAsync(string id)
        {
            return await _unitOfWork.Businesses.GetByIdAsync(id);
        }

        public async Task<Business> CreateAsync(Business business)
        {
            await _unitOfWork.Businesses.AddAsync(business);
            await _unitOfWork.CompleteAsync();
            return business;
        }

        public async Task<Business?> UpdateAsync(string id, Business business)
        {
            var existing = await _unitOfWork.Businesses.GetByIdAsync(id);
            if (existing == null) return null;

            existing.Name = business.Name;
            existing.Type = business.Type;
            existing.Address = business.Address;
            existing.Phone = business.Phone;

            _unitOfWork.Businesses.Update(existing);
            await _unitOfWork.CompleteAsync();

            return existing;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var business = await _unitOfWork.Businesses.GetByIdAsync(id);
            if (business == null) return false;

            _unitOfWork.Businesses.Delete(business);
            await _unitOfWork.CompleteAsync();

            return true;
        }

       
        // Full onboarding for a new restaurant business.
       
        public async Task<Business> OnboardRestaurantAsync(BusinessOnboardingDTO dto)
        {
            // 1) Create Business (BusinessId is Guid by default in entity)
            var business = dto.ToBusiness();
            await _unitOfWork.Businesses.AddAsync(business);

            // 2) Create Setting (agent configuration)
            var setting = dto.ToSetting(business.Id);
            await _settingsRepository.AddAsync(setting);

            // 3) Seed Knowledge Base
            var kbEntities = dto.ToKnowledgeBaseEntities(business.Id);
            foreach (var kb in kbEntities)
            {
                await _knowledgeBaseRepository.AddAsync(kb);
            }

            // 4) Create Subscription
            var subscription = dto.ToSubscription(business.Id);
            await _subscriptionRepository.AddAsync(subscription);

            // 5) Create initial PaymentTransaction (status set to Success for now)
            var payment = dto.ToPaymentTransaction(subscription.Id);

            // NOTE: Card details from dto are not stored for security reasons in this simple example.
            await _paymentRepository.AddAsync(payment);

            // 6) Commit everything
            await _unitOfWork.CompleteAsync();

            return business;
        }
    }
}
