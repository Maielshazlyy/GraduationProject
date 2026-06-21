using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_layer.DTOS.Setting
{
    public class SettingUpdateDTO
    {
        // General Settings
        public bool? AutoAssignTickets { get; set; }
        public bool? EnableNotifications { get; set; }
        public string? Language { get; set; }
        public string? TimeZone { get; set; }

        // Chatbot Settings
        public bool? ChatbotEnabled { get; set; }
        public string? ChatbotWelcomeMessage { get; set; }
        public string? ChatbotPersonality { get; set; }

        // Voice Settings
        public string? AgentVoice { get; set; }
        public string? AgentVoiceProvider { get; set; }
        public double? AgentVoiceSpeed { get; set; }
        public double? AgentVoicePitch { get; set; }
        public string? AgentVoiceLanguage { get; set; }

        // Custom AI Prompts
        public string? CustomSystemPrompt { get; set; }
        public string? CustomGreetingTemplate { get; set; }

        // Notification Settings
        public bool? EmailNotifications { get; set; }
        public bool? SmsNotifications { get; set; }
        public bool? PushNotifications { get; set; }
    }
}
 