using System;
using Microsoft.Extensions.DependencyInjection;
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
        private readonly IServiceScopeFactory _scopeFactory;

        public InteractionService(
            IInteractionRepository interactionRepository,
            IBusinessRepository businessRepository,
            ICustomerRepository customerRepository,
            IRepository<User> userRepository,
            IUnitOfWork unitOfWork,
            IServiceScopeFactory scopeFactory)
        {
            _interactionRepository = interactionRepository;
            _businessRepository    = businessRepository;
            _customerRepository    = customerRepository;
            _userRepository        = userRepository;
            _unitOfWork            = unitOfWork;
            _scopeFactory          = scopeFactory;
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

            // ── Trigger post-session AI analysis ─────────────────────────────
            // Runs in a NEW scope so the background task has its own DbContext
            // and is not affected by the current request scope being disposed.
            // Non-fatal: analysis failure must never block the close operation.
            var interactionId = interaction.InteractionId;
            _ = Task.Run(async () =>
            {
                using var scope = _scopeFactory.CreateScope();
                var analysisService = scope.ServiceProvider
                    .GetRequiredService<IAiAnalysisService>();
                try
                {
                    await analysisService.AnalyzeSessionAsync(interactionId);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(
                        $"[AiAnalysis] Analysis failed for {interactionId}: {ex.Message}");
                }
            });

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

