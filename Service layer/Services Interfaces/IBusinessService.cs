using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain_layer.Models;
using Service_layer.DTOS.Business;

namespace Service_layer.Services_Interfaces
{
   public interface IBusinessService
    {
        Task<IEnumerable<Business>> GetAllAsync();
        Task<Business?> GetByIdAsync(string id);
        Task<Business> CreateAsync(Business business);
        Task<Business?> UpdateAsync(string id, Business business);
        Task<bool> DeleteAsync(string id);

        /// <summary>
        /// Full onboarding for a new business (restaurant) including:
        /// - Business creation
        /// - Agent/chatbot configuration
        /// - Initial knowledge base seeding
        /// - Subscription & first payment
        /// </summary>
        Task<Business> OnboardRestaurantAsync(BusinessOnboardingDTO dto);
    }
}
