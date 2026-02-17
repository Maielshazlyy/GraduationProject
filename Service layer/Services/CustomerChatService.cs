using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Domain_layer.Interfaces;
using Domain_layer.Models;
using Domain_layer.enums;
using Service_layer.DTOS.Chat;
using Service_layer.Services_Interfaces;

namespace Service_layer.Services
{
    /// <summary>
    /// Service for handling customer text chat interactions (WebChat channel).
    /// Handles text messages, intent detection, orders, tickets, and recommendations.
    /// </summary>
    public class CustomerChatService : ICustomerChatService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IIntentDetectionService _intentDetectionService;
        private readonly ISettingService _settingService;
        private readonly CustomerInteractionBusinessLogic _businessLogic;

        public CustomerChatService(
            IUnitOfWork unitOfWork,
            IIntentDetectionService intentDetectionService,
            ISettingService settingService)
        {
            _unitOfWork = unitOfWork;
            _intentDetectionService = intentDetectionService;
            _settingService = settingService;
            _businessLogic = new CustomerInteractionBusinessLogic(unitOfWork);
        }

        public async Task<CustomerChatResponseDTO> HandleMessageAsync(CustomerChatRequestDTO request)
        {
            // Ensure this is a Chat request
            var channel = string.IsNullOrWhiteSpace(request.Channel) ? "WebChat" : request.Channel;
            if (channel != "WebChat")
            {
                throw new ArgumentException("This service handles WebChat only. Use ICustomerVoiceService for Voice.");
            }

            if (string.IsNullOrWhiteSpace(request.Message))
            {
                throw new ArgumentException("Message is required for Chat.");
            }

            // 1) Ensure interaction exists
            var interaction = await GetOrCreateInteractionAsync(request, "WebChat");

            // 2) Store customer message (text only for Chat)
            var customerMessage = new Message
            {
                MessageId = Guid.NewGuid().ToString(),
                InteractionId = interaction.InteractionId,
                SenderType = "Customer",
                Content = request.Message,
                SentAt = DateTime.UtcNow
            };

            await _unitOfWork.Messages.AddAsync(customerMessage);
            await _unitOfWork.CompleteAsync();

            // 3) Load recent messages for intent detection
            var recentMessages = (await _unitOfWork.Messages
                    .FindAsync(m => m.InteractionId == interaction.InteractionId))
                .OrderBy(m => m.SentAt)
                .Select(m => $"{m.SenderType}: {m.Content}")
                .TakeLast(10)
                .ToList();

            var intentResult = await _intentDetectionService
                .DetectIntentAsync(interaction.BusinessId, interaction.InteractionId, recentMessages);

            customerMessage.Intent = intentResult.Intent;
            customerMessage.ConfidenceScore = intentResult.Confidence;
            customerMessage.AiMetadataJson = System.Text.Json.JsonSerializer.Serialize(intentResult);
            _unitOfWork.Messages.Update(customerMessage);

            // 4) Execute business logic based on intent
            string replyText;
            string? orderId = null;
            string? ticketId = null;
            ChatCartSummaryDTO? cartSummary = null;
            var recommendations = new List<RecommendationItemDTO>();

            switch (intentResult.Intent)
            {
                case "CreateOrder":
                    {
                        var orderResult = await _businessLogic.HandleCreateOrderAsync(interaction, intentResult);
                        orderId = orderResult.Order?.OrderId;
                        cartSummary = orderResult.Cart;
                        recommendations = orderResult.Recommendations;
                        interaction.InteractionType = "Order";
                        interaction.RelatedOrderId = orderId;
                        replyText = _businessLogic.BuildOrderReply(orderResult, false, null, intentResult.DetectedDialect);
                        break;
                    }

                case "ModifyOrder":
                    {
                        replyText = await _businessLogic.HandleModifyOrderAsync(interaction, intentResult);
                        break;
                    }

                case "CancelOrder":
                    {
                        replyText = await _businessLogic.HandleCancelOrderAsync(interaction, intentResult);
                        break;
                    }

                case "Complaint":
                case "RequestHumanAgent":
                    {
                        var ticket = await _businessLogic.HandleTicketAsync(interaction, intentResult);
                        ticketId = ticket.TicketId;
                        interaction.InteractionType = "Ticket";
                        interaction.RelatedTicketId = ticketId;
                        if (intentResult.Intent == "RequestHumanAgent")
                        {
                            interaction.Status = "Escalated";
                        }
                        replyText = _businessLogic.BuildTicketReply(ticket, intentResult.Intent, intentResult.DetectedDialect);
                        break;
                    }

                case "AskAboutOrderStatus":
                    {
                        replyText = await _businessLogic.HandleAskOrderStatusAsync(interaction, intentResult);
                        break;
                    }

                case "AskAboutProducts":
                    {
                        replyText = await _businessLogic.HandleAskProductsAsync(interaction, intentResult);
                        break;
                    }

                default:
                    {
                        replyText = await _businessLogic.HandleGeneralQuestionAsync(interaction, intentResult);
                        break;
                    }
            }

            _unitOfWork.Interactions.Update(interaction);
            await _unitOfWork.CompleteAsync();

            // 5) Store AI reply message
            var aiMessage = new Message
            {
                MessageId = Guid.NewGuid().ToString(),
                InteractionId = interaction.InteractionId,
                SenderType = "AI",
                Content = replyText,
                SentAt = DateTime.UtcNow,
                Intent = intentResult.Intent,
                ConfidenceScore = intentResult.Confidence
            };

            await _unitOfWork.Messages.AddAsync(aiMessage);
            await _unitOfWork.CompleteAsync();

            // 6) Return response (text only for Chat)
            return new CustomerChatResponseDTO
            {
                InteractionId = interaction.InteractionId,
                ReplyText = replyText,
                OrderId = orderId,
                TicketId = ticketId,
                Cart = cartSummary,
                Recommendations = recommendations,
                IsInterrupted = false
            };
        }

