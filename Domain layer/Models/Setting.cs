using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_layer.Models
{
  public  class Setting
    {

        public int SettingId { get; set; }

        // General
        public bool AutoAssignTickets { get; set; } = true;
        public bool EnableNotifications { get; set; } = true;
        public string Language { get; set; } = "en";
        public string TimeZone { get; set; } = "UTC";

        // Chatbot
        public bool ChatbotEnabled { get; set; } = true;
        public string ChatbotWelcomeMessage { get; set; } = "Welcome! How can I help you?";
        public string ChatbotPersonality { get; set; } = "Friendly";

        // Notification channels
        public bool EmailNotifications { get; set; } = true;
        public bool SmsNotifications { get; set; } = false;
        public bool PushNotifications { get; set; } = true;

        // Business relation (1:1)
        public int BusinessId { get; set; }
        public Business Business { get; set; }

    }
}
