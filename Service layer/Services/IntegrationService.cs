using System;
using Domain_layer.Interfaces;
using Domain_layer.Models;
using Domain_layer.enums;
using Service_layer.DTOS.Integration;
using Service_layer.Services_Interfaces;

namespace Service_layer.Services
{
    public class IntegrationService : IIntegrationService
    {
        private readonly IIntegrationRepository _integrationRepository;
        private readonly IBusinessRepository _businessRepository;
        private readonly IUnitOfWork _unitOfWork;

        public IntegrationService(
            IIntegrationRepository integrationRepository,
            IBusinessRepository businessRepository,
            IUnitOfWork unitOfWork)
        {
            _integrationRepository = integrationRepository;
            _businessRepository = businessRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Integration>> GetAllAsync()
        {
            return await _integrationRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Integration>> GetByBusinessIdAsync(string businessId)
        {
            return await _integrationRepository.GetByBusinessIdAsync(businessId);
        }

        public async Task<Integration?> GetByIdAsync(string id)
        {
            return await _integrationRepository.GetByIdAsync(id);
        }

        public async Task<Integration> ConnectAsync(IntegrationConnectDTO dto)
        {
            var business = await _businessRepository.GetByIdAsync(dto.BusinessId);
            if (business == null)
                throw new ArgumentException($"Business with id '{dto.BusinessId}' not found.");

            // Check if integration already exists
            var existing = await _integrationRepository.GetByPlatformNameAsync(dto.BusinessId, dto.PlatformName);
            if (existing != null)
                throw new ArgumentException($"Integration '{dto.PlatformName}' already exists for this business.");

            var integration = new Integration
            {
                Id = Guid.NewGuid().ToString(),
                IntegrationId = Guid.NewGuid().ToString(),
                BusinessId = dto.BusinessId,
                PlatformName = dto.PlatformName,
                ApiKeyOrConfig = dto.ApiKeyOrConfig,
                Status = IntegrationStatus.Active,
                LastSyncDate = null
            };

            await _integrationRepository.AddAsync(integration);
            await _unitOfWork.CompleteAsync();
            return integration;
        }

        public async Task<Integration?> SyncAsync(string id, IntegrationSyncDTO dto)
        {
            var integration = await _integrationRepository.GetByIdAsync(id);
            if (integration == null) return null;

            integration.LastSyncDate = DateTime.UtcNow;
            // Here you would implement actual sync logic based on SyncType

            _integrationRepository.Update(integration);
            await _unitOfWork.CompleteAsync();
            return integration;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var integration = await _integrationRepository.GetByIdAsync(id);
            if (integration == null) return false;

            _integrationRepository.Delete(integration);
            await _unitOfWork.CompleteAsync();
            return true;
        }
    }
}

