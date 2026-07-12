using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_layer.Models
{
  public  class Setting
    {

        public string SettingId { get; set; }

        // General
        public bool AutoAssignTickets { get; set; } = true;
        public bool EnableNotifications { get; set; } = true;
        public string Language { get; set; } = "en";
        public string TimeZone { get; set; } = "UTC";

        // Chatbot
        public bool ChatbotEnabled { get; set; } = true;
        public string ChatbotWelcomeMessage { get; set; } = "Welcome! How can I help you?";
        public string ChatbotPersonality { get; set; } = "Friendly";

        // AI Agent Voice Settings
        public string AgentVoice { get; set; } = "default"; // e.g., "male", "female", "neutral", "ar-SA-ZariyahNeural", "en-US-JennyNeural"
        public string AgentVoiceProvider { get; set; } = "azure"; // e.g., "azure", "google", "aws", "elevenlabs"
        public double AgentVoiceSpeed { get; set; } = 1.0; // 0.5 to 2.0 (default 1.0)
        public double AgentVoicePitch { get; set; } = 1.0; // 0.5 to 2.0 (default 1.0)
        public string AgentVoiceLanguage { get; set; } = "en-US"; // e.g., "en-US", "ar-SA", "fr-FR"

        // Custom AI Prompts (Optional)
        public string? CustomSystemPrompt { get; set; } // Custom system prompt for AI agent
        public string? CustomGreetingTemplate { get; set; } // Template for greeting messages

        // Notification channels
        public bool EmailNotifications { get; set; } = true;
        public bool SmsNotifications { get; set; } = false;
        public bool PushNotifications { get; set; } = true;

        // Business relation (1:1)
        public string BusinessId { get; set; }
        public Business Business { get; set; }

    }
}
