using Domain_layer.Models;
using Service_layer.DTOS.AuditLog;

namespace Service_layer.Mapping
{
    public static class AuditLogMapping
    {
        public static AuditLogResponseDTO ToDto(this AuditLog a)
        {
            if (a == null) return null;

            return new AuditLogResponseDTO
            {
                AuditLogId = a.AuditLogId,
                BusinessId = a.BusinessId,
                BusinessName = a.Business?.Name ?? string.Empty,
                Action = a.Action,
                Entity = a.Entity,
                EntityId = a.EntityId,
                CreatedAt = a.CreatedAt,
                UserId = a.UserId,
                UserName = a.User?.FullName ?? string.Empty
            };
        }

        public static IEnumerable<AuditLogResponseDTO> ToDtoList(this IEnumerable<AuditLog> auditLogs)
        {
            return auditLogs.Select(a => a.ToDto()).ToList();
        }
    }
}

