namespace Service_layer.DTOS.Setting
{
    public class SettingResponseDTO
    {
        public string SettingId { get; set; }
        public string BusinessId { get; set; }
        public bool AutoAssignTickets { get; set; }
        public bool EnableNotifications { get; set; }
        public string Language { get; set; }
        public string TimeZone { get; set; }
        public bool ChatbotEnabled { get; set; }
        public string ChatbotWelcomeMessage { get; set; }
        public string ChatbotPersonality { get; set; }
        public bool EmailNotifications { get; set; }
        public bool SmsNotifications { get; set; }
        public bool PushNotifications { get; set; }
    }
}

