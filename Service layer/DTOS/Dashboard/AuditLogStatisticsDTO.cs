namespace Service_layer.DTOS.Dashboard
{
    public class AuditLogStatisticsDTO
    {
        public int TotalActions { get; set; }
        public int ActionsLast24Hours { get; set; }
        public int ActionsLast7Days { get; set; }
        public int ActionsLast30Days { get; set; }

        // Actions by Entity Type
        public Dictionary<string, int> ActionsByEntity { get; set; } = new Dictionary<string, int>();

        // Actions by Type
        public Dictionary<string, int> ActionsByType { get; set; } = new Dictionary<string, int>();

        // Most Active Users
        public List<UserActivityDTO> MostActiveUsers { get; set; } = new List<UserActivityDTO>();

        // Recent Critical Actions
        public List<string> RecentCriticalActions { get; set; } = new List<string>();
    }

    public class UserActivityDTO
    {
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public int ActionCount { get; set; }
    }
}

