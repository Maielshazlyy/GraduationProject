using Domain_layer.Models;

namespace Service_layer.Services_Interfaces
{
    public interface IAuditLogService
    {
        Task<IEnumerable<AuditLog>> GetAllAsync();
        Task<IEnumerable<AuditLog>> GetByBusinessIdAsync(string businessId);
        Task<IEnumerable<AuditLog>> GetByUserIdAsync(string userId);
        Task<AuditLog?> GetByIdAsync(string id);
    }
}

