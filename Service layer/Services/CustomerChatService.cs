using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Domain_layer.Interfaces;
using Domain_layer.Models;
using Domain_layer.enums;
using Service_layer.DTOS.AiChat;
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
        private readonly IResponseGenerationService _responseGenerationService;
        private readonly ISettingService _settingService;
        private readonly IAuditLogService? _auditLogService;
        private readonly ISentimentService? _sentimentService;
        private readonly IAiChatService _aiChatService;
        private readonly CustomerInteractionBusinessLogic _businessLogic;

        public CustomerChatService(
            IUnitOfWork unitOfWork,
            IIntentDetectionService intentDetectionService,
            IResponseGenerationService responseGenerationService,
            ISettingService settingService,
            IAiChatService aiChatService,
            IAuditLogService? auditLogService = null,
            ISentimentService? sentimentService = null)
        {
            _unitOfWork = unitOfWork;
            _intentDetectionService = intentDetectionService;
            _responseGenerationService = responseGenerationService;
            _settingService = settingService;
            _aiChatService = aiChatService;
            _auditLogService = auditLogService;
            _sentimentService = sentimentService;
            _businessLogic = new CustomerInteractionBusinessLogic(unitOfWork, auditLogService);
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
            var interaction = await GetOrCreateInteractionAsync(request, channel);

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
            await _unitOfWork.CompleteAsync();

            // 3.5) Analyze sentiment of customer message using AI
            if (_sentimentService != null && !string.IsNullOrWhiteSpace(request.Message))
            {
                try
                {
                    var language = intentResult.DetectedLanguage ?? "ar";
                    var sentiment = await _sentimentService.AnalyzeSentimentAsync(
                        customerMessage.MessageId, 
                        request.Message, 
                        language);
                    // Sentiment is automatically linked to message via MessageId
                }
                catch (Exception ex)
                {
                    // Log error but don't fail the request
                    // TODO: Add proper logging
                    System.Diagnostics.Debug.WriteLine($"Sentiment analysis failed: {ex.Message}");
                }
            }

            // 4) Check if this message should be escalated to a human agent
            var shouldEscalate = ShouldEscalateToHuman(intentResult);

            // 5) Execute business logic based on intent / escalation
            string? orderId = null;
            string? ticketId = null;
            ChatCartSummaryDTO? cartSummary = null;
            var recommendations = new List<RecommendationItemDTO>();
            var context = BuildResponseContext(interaction, intentResult, recentMessages, "WebChat");

            if (shouldEscalate)
            {
                var ticket = await _businessLogic.HandleTicketAsync(interaction, intentResult);
                ticketId = ticket.TicketId;
                context.ActionOutcome = "EscalatedToHuman";
                context.ActionData["ticketId"] = ticket.TicketId;

                if (_auditLogService != null && ticket.TicketType == "HumanEscalation")
                {
                    await _auditLogService.LogInteractionActionAsync(
                        businessId: interaction.BusinessId,
                        action: $"EscalateToHuman_{intentResult.Intent}",
                        interactionId: interaction.InteractionId,
                        userId: null
                    );
                }
            }
            else
            {
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
                            context.ActionOutcome = "OrderCreated";
                            context.ActionData["orderId"] = orderId ?? "";
                            context.ActionData["totalPrice"] = cartSummary?.TotalPrice ?? 0m;
                            context.ActionData["hasDeliveryDelay"] = false;
                            context.ActionData["recommendations"] = recommendations;
                            break;
                        }

                    case "ModifyOrder":
                        {
                            var msg = await _businessLogic.HandleModifyOrderAsync(interaction, intentResult);
                            context.ActionOutcome = "ModifyOrderHandled";
                            context.ActionData["message"] = msg;
                            break;
                        }

                    case "CancelOrder":
                        {
                            var msg = await _businessLogic.HandleCancelOrderAsync(interaction, intentResult);
                            context.ActionOutcome = "OrderCancelled";
                            context.ActionData["message"] = msg;
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
                                interaction.Status = "Escalated";
                            context.ActionOutcome = ticket.TicketType == "HumanEscalation" ? "EscalatedToHuman" : "TicketCreated";
                            context.ActionData["ticketId"] = ticket.TicketId;
                            break;
                        }

                    case "AskAboutOrderStatus":
                        {
                            var msg = await _businessLogic.HandleAskOrderStatusAsync(interaction, intentResult);
                            context.ActionOutcome = "OrderStatusRetrieved";
                            context.ActionData["message"] = msg;
                            break;
                        }

                    case "AskAboutProducts":
                        {
                            var msg = await _businessLogic.HandleAskProductsAsync(interaction, intentResult);
                            context.ActionOutcome = "ProductsListed";
                            context.ActionData["message"] = msg;
                            break;
                        }

                    default:
                        {
                            var msg = await _businessLogic.HandleGeneralQuestionAsync(interaction, intentResult);
                            context.ActionOutcome = "GeneralQuestion";
                            context.ActionData["message"] = msg;
                            break;
                        }
                }
            }

            _unitOfWork.Interactions.Update(interaction);
            await _unitOfWork.CompleteAsync();

            // 5.5) AI Response Generation - forward message to external AI API
            string replyText;
            try
            {
                var aiResponse = await _aiChatService.SendMessageAsync(new AiChatRequestDTO
                {
                    Message = request.Message!,
                    SessionId = interaction.InteractionId
                });
                replyText = aiResponse.GetReplyText();
                if (string.IsNullOrWhiteSpace(replyText))
                    replyText = await _responseGenerationService.GenerateResponseAsync(context);
            }
            catch
            {
                replyText = await _responseGenerationService.GenerateResponseAsync(context);
            }

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

        /// <summary>
        /// Get backend-driven recommendations for a main menu item
        /// (e.g., suggest fries and drink with burger) without using chat.
        /// This is used by the public ordering page when the customer
        /// selects a main item directly from the menu.
        /// </summary>
        public async Task<List<RecommendationItemDTO>> GetOrderRecommendationsAsync(
            CustomerOrderRecommendationRequestDTO request)
        {
            if (string.IsNullOrWhiteSpace(request.BusinessId))
            {
                throw new ArgumentException("BusinessId is required.");
            }

            if (string.IsNullOrWhiteSpace(request.MainMenuItemId))
            {
                throw new ArgumentException("MainMenuItemId is required.");
            }

            var allItems = (await _unitOfWork.MenuItems
                    .GetByBusinessIdAsync(request.BusinessId))
                .Where(mi => mi.IsAvailable)
                .ToList();

            if (!allItems.Any())
            {
                return new List<RecommendationItemDTO>();
            }

            var mainItem = allItems.FirstOrDefault(mi => mi.MenuItemId == request.MainMenuItemId);
            if (mainItem == null)
            {
                throw new ArgumentException($"Menu item with id '{request.MainMenuItemId}' not found or not available.");
            }

            // We don't need a real order instance for now because the current
            // recommendation logic only depends on the menu items and main item.
            var dummyOrder = new Order
            {
                OrderId = Guid.NewGuid().ToString(),
                BusinessId = request.BusinessId
            };

            return _businessLogic.BuildRecommendationsForOrder(allItems, mainItem, dummyOrder);
        }

        #region Private Helper Methods

        private static bool ShouldEscalateToHuman(DetectedIntentResultDTO intentResult)
        {
            const double confidenceThreshold = 0.6;

            if (intentResult == null)
                return false;

            if (string.Equals(intentResult.Intent, "RequestHumanAgent", StringComparison.OrdinalIgnoreCase))
                return true;

            if (intentResult.RequiresEscalation)
                return true;

            if (!string.IsNullOrWhiteSpace(intentResult.ComplexityLevel) &&
                string.Equals(intentResult.ComplexityLevel, "High", StringComparison.OrdinalIgnoreCase))
                return true;

            if (intentResult.Confidence < confidenceThreshold)
                return true;

            return false;
        }

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

        private static ResponseGenerationContextDTO BuildResponseContext(
            Interaction interaction,
            DetectedIntentResultDTO intentResult,
            List<string> recentMessages,
            string channel)
        {
            return new ResponseGenerationContextDTO
            {
                BusinessId = interaction.BusinessId,
                InteractionId = interaction.InteractionId,
                Intent = intentResult.Intent,
                DetectedLanguage = intentResult.DetectedLanguage,
                DetectedDialect = intentResult.DetectedDialect,
                RecentMessages = recentMessages,
                ActionOutcome = "",
                ActionData = new Dictionary<string, object>(),
                Channel = channel
            };
        }

        #endregion
    }
}

