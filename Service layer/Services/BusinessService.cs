using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain_layer.Interfaces;
using Domain_layer.Models;
using Domain_layer.enums;
using Service_layer.DTOS.Business;
using Service_layer.Services_Interfaces;
using Service_layer.Mapping;

namespace Service_layer.Services
{
   public class BusinessService:IBusinessService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISettingRepository _settingsRepository;
        private readonly IKnowledgeBaseRepository _knowledgeBaseRepository;
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IPaymentTransactionRepository _paymentRepository;

        public BusinessService(
            IUnitOfWork unitOfWork,
            ISettingRepository settingsRepository,
            IKnowledgeBaseRepository knowledgeBaseRepository,
            ISubscriptionRepository subscriptionRepository,
            IPaymentTransactionRepository paymentRepository)
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

        public async Task<Business?> UpdateAsync(string id, BusinessUpdateDTO dto)
        {
            var existing = await _unitOfWork.Businesses.GetByIdAsync(id);
            if (existing == null) return null;

            // Basic Information (Partial Update)
            if (!string.IsNullOrWhiteSpace(dto.Name))
                existing.Name = dto.Name;
            if (!string.IsNullOrWhiteSpace(dto.Type))
                existing.Type = dto.Type;
            if (!string.IsNullOrWhiteSpace(dto.Address))
                existing.Address = dto.Address;
            if (!string.IsNullOrWhiteSpace(dto.Phone))
                existing.Phone = dto.Phone;

            // Contact Information
            if (dto.Email != null)
                existing.Email = dto.Email;
            if (dto.Website != null)
                existing.Website = dto.Website;
            if (dto.FacebookUrl != null)
                existing.FacebookUrl = dto.FacebookUrl;
            if (dto.InstagramUrl != null)
                existing.InstagramUrl = dto.InstagramUrl;

            // Location
            if (dto.City != null)
                existing.City = dto.City;
            if (dto.Country != null)
                existing.Country = dto.Country;
            if (dto.Latitude.HasValue)
                existing.Latitude = dto.Latitude;
            if (dto.Longitude.HasValue)
                existing.Longitude = dto.Longitude;

            // Restaurant Information
            if (dto.Description != null)
                existing.Description = dto.Description;
            if (dto.CuisineType != null)
                existing.CuisineType = dto.CuisineType;
            if (dto.PriceRange != null)
                existing.PriceRange = dto.PriceRange;
            if (dto.LogoUrl != null)
                existing.LogoUrl = dto.LogoUrl;
            if (dto.CoverImageUrl != null)
                existing.CoverImageUrl = dto.CoverImageUrl;

            // Features & Services
            if (dto.HasDelivery.HasValue)
                existing.HasDelivery = dto.HasDelivery.Value;
            if (dto.HasTakeout.HasValue)
                existing.HasTakeout = dto.HasTakeout.Value;
            if (dto.HasParking.HasValue)
                existing.HasParking = dto.HasParking.Value;
            if (dto.HasWiFi.HasValue)
                existing.HasWiFi = dto.HasWiFi.Value;
            if (dto.HasOutdoorSeating.HasValue)
                existing.HasOutdoorSeating = dto.HasOutdoorSeating.Value;
            if (dto.AcceptsReservations.HasValue)
                existing.AcceptsReservations = dto.AcceptsReservations.Value;

            // Payment Methods
            if (dto.PaymentMethods != null)
                existing.PaymentMethods = dto.PaymentMethods;

            // Status
            if (dto.IsActive.HasValue)
                existing.IsActive = dto.IsActive.Value;
            if (dto.IsVerified.HasValue)
                existing.IsVerified = dto.IsVerified.Value;

            // Update Working Hours if provided
            if (dto.WorkingHours != null && dto.WorkingHours.Any())
            {
                // Delete existing working hours for this business
                var existingHours = await _unitOfWork.WorkingHours.GetByBusinessIdAsync(id);
                foreach (var wh in existingHours)
                {
                    _unitOfWork.WorkingHours.Delete(wh);
                }

                // Add new working hours
                foreach (var whDto in dto.WorkingHours)
                {
                    var wh = new WorkingHours
                    {
                        WorkingHoursId = Guid.NewGuid().ToString(),
                        DayOfWeek = whDto.DayOfWeek,
                        OpenTime = !string.IsNullOrWhiteSpace(whDto.OpenTime) && TimeSpan.TryParse(whDto.OpenTime, out var openTime) ? openTime : null,
                        CloseTime = !string.IsNullOrWhiteSpace(whDto.CloseTime) && TimeSpan.TryParse(whDto.CloseTime, out var closeTime) ? closeTime : null,
                        IsClosed = whDto.IsClosed,
                        BusinessId = id
                    };
                    await _unitOfWork.WorkingHours.AddAsync(wh);
                }
            }

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

        /// <summary>
        /// Full onboarding for a new restaurant business.
        /// Creates Business, Setting (agent config), Knowledge Base, Menu Categories, Menu Items, Subscription, and PaymentTransaction.
        /// </summary>
        public async Task<Business> OnboardRestaurantAsync(BusinessOnboardingDTO dto)
        {
            // 1) Create Business
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

            // 4) Create Menu Categories
            var menuCategories = dto.ToMenuCategoryEntities(business.Id).ToList();
            var categoryNameToIdMap = new Dictionary<string, string>();
            
            foreach (var category in menuCategories)
            {
                await _unitOfWork.MenuCategories.AddAsync(category);
                categoryNameToIdMap[category.Name] = category.MenuCategoryId;
            }

            // 5) Create Menu Items (after categories are created)
            var menuItems = dto.ToMenuItemEntities(business.Id, categoryNameToIdMap);
            foreach (var item in menuItems)
            {
                await _unitOfWork.MenuItems.AddAsync(item);
            }

            // 6) Create Working Hours
            var workingHours = dto.ToWorkingHoursEntities(business.Id);
            foreach (var wh in workingHours)
            {
                await _unitOfWork.WorkingHours.AddAsync(wh);
            }

            // 7) Create Subscription
            var subscription = dto.ToSubscription(business.Id);
            await _subscriptionRepository.AddAsync(subscription);

            // 8) Create initial PaymentTransaction
            var payment = dto.ToPaymentTransaction(subscription.Id);
            // NOTE: Card details from dto are not stored for security reasons in this simple example.
            // In production, you would call a payment gateway and store only a token/reference.
            await _paymentRepository.AddAsync(payment);

            // 9) Commit everything
            await _unitOfWork.CompleteAsync();

            return business;
        }
    }
}
