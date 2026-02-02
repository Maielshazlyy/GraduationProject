using Domain_layer.Interfaces;
using Domain_layer.Models;
using Service_layer.Services_Interfaces;

namespace Service_layer.Services
{
    public class AuditLogService : IAuditLogService
    {
        private readonly IAuditLogRepository _auditLogRepository;

        public AuditLogService(IAuditLogRepository auditLogRepository)
        {
            _auditLogRepository = auditLogRepository;
        }

        public async Task<IEnumerable<AuditLog>> GetAllAsync()
        {
            return await _auditLogRepository.GetAllAsync();
        }

        public async Task<IEnumerable<AuditLog>> GetByBusinessIdAsync(string businessId)
        {
            return await _auditLogRepository.GetByBusinessIdAsync(businessId);
        }

        public async Task<IEnumerable<AuditLog>> GetByUserIdAsync(string userId)
        {
            return await _auditLogRepository.GetByUserIdAsync(userId);
        }

        public async Task<AuditLog?> GetByIdAsync(string id)
        {
            return await _auditLogRepository.GetByIdAsync(id);
        }
    }
}

