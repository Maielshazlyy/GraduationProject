using System;
using Domain_layer.Interfaces;
using Domain_layer.Models;
using Service_layer.DTOS.Interaction;
using Service_layer.Services_Interfaces;

namespace Service_layer.Services
{
    public class InteractionService : IInteractionService
    {
        private readonly IInteractionRepository _interactionRepository;
        private readonly IBusinessRepository _businessRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public InteractionService(
            IInteractionRepository interactionRepository,
            IBusinessRepository businessRepository,
            ICustomerRepository customerRepository,
            IRepository<User> userRepository,
            IUnitOfWork unitOfWork)
        {
            _interactionRepository = interactionRepository;
            _businessRepository = businessRepository;
            _customerRepository = customerRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Interaction>> GetAllAsync()
        {
            return await _interactionRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Interaction>> GetByBusinessIdAsync(string businessId)
        {
            return await _interactionRepository.GetByBusinessIdAsync(businessId);
        }

        public async Task<IEnumerable<Interaction>> GetByCustomerIdAsync(string customerId)
        {
            return await _interactionRepository.GetByCustomerIdAsync(customerId);
        }

        public async Task<IEnumerable<Interaction>> GetByUserIdAsync(string userId)
        {
            return await _interactionRepository.GetByUserIdAsync(userId);
        }

        public async Task<Interaction?> GetByIdAsync(string id)
        {
            return await _interactionRepository.GetByIdAsync(id);
        }

        public async Task<Interaction> StartInteractionAsync(StartInteractionDTO dto)
        {
            var business = await _businessRepository.GetByIdAsync(dto.BusinessId);
            if (business == null)
                throw new ArgumentException($"Business with id '{dto.BusinessId}' not found.");

            var customer = await _customerRepository.GetByIdAsync(dto.CustomerId);
            if (customer == null)
                throw new ArgumentException($"Customer with id '{dto.CustomerId}' not found.");

            var interaction = new Interaction
            {
                InteractionId = Guid.NewGuid().ToString(),
                Channel = dto.Channel,
                Status = "Open",
                IsEnded = false,
                BusinessId = dto.BusinessId,
                CustomerId = dto.CustomerId,
                StartedAt = DateTime.UtcNow
            };

            await _interactionRepository.AddAsync(interaction);
            await _unitOfWork.CompleteAsync();
            return interaction;
        }

        public async Task<Interaction?> EndInteractionAsync(string id, EndInteractionDTO dto)
        {
            var interaction = await _interactionRepository.GetByIdAsync(id);
            if (interaction == null) return null;

            interaction.Status = "Closed";
            interaction.IsEnded = true;
            interaction.EndedAt = DateTime.UtcNow;

            if (!string.IsNullOrEmpty(dto.UserId))
            {
                var user = await _userRepository.GetByIdAsync(dto.UserId);
                if (user == null)
                    throw new ArgumentException($"User with id '{dto.UserId}' not found.");

                interaction.HandledByUserId = dto.UserId;
            }

            _interactionRepository.Update(interaction);
            await _unitOfWork.CompleteAsync();
            return interaction;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var interaction = await _interactionRepository.GetByIdAsync(id);
            if (interaction == null) return false;

            _interactionRepository.Delete(interaction);
            await _unitOfWork.CompleteAsync();
            return true;
        }
    }
}