        public async Task<CustomerCapabilitiesDTO> GetCapabilitiesAsync(string businessId)
        {
            var business = await _unitOfWork.Businesses.GetByIdAsync(businessId);
            if (business == null)
                throw new ArgumentException($"Business with id '{businessId}' not found.");

            var settings = await _settingService.GetByBusinessIdAsync(businessId);
            
            return new CustomerCapabilitiesDTO
            {
                BusinessId = businessId,
                BusinessName = business.Name,
                ChatEnabled = settings?.ChatbotEnabled ?? true,
                VoiceEnabled = true,
                WelcomeMessage = settings?.ChatbotWelcomeMessage ?? "Welcome! How can I help you?",
                VoiceSettings = settings != null ? new VoiceSettingsDTO
                {
                    AgentVoice = settings.AgentVoice,
                    AgentVoiceProvider = settings.AgentVoiceProvider,
                    AgentVoiceSpeed = settings.AgentVoiceSpeed,
                    AgentVoicePitch = settings.AgentVoicePitch,
                    AgentVoiceLanguage = settings.AgentVoiceLanguage
                } : null
            };
        }

        #region Private Helper Methods

        private async Task<Interaction> GetOrCreateInteractionAsync(CustomerChatRequestDTO request, string channel)
        {
            if (!string.IsNullOrWhiteSpace(request.InteractionId))
            {
                var existing = await _unitOfWork.Interactions.GetByIdAsync(request.InteractionId);
                if (existing != null)
                    return existing;
            }

            var interaction = new Interaction
            {
                InteractionId = Guid.NewGuid().ToString(),
                BusinessId = request.BusinessId,
                CustomerId = request.CustomerId ?? Guid.NewGuid().ToString(),
                Channel = channel,
                Status = "Open",
                StartedAt = DateTime.UtcNow
            };

            await _unitOfWork.Interactions.AddAsync(interaction);
            await _unitOfWork.CompleteAsync();

            return interaction;
        }

        #endregion
    }
}

