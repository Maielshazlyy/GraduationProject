using Domain_layer.Models;

namespace Domain_layer.Interfaces
{
    public interface IAuditLogRepository : IRepository<AuditLog>
    {
        Task<IEnumerable<AuditLog>> GetByBusinessIdAsync(string businessId);
        Task<IEnumerable<AuditLog>> GetByUserIdAsync(string userId);
    }
}

