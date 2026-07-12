using System;
using Domain_layer.Interfaces;
using Domain_layer.Models;
using Service_layer.DTOS.Customer;
using Service_layer.Services_Interfaces;

namespace Service_layer.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IBusinessRepository _businessRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CustomerService(
            ICustomerRepository customerRepository,
            IBusinessRepository businessRepository,
            IUnitOfWork unitOfWork)
        {
            _customerRepository = customerRepository;
            _businessRepository = businessRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            return await _customerRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Customer>> GetByBusinessIdAsync(string businessId)
        {
            return await _customerRepository.GetByBusinessIdAsync(businessId);
        }

        public async Task<Customer?> GetByIdAsync(string id)
        {
            return await _customerRepository.GetByIdAsync(id);
        }

        public async Task<Customer?> GetByEmailAsync(string email)
        {
            return await _customerRepository.GetByEmailAsync(email);
        }

        public async Task<Customer> CreateAsync(CustomerCreateDTO dto)
        {
            var business = await _businessRepository.GetByIdAsync(dto.BusinessId);
            if (business == null)
                throw new ArgumentException($"Business with id '{dto.BusinessId}' not found.");

            // Check if email already exists for this business
            if (!string.IsNullOrEmpty(dto.Email))
            {
                var existingCustomer = await _customerRepository.GetByEmailAsync(dto.Email);
                if (existingCustomer != null && existingCustomer.BusinessId == dto.BusinessId)
                    throw new ArgumentException($"Customer with email '{dto.Email}' already exists for this business.");
            }

            var customer = new Customer
            {
                CustomerId = Guid.NewGuid().ToString(),
                FullName = dto.FullName,
                Email = dto.Email ?? string.Empty,
                Phone = dto.Phone ?? string.Empty,
                BusinessId = dto.BusinessId,
                CreatedAt = DateTime.UtcNow
            };

            await _customerRepository.AddAsync(customer);
            await _unitOfWork.CompleteAsync();
            return customer;
        }

        public async Task<Customer?> UpdateAsync(string id, CustomerUpdateDTO dto)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer == null) return null;

            customer.FullName = dto.FullName;
            if (!string.IsNullOrEmpty(dto.Email))
                customer.Email = dto.Email;
            if (!string.IsNullOrEmpty(dto.Phone))
                customer.Phone = dto.Phone;

            _customerRepository.Update(customer);
            await _unitOfWork.CompleteAsync();
            return customer;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer == null) return false;

            _customerRepository.Delete(customer);
            await _unitOfWork.CompleteAsync();
            return true;
        }
    }
}

