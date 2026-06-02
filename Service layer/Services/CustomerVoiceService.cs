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
    /// Service for handling customer voice call interactions (Voice channel).
    /// Handles audio messages, speech-to-text, text-to-speech, call sessions,
    /// delivery delays, recovery, and feedback collection.
    /// 
    /// ⚠️ NOTE: This REST API endpoint is a PLACEHOLDER.
    /// The actual Voice implementation will use WebSocket (SignalR) for real-time streaming.
    /// Audio will be streamed directly from the device, not saved as files.
    /// </summary>
    public class CustomerVoiceService : ICustomerVoiceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISettingService _settingService;
        private readonly IAuditLogService? _auditLogService;
        private readonly ISentimentService? _sentimentService;
        private readonly IAiChatService _aiChatService;
        private readonly CustomerInteractionBusinessLogic _businessLogic;

        public CustomerVoiceService(
            IUnitOfWork unitOfWork,
            ISettingService settingService,
            IAiChatService aiChatService,
            IAuditLogService? auditLogService = null,
            ISentimentService? sentimentService = null)
        {
            _unitOfWork      = unitOfWork;
            _settingService  = settingService;
            _aiChatService   = aiChatService;
            _auditLogService = auditLogService;
            _sentimentService = sentimentService;
            _businessLogic   = new CustomerInteractionBusinessLogic(unitOfWork, auditLogService: auditLogService);
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

            // 1) Ensure interaction exists (with CallSessionId for Voice)
            var interaction = await GetOrCreateInteractionAsync(request, "Voice");

            // 2) Call AI — STT/TTS are handled by the AI.
            //    Backend sends the raw audio (or text); AI returns the transcript,
            //    the reply text, the reply audio, and all the business signals
            //    (same contract as Chat — see CustomerChatService).
            AiVoiceResponseDTO aiResponse;
            try
            {
                aiResponse = await _aiChatService.SendVoiceAsync(new AiVoiceRequestDTO
                {
                    SessionId   = interaction.InteractionId,
                    BusinessId  = interaction.BusinessId,
                    AudioData   = request.AudioData,
                    AudioFormat = request.AudioFormat,
                    Message     = request.Message
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[CustomerVoiceService] AI call failed: {ex.Message}");
                aiResponse = new AiVoiceResponseDTO
                {
                    Reply = "عذراً، حدث خطأ مؤقت. يرجى المحاولة مرة أخرى."
                };
            }

            // 3) Resolve the customer's words: AI transcript first, then any text sent
            var messageText = !string.IsNullOrWhiteSpace(aiResponse.Transcript)
                ? aiResponse.Transcript!
                : (request.Message ?? string.Empty);

            // 4) Map AI signals → intent DTO (audit/sentiment only)
            var intentResult = MapAiResponseToIntent(aiResponse);

            // 5) Store the customer message (transcribed by AI)
            var customerMessage = new Message
            {
                MessageId       = Guid.NewGuid().ToString(),
                InteractionId   = interaction.InteractionId,
                SenderType      = "Customer",
                Content         = messageText,
                SentAt          = DateTime.UtcNow,
                Intent          = intentResult.Intent,
                ConfidenceScore = intentResult.Confidence,
                AiMetadataJson  = System.Text.Json.JsonSerializer.Serialize(intentResult)
            };

            await _unitOfWork.Messages.AddAsync(customerMessage);
            await _unitOfWork.CompleteAsync();

            // 5.5) Sentiment analysis
            if (_sentimentService != null && !string.IsNullOrWhiteSpace(messageText))
            {
                try
                {
                    await _sentimentService.AnalyzeSentimentAsync(
                        customerMessage.MessageId,
                        messageText,
                        intentResult.DetectedLanguage ?? "ar");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Sentiment analysis failed: {ex.Message}");
                }
            }

            // 5) Backend DB actions driven purely by AI signal flags
            string? orderId = null;
            string? ticketId = null;
            ChatCartSummaryDTO? cartSummary = null;
            var recommendations = new List<RecommendationItemDTO>();
            bool hasDeliveryDelay = false;
            List<string>? alternativeTimeSlots = null;

            if (aiResponse.EscalationRequested)
            {
                var ticket = await _businessLogic.HandleTicketAsync(
                    interaction, intentResult,
                    aiTicketDetails: null,
                    isEscalation: true);

                ticketId = ticket.TicketId;
                // Caller owns all interaction mutations — set them here after HandleTicketAsync
                interaction.InteractionType = "Ticket";
                interaction.RelatedTicketId = ticketId;
                interaction.Status          = "Escalated";

                if (_auditLogService != null)
                {
                    await _auditLogService.LogInteractionActionAsync(
                        businessId:    interaction.BusinessId,
                        action:        "EscalateToHuman",
                        interactionId: interaction.InteractionId,
                        userId:        null);
                }
            }
            else if (aiResponse.TicketDetected)
            {
                var ticket = await _businessLogic.HandleTicketAsync(
                    interaction, intentResult,
                    aiTicketDetails: aiResponse.TicketDetails); // null-safe; HandleTicketAsync uses defaults

                ticketId = ticket.TicketId;
                interaction.InteractionType = "Ticket";
                interaction.RelatedTicketId = ticketId;
            }
            else if (aiResponse.OrderFinalized && aiResponse.OrderDetails != null)
            {
                var orderResult = await _businessLogic.HandleCreateOrderWithDeliveryCheckAsync(
                    interaction, aiResponse.OrderDetails);

                // Only tag the interaction as an order when one was actually created.
                if (orderResult.Order != null)
                {
                    orderId = orderResult.Order.OrderId;
                    interaction.InteractionType = "Order";
                    interaction.RelatedOrderId  = orderId;
                }

                cartSummary          = orderResult.Cart;
                recommendations      = orderResult.Recommendations;
                hasDeliveryDelay     = orderResult.HasDeliveryDelay;
                alternativeTimeSlots = orderResult.AlternativeTimeSlots;
            }
            // order_detected but NOT order_finalized → cart still being built; no DB action

            _unitOfWork.Interactions.Update(interaction);
            await _unitOfWork.CompleteAsync();

            // 6) Reply — AI owns both the text and the TTS audio
            var replyText = aiResponse.GetReplyText();

            // 7) Store AI reply message
            var aiMessage = new Message
            {
                MessageId       = Guid.NewGuid().ToString(),
                InteractionId   = interaction.InteractionId,
                SenderType      = "AI",
                Content         = replyText,
                SentAt          = DateTime.UtcNow,
                Intent          = intentResult.Intent,
                ConfidenceScore = intentResult.Confidence
            };

            await _unitOfWork.Messages.AddAsync(aiMessage);
            await _unitOfWork.CompleteAsync();

            // 8) Return response — reply audio comes straight from the AI (no backend TTS)
            return new CustomerChatResponseDTO
            {
                InteractionId        = interaction.InteractionId,
                ReplyText            = replyText,
                ReplyAudio           = aiResponse.ReplyAudio,
                ReplyAudioFormat     = aiResponse.ReplyAudioFormat,
                OrderId              = orderId,
                TicketId             = ticketId,
                FeedbackRequested    = aiResponse.FeedbackRequested,
                Cart                 = cartSummary,
                Recommendations      = recommendations,
                HasDeliveryDelay     = hasDeliveryDelay,
                AlternativeTimeSlots = alternativeTimeSlots,
                IsInterrupted        = interaction.Status == "Interrupted"
            };
        }

        public async Task<Interaction> InitializeVoiceSessionAsync(string businessId, string? customerId, string callSessionId)
        {
            var resolvedCustomerId = await EnsureCustomerAsync(businessId, customerId);

            var interaction = new Interaction
            {
                InteractionId = Guid.NewGuid().ToString(),
                BusinessId    = businessId,
                CustomerId    = resolvedCustomerId,
                Channel       = "Voice",
                CallSessionId = callSessionId,
                Status        = "Open",
                StartedAt     = DateTime.UtcNow
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

        /// <summary>
        /// Translates AI response flags into DetectedIntentResultDTO.
        /// Used only for audit logging and sentiment analysis — NOT for business decisions.
        /// </summary>
        private static DetectedIntentResultDTO MapAiResponseToIntent(AiChatResponseDTO ai)
        {
            string intent;
            if      (ai.EscalationRequested)                intent = "RequestHumanAgent";
            else if (ai.TicketDetected)                     intent = "Complaint";
            else if (ai.OrderFinalized || ai.OrderDetected) intent = "CreateOrder";
            else                                            intent = "GeneralQuestion";

            return new DetectedIntentResultDTO
            {
                Intent             = intent,
                RequiresAction     = ai.EscalationRequested || ai.TicketDetected || ai.OrderFinalized,
                RequiresEscalation = ai.EscalationRequested,
                PriorityLevel      = ai.TicketDetails?.Priority,
                EscalationReason   = ai.EscalationRequested ? ai.TicketDetails?.Description : null,
                ComplexityLevel    = ai.EscalationRequested ? "High" : null
            };
        }

        private async Task<Interaction> GetOrCreateInteractionAsync(CustomerChatRequestDTO request, string channel)
        {
            // ── Validate business exists before creating any records ────────────
            var business = await _unitOfWork.Businesses.GetByIdAsync(request.BusinessId);
            if (business == null)
                throw new ArgumentException($"Business '{request.BusinessId}' not found.");

            // ── Resume existing interaction (must belong to this business) ──────
            if (!string.IsNullOrWhiteSpace(request.InteractionId))
            {
                var existing = await _unitOfWork.Interactions.GetByIdAsync(request.InteractionId);
                if (existing != null && existing.BusinessId == request.BusinessId)
                {
                    // Reject messages on a closed interaction — session_id must never be reused
                    if (existing.IsEnded == true)
                        throw new InvalidOperationException(
                            $"Interaction '{request.InteractionId}' is already ended. Start a new interaction.");

                    if (!string.IsNullOrWhiteSpace(request.CallSessionId))
                    {
                        existing.CallSessionId = request.CallSessionId;
                        _unitOfWork.Interactions.Update(existing);
                        await _unitOfWork.CompleteAsync();
                    }
                    return existing;
                }
                // wrong id or wrong business → fall through and create a new interaction
            }

            var customerId = await EnsureCustomerAsync(request.BusinessId, request.CustomerId);

            var interaction = new Interaction
            {
                InteractionId = Guid.NewGuid().ToString(),
                BusinessId    = request.BusinessId,
                CustomerId    = customerId,
                Channel       = channel,
                CallSessionId = request.CallSessionId,
                Status        = "Open",
                StartedAt     = DateTime.UtcNow
            };

            await _unitOfWork.Interactions.AddAsync(interaction);
            await _unitOfWork.CompleteAsync();

            return interaction;
        }

        /// <summary>
        /// Returns the existing customer id if valid, otherwise creates a guest Customer
        /// record so the Interaction FK constraint is always satisfied.
        /// </summary>
        private async Task<string> EnsureCustomerAsync(string businessId, string? customerId)
        {
            if (!string.IsNullOrWhiteSpace(customerId))
            {
                var existing = await _unitOfWork.Customers.GetByIdAsync(customerId);
                if (existing != null) return customerId;
            }

            var guest = new Customer
            {
                CustomerId = Guid.NewGuid().ToString(),
                BusinessId = businessId,
                FullName   = "Guest",
                Email      = string.Empty,
                Phone      = string.Empty,
                CreatedAt  = DateTime.UtcNow
            };
            await _unitOfWork.Customers.AddAsync(guest);
            await _unitOfWork.CompleteAsync();
            return guest.CustomerId;
        }

        private double MapRatingToSentiment(int rating)
        {
            // Map 1-5 rating to sentiment score (-1 to 1)
            return (rating - 3) / 2.0; // 1 -> -1, 2 -> -0.5, 3 -> 0, 4 -> 0.5, 5 -> 1
        }

        #endregion
    }
}

