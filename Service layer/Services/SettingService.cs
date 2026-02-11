using Domain_layer.Interfaces;
using Domain_layer.Models;
using Service_layer.DTOS.Setting;
using Service_layer.Services_Interfaces;

namespace Service_layer.Services
{
    public class SettingService : ISettingService
    {
        private readonly ISettingRepository _settingRepository;
        private readonly IBusinessRepository _businessRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SettingService(
            ISettingRepository settingRepository,
            IBusinessRepository businessRepository,
            IUnitOfWork unitOfWork)
        {
            _settingRepository = settingRepository;
            _businessRepository = businessRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Setting?> GetByBusinessIdAsync(string businessId)
        {
            return await _settingRepository.GetByBusinessIdAsync(businessId);
        }

        public async Task<Setting?> UpdateAsync(string businessId, SettingUpdateDTO dto)
        {
            var business = await _businessRepository.GetByIdAsync(businessId);
            if (business == null)
                throw new ArgumentException($"Business with id '{businessId}' not found.");

            var setting = await _settingRepository.GetByBusinessIdAsync(businessId);
            if (setting == null)
                throw new ArgumentException($"Settings not found for business '{businessId}'. Please create settings first.");

            // Update only provided fields (partial update)
            if (dto.AutoAssignTickets.HasValue)
                setting.AutoAssignTickets = dto.AutoAssignTickets.Value;
            
            if (dto.EnableNotifications.HasValue)
                setting.EnableNotifications = dto.EnableNotifications.Value;
            
            if (!string.IsNullOrWhiteSpace(dto.Language))
                setting.Language = dto.Language;
            
            if (!string.IsNullOrWhiteSpace(dto.TimeZone))
                setting.TimeZone = dto.TimeZone;

            // Chatbot Settings
            if (dto.ChatbotEnabled.HasValue)
                setting.ChatbotEnabled = dto.ChatbotEnabled.Value;
            
            if (!string.IsNullOrWhiteSpace(dto.ChatbotWelcomeMessage))
                setting.ChatbotWelcomeMessage = dto.ChatbotWelcomeMessage;
            
            if (!string.IsNullOrWhiteSpace(dto.ChatbotPersonality))
                setting.ChatbotPersonality = dto.ChatbotPersonality;

            // Voice Settings
            if (!string.IsNullOrWhiteSpace(dto.AgentVoice))
                setting.AgentVoice = dto.AgentVoice;
            
            if (!string.IsNullOrWhiteSpace(dto.AgentVoiceProvider))
                setting.AgentVoiceProvider = dto.AgentVoiceProvider;
            
            if (dto.AgentVoiceSpeed.HasValue)
                setting.AgentVoiceSpeed = Math.Clamp(dto.AgentVoiceSpeed.Value, 0.5, 2.0);
            
            if (dto.AgentVoicePitch.HasValue)
                setting.AgentVoicePitch = Math.Clamp(dto.AgentVoicePitch.Value, 0.5, 2.0);
            
            if (!string.IsNullOrWhiteSpace(dto.AgentVoiceLanguage))
                setting.AgentVoiceLanguage = dto.AgentVoiceLanguage;

            // Custom AI Prompts
            if (dto.CustomSystemPrompt != null)
                setting.CustomSystemPrompt = dto.CustomSystemPrompt;
            
            if (dto.CustomGreetingTemplate != null)
                setting.CustomGreetingTemplate = dto.CustomGreetingTemplate;

            // Notification Settings
            if (dto.EmailNotifications.HasValue)
                setting.EmailNotifications = dto.EmailNotifications.Value;
            
            if (dto.SmsNotifications.HasValue)
                setting.SmsNotifications = dto.SmsNotifications.Value;
            
            if (dto.PushNotifications.HasValue)
                setting.PushNotifications = dto.PushNotifications.Value;

            _settingRepository.Update(setting);
            await _unitOfWork.CompleteAsync();
            return setting;
        }
    }
}

