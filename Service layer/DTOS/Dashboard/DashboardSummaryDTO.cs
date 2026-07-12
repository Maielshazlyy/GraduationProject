namespace Service_layer.DTOS.Dashboard
{
    public class DashboardSummaryDTO
    {
        // Business Info
        public string BusinessId { get; set; } = string.Empty;
        public string BusinessName { get; set; } = string.Empty;
        public string BusinessType { get; set; } = string.Empty;

        // Settings Summary
        public bool HasSettings { get; set; }
        public bool ChatbotEnabled { get; set; }
        public string? WelcomeMessage { get; set; }
        public string? AgentVoice { get; set; }

        // Quick Stats
        public int TotalMenuItems { get; set; }
        public int TotalMenuCategories { get; set; }
        public int TotalKnowledgeBaseItems { get; set; }
        public int TotalFAQs { get; set; }

        // Recent Activity
        public DateTime? LastMenuUpdate { get; set; }
        public DateTime? LastKnowledgeBaseUpdate { get; set; }

        // Setup Status
        public bool IsSetupComplete { get; set; }
        public List<string> SetupStepsCompleted { get; set; } = new List<string>();
        public List<string> SetupStepsPending { get; set; } = new List<string>();

        // Audit Log Summary
        public int TotalAuditLogs { get; set; }
        public int AuditLogsLast24Hours { get; set; }
        public DateTime? LastAuditLogDate { get; set; }
    }
}

