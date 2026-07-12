using Domain_layer.Models;
using Service_layer.DTOS.Setting;

namespace Service_layer.Mapping
{
    public static class SettingMapping
    {
        public static SettingResponseDTO ToDto(this Setting s)
        {
            if (s == null) return null;

            return new SettingResponseDTO
            {
                SettingId = s.SettingId,
                BusinessId = s.BusinessId,
                AutoAssignTickets = s.AutoAssignTickets,
                EnableNotifications = s.EnableNotifications,
                Language = s.Language,
                TimeZone = s.TimeZone,
                ChatbotEnabled = s.ChatbotEnabled,
                ChatbotWelcomeMessage = s.ChatbotWelcomeMessage,
                ChatbotPersonality = s.ChatbotPersonality,
                EmailNotifications = s.EmailNotifications,
                SmsNotifications = s.SmsNotifications,
                PushNotifications = s.PushNotifications,
                // Voice Settings
                AgentVoice = s.AgentVoice,
                AgentVoiceProvider = s.AgentVoiceProvider,
                AgentVoiceSpeed = s.AgentVoiceSpeed,
                AgentVoicePitch = s.AgentVoicePitch,
                AgentVoiceLanguage = s.AgentVoiceLanguage,
                // Custom AI Prompts
                CustomSystemPrompt = s.CustomSystemPrompt,
                CustomGreetingTemplate = s.CustomGreetingTemplate
            };
        }
    }
}

