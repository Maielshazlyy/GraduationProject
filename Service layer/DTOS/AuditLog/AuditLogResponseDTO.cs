namespace Service_layer.DTOS.AuditLog
{
    public class AuditLogResponseDTO
    {
        public string AuditLogId { get; set; }
        public string BusinessId { get; set; }
        public string BusinessName { get; set; }
        public string Action { get; set; }
        public string Entity { get; set; }
        public string EntityId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? UserId { get; set; }
        public string? UserName { get; set; }
    }
}

