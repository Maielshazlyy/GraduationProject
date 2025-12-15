using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain_layer.Interfaces;
using Domain_layer.Models;
using Service_layer.Services_Interfaces;

namespace Service_layer.Services
{
   public class BusinessService:IBusinessService
    {
        private readonly IUnitOfWork _unitOfWork;
        public BusinessService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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

            existing.BusinessId = business.BusinessId;
            existing.Name = business.Name;
            existing.Type = business.Type;
            existing.Address = business.Address;
            existing.Phone = business.Phone;
            existing.CreatedAt = business.CreatedAt;

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
    }
}
