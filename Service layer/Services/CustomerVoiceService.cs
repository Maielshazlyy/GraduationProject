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
    /// Service for handling customer voice call interactions (Voice channel).
    /// Handles audio messages, speech-to-text, text-to-speech, call sessions,
    /// delivery delays, recovery, and feedback collection.
    /// </summary>
    public class CustomerVoiceService : ICustomerVoiceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IIntentDetectionService _intentDetectionService;
        private readonly ISettingService _settingService;
        private readonly IAuditLogService? _auditLogService;
        private readonly ISentimentService? _sentimentService;
        private readonly CustomerInteractionBusinessLogic _businessLogic;

        public CustomerVoiceService(
            IUnitOfWork unitOfWork,
            IIntentDetectionService intentDetectionService,
            ISettingService settingService,
            IAuditLogService? auditLogService = null,
            ISentimentService? sentimentService = null)
        {
            _unitOfWork = unitOfWork;
            _intentDetectionService = intentDetectionService;
            _settingService = settingService;
            _auditLogService = auditLogService;
            _sentimentService = sentimentService;
            _businessLogic = new CustomerInteractionBusinessLogic(unitOfWork, auditLogService);
        }

        public async Task<CustomerChatResponseDTO> HandleVoiceMessageAsync(CustomerChatRequestDTO request)
        {
            // Ensure this is a Voice request
            var channel = string.IsNullOrWhiteSpace(request.Channel) ? "Voice" : request.Channel;
            if (channel != "Voice")
            {
                throw new ArgumentException("This service handles Voice only. Use ICustomerChatService for Chat.");
            }

            if (string.IsNullOrWhiteSpace(request.AudioData) && string.IsNullOrWhiteSpace(request.Message))
            {
                throw new ArgumentException("AudioData or Message is required for Voice.");
            }

            // 1) Convert audio to text (if audio provided)
            string messageText = request.Message ?? string.Empty;
            string? audioPath = null;
            if (!string.IsNullOrWhiteSpace(request.AudioData))
            {
                // TODO: Integrate with speech-to-text service (Azure Speech, Google Speech-to-Text, etc.)
                messageText = await ConvertAudioToTextAsync(request.AudioData, request.AudioFormat);
                // TODO: Save audio file and store path
                // audioPath = await SaveAudioFileAsync(request.AudioData, request.AudioFormat);
            }

            if (string.IsNullOrWhiteSpace(messageText))
            {
                throw new ArgumentException("Message text is required after audio conversion.");
            }

            // 2) Ensure interaction exists (with CallSessionId for Voice)
            var interaction = await GetOrCreateInteractionAsync(request, "Voice");

            // 3) Store customer message (with AudioPath for Voice)
            var customerMessage = new Message
            {
                MessageId = Guid.NewGuid().ToString(),
                InteractionId = interaction.InteractionId,
                SenderType = "Customer",
                Content = messageText,
                AudioPath = audioPath, // For Voice messages
                SentAt = DateTime.UtcNow
            };

            await _unitOfWork.Messages.AddAsync(customerMessage);
            await _unitOfWork.CompleteAsync();

            // 4) Load recent messages for intent detection
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

            // 4.5) Analyze sentiment of customer message using AI
            if (_sentimentService != null && !string.IsNullOrWhiteSpace(messageText))
            {
                try
                {
                    var language = intentResult.DetectedLanguage ?? "ar";
                    var sentiment = await _sentimentService.AnalyzeSentimentAsync(
                        customerMessage.MessageId, 
                        messageText, 
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

            // 5) Execute business logic based on intent (with delivery delay check for Voice)
            string replyText;
            string? orderId = null;
            string? ticketId = null;
            ChatCartSummaryDTO? cartSummary = null;
            var recommendations = new List<RecommendationItemDTO>();
            bool hasDeliveryDelay = false;
            List<string>? alternativeTimeSlots = null;

            var shouldEscalate = ShouldEscalateToHuman(intentResult);

            if (shouldEscalate)
            {
                var ticket = await _businessLogic.HandleTicketAsync(interaction, intentResult);
                ticketId = ticket.TicketId;
                replyText = _businessLogic.BuildTicketReply(ticket, intentResult.Intent, intentResult.DetectedDialect);
                
                // Log escalation event
                if (_auditLogService != null && ticket.TicketType == "HumanEscalation")
                {
                    await _auditLogService.LogInteractionActionAsync(
                        businessId: interaction.BusinessId,
                        action: $"EscalateToHuman_{intentResult.Intent}",
                        interactionId: interaction.InteractionId,
                        userId: null // AI-triggered escalation
                    );
                }
            }
            else
            {
                switch (intentResult.Intent)
                {
                    case "CreateOrder":
                        {
                            var orderResult = await _businessLogic.HandleCreateOrderWithDeliveryCheckAsync(interaction, intentResult);
                            orderId = orderResult.Order?.OrderId;
                            cartSummary = orderResult.Cart;
                            recommendations = orderResult.Recommendations;
                            hasDeliveryDelay = orderResult.HasDeliveryDelay;
                            alternativeTimeSlots = orderResult.AlternativeTimeSlots;
                            interaction.InteractionType = "Order";
                            interaction.RelatedOrderId = orderId;
                            replyText = _businessLogic.BuildOrderReply(
                                (orderResult.Order, orderResult.Cart, orderResult.Recommendations),
                                hasDeliveryDelay,
                                alternativeTimeSlots,
                                intentResult.DetectedDialect);
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
            }

            _unitOfWork.Interactions.Update(interaction);
            await _unitOfWork.CompleteAsync();

            // 6) Store AI reply message
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

            // 7) Convert text reply to audio (Text-to-Speech)
            string? replyAudio = null;
            string? replyAudioFormat = null;
            var settings = await _settingService.GetByBusinessIdAsync(interaction.BusinessId);
            if (settings != null)
            {
                // TODO: Integrate with text-to-speech service (Azure TTS, Google TTS, etc.)
                // replyAudio = await ConvertTextToAudioAsync(replyText, settings, intentResult.DetectedDialect);
                // replyAudioFormat = "audio/wav"; // or based on settings
            }

            // 8) Return response (with audio for Voice)
            return new CustomerChatResponseDTO
            {
                InteractionId = interaction.InteractionId,
                ReplyText = replyText,
                ReplyAudio = replyAudio,
                ReplyAudioFormat = replyAudioFormat,
                OrderId = orderId,
                TicketId = ticketId,
                Cart = cartSummary,
                Recommendations = recommendations,
                HasDeliveryDelay = hasDeliveryDelay,
                AlternativeTimeSlots = alternativeTimeSlots,
                IsInterrupted = interaction.Status == "Interrupted"
            };
        }

        public async Task<Interaction> InitializeVoiceSessionAsync(string businessId, string? customerId, string callSessionId)
        {
            var interaction = new Interaction
            {
                InteractionId = Guid.NewGuid().ToString(),
                BusinessId = businessId,
                CustomerId = customerId ?? Guid.NewGuid().ToString(),
                Channel = "Voice",
                CallSessionId = callSessionId,
                Status = "Open",
                StartedAt = DateTime.UtcNow
            };

            await _unitOfWork.Interactions.AddAsync(interaction);
            await _unitOfWork.CompleteAsync();

            return interaction;
        }

        public async Task MarkInteractionInterruptedAsync(string interactionId)
        {
            var interaction = await _unitOfWork.Interactions.GetByIdAsync(interactionId);
            if (interaction == null)
                throw new ArgumentException($"Interaction with id '{interactionId}' not found.");

            interaction.Status = "Interrupted";
            _unitOfWork.Interactions.Update(interaction);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<Feedback> SubmitFeedbackAsync(VoiceFeedbackDTO feedbackDto)
        {
            var interaction = await _unitOfWork.Interactions.GetByIdAsync(feedbackDto.InteractionId);
            if (interaction == null)
                throw new ArgumentException($"Interaction with id '{feedbackDto.InteractionId}' not found.");

            var feedback = new Feedback
            {
                FeedbackId = Guid.NewGuid().ToString(),
                InteractionId = feedbackDto.InteractionId,
                CustomerId = interaction.CustomerId,
                Rating = feedbackDto.Rating,
                Comment = feedbackDto.Comment ?? string.Empty,
                CreatedAt = DateTime.UtcNow,
                SentimentScore = MapRatingToSentiment(feedbackDto.Rating)
            };

            await _unitOfWork.Feedbacks.AddAsync(feedback);
            await _unitOfWork.CompleteAsync();

            return feedback;
        }

        public async Task<VoiceSettingsDTO?> GetVoiceSettingsAsync(string businessId)
        {
            var settings = await _settingService.GetByBusinessIdAsync(businessId);
            if (settings == null)
                return null;

            return new VoiceSettingsDTO
            {
                AgentVoice = settings.AgentVoice,
                AgentVoiceProvider = settings.AgentVoiceProvider,
                AgentVoiceSpeed = settings.AgentVoiceSpeed,
                AgentVoicePitch = settings.AgentVoicePitch,
                AgentVoiceLanguage = settings.AgentVoiceLanguage
            };
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
                {
                    // Update CallSessionId if provided
                    if (!string.IsNullOrWhiteSpace(request.CallSessionId))
                    {
                        existing.CallSessionId = request.CallSessionId;
                        _unitOfWork.Interactions.Update(existing);
                        await _unitOfWork.CompleteAsync();
                    }
                    return existing;
                }
            }

            var interaction = new Interaction
            {
                InteractionId = Guid.NewGuid().ToString(),
                BusinessId = request.BusinessId,
                CustomerId = request.CustomerId ?? Guid.NewGuid().ToString(),
                Channel = channel,
                CallSessionId = request.CallSessionId, // For Voice calls
                Status = "Open",
                StartedAt = DateTime.UtcNow
            };

            await _unitOfWork.Interactions.AddAsync(interaction);
            await _unitOfWork.CompleteAsync();

            return interaction;
        }

        private double MapRatingToSentiment(int rating)
        {
            // Map 1-5 rating to sentiment score (-1 to 1)
            return (rating - 3) / 2.0; // 1 -> -1, 2 -> -0.5, 3 -> 0, 4 -> 0.5, 5 -> 1
        }

        private async Task<string> ConvertAudioToTextAsync(string audioDataBase64, string? audioFormat)
        {
            // TODO: Integrate with speech-to-text service (Azure Speech, Google Speech-to-Text, etc.)
            // For now, return placeholder
            await Task.CompletedTask;
            return "[Audio converted to text - integrate with speech-to-text service]";
        }

        #endregion
    }
}

