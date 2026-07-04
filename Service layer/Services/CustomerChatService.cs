using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Domain_layer.Interfaces;
using Domain_layer.Models;
using Domain_layer.enums;
using Service_layer.DTOS.AiChat;
using Service_layer.DTOS.Chat;
using Service_layer.DTOS.Notification;
using Service_layer.Services_Interfaces;
using Microsoft.Extensions.Logging;

namespace Service_layer.Services
{
    /// <summary>
    /// Service for handling customer text chat interactions (WebChat channel).
    /// Handles text messages, intent detection, orders, tickets, and recommendations.
    /// </summary>
    public class CustomerChatService : ICustomerChatService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISettingService _settingService;
        private readonly IAuditLogService? _auditLogService;
        private readonly ISentimentService? _sentimentService;
        private readonly INotificationService? _notificationService;
        private readonly IAiChatService _aiChatService;
        private readonly ILogger<CustomerChatService>? _logger;
        private readonly CustomerInteractionBusinessLogic _businessLogic;

        public CustomerChatService(
            IUnitOfWork unitOfWork,
            ISettingService settingService,
            IAiChatService aiChatService,
            IAuditLogService? auditLogService = null,
            ISentimentService? sentimentService = null,
            INotificationService? notificationService = null,
            ILogger<CustomerChatService>? logger = null)
        {
            _unitOfWork    = unitOfWork;
            _settingService = settingService;
            _aiChatService  = aiChatService;
            _auditLogService = auditLogService;
            _sentimentService = sentimentService;
            _notificationService = notificationService;
            _logger        = logger;
            _businessLogic  = new CustomerInteractionBusinessLogic(unitOfWork, auditLogService);
        }

        public async Task<CustomerChatResponseDTO> HandleMessageAsync(CustomerChatRequestDTO request)
        {
            // channel is always forced to "WebChat" by the controller — no need to re-check here
            if (string.IsNullOrWhiteSpace(request.Message))
                throw new ArgumentException("Message is required for Chat.");

            // ── 1) Get or create interaction ────────────────────────────────────
            var (interaction, _) = await GetOrCreateInteractionAsync(request, "WebChat");

            // ── 2) Persist customer message ─────────────────────────────────────
            var customerMessage = new Message
            {
                MessageId     = Guid.NewGuid().ToString(),
                InteractionId = interaction.InteractionId,
                SenderType    = "Customer",
                Content       = request.Message,
                SentAt        = DateTime.UtcNow
            };

            await _unitOfWork.Messages.AddAsync(customerMessage);
            await _unitOfWork.CompleteAsync();

            // ── 3) Call AI — single round-trip gets reply + all intent signals ──
            //
            //  CONTRACT:
            //    AI  → reply, order_detected, order_finalized, order_details,
            //           ticket_detected, ticket_details,
            //           escalation_requested, feedback_requested
            //    Backend → creates orders/tickets/escalations in DB based on those flags
            //    Backend → ignores AI-side system_events (AI internal tracking only)
            //
            AiChatResponseDTO aiResponse;
            try
            {
                aiResponse = await _aiChatService.SendMessageAsync(new AiChatRequestDTO
                {
                    SessionId  = interaction.InteractionId,
                    BusinessId = interaction.BusinessId,
                    Message    = request.Message!
                });
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex,
                    "[CustomerChatService] AI call failed for InteractionId={InteractionId} BusinessId={BusinessId}",
                    interaction.InteractionId, interaction.BusinessId);
                aiResponse = new AiChatResponseDTO
                {
                    Reply = "عذراً، حدث خطأ مؤقت. يرجى المحاولة مرة أخرى."
                };
            }

            // ── 4) Map AI signals → DetectedIntentResultDTO (for audit/sentiment) ─
            var intentResult = MapAiResponseToIntent(aiResponse);

            customerMessage.Intent          = intentResult.Intent;
            customerMessage.ConfidenceScore = intentResult.Confidence;
            customerMessage.AiMetadataJson  = System.Text.Json.JsonSerializer.Serialize(intentResult);
            _unitOfWork.Messages.Update(customerMessage);
            await _unitOfWork.CompleteAsync();

            // ── 4.5) Sentiment analysis ─────────────────────────────────────────
            if (_sentimentService != null)
            {
                try
                {
                    await _sentimentService.AnalyzeSentimentAsync(
                        customerMessage.MessageId,
                        request.Message,
                        intentResult.DetectedLanguage ?? "ar");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Sentiment analysis failed: {ex.Message}");
                }
            }

            // ── 5) Backend DB actions driven purely by AI signal flags ───────────
            string? orderId          = null;
            string? ticketId         = null;
            ChatCartSummaryDTO? cart = null;
            var recommendations      = new List<RecommendationItemDTO>();

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

                // Best-effort: surface this in the notification bell immediately so an
                // agent doesn't have to stumble on it by browsing the Tickets list.
                if (_notificationService != null)
                {
                    try
                    {
                        await _notificationService.CreateAsync(new NotificationCreateDTO
                        {
                            Title = "Customer needs a human agent",
                            Message = $"Ticket #{ticket.TicketId.Substring(0, 8)} — {ticket.Subject}",
                            BusinessId = interaction.BusinessId,
                            UserId = null
                        });
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Escalation notification failed: {ex.Message}");
                    }
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
                var orderResult = await _businessLogic.HandleCreateOrderAsync(
                    interaction, aiResponse.OrderDetails);

                // Only tag the interaction as an order when one was actually created.
                // If ALL cart items were unavailable/unmatched, HandleCreateOrderAsync
                // returns a null Order — setting InteractionType then would be wrong.
                if (orderResult.Order != null)
                {
                    orderId = orderResult.Order.OrderId;
                    interaction.InteractionType = "Order";
                    interaction.RelatedOrderId  = orderId;
                }

                cart            = orderResult.Cart;
                recommendations = orderResult.Recommendations;
            }
            // order_detected but NOT order_finalized → cart still being built; no DB action

            _unitOfWork.Interactions.Update(interaction);
            await _unitOfWork.CompleteAsync();

            // ── 6) Reply text — AI owns this entirely ────────────────────────────
            var replyText = aiResponse.GetReplyText();

            // ── 7) Persist AI reply message ──────────────────────────────────────
            var aiMessage = new Message
            {
                MessageId     = Guid.NewGuid().ToString(),
                InteractionId = interaction.InteractionId,
                SenderType    = "AI",
                Content       = replyText,
                SentAt        = DateTime.UtcNow,
                Intent        = intentResult.Intent,
                ConfidenceScore = intentResult.Confidence
            };

            await _unitOfWork.Messages.AddAsync(aiMessage);
            await _unitOfWork.CompleteAsync();

            // ── 8) Return ────────────────────────────────────────────────────────
            return new CustomerChatResponseDTO
            {
                InteractionId     = interaction.InteractionId,
                ReplyText         = replyText,
                OrderId           = orderId,
                TicketId          = ticketId,
                FeedbackRequested = aiResponse.FeedbackRequested,
                Cart              = cart,
                Recommendations   = recommendations,
                IsInterrupted     = false
            };
        }

        /// <summary>
        /// Maps AI response flags to DetectedIntentResultDTO.
        /// Used only for audit logging and sentiment analysis compatibility —
        /// business logic is now driven by AI flags directly, not this DTO.
        /// </summary>
        /// <summary>
        /// Translates AI response flags into DetectedIntentResultDTO.
        /// Used only for audit logging and sentiment analysis — NOT for any business decisions.
        /// All values come from the AI response; nothing is guessed or hardcoded.
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

        private async Task<(Interaction interaction, bool isNew)> GetOrCreateInteractionAsync(
            CustomerChatRequestDTO request, string channel)
        {
            // ── Validate business exists before creating any records ────────────
            // A Business carries two GUIDs: the primary key `Id` and a separate
            // `BusinessId` column, and the API exposes both to clients. Resolve by
            // the primary key first, then fall back to the `BusinessId` column so a
            // caller that sent either value still resolves to the same business.
            var business = await _unitOfWork.Businesses.GetByIdAsync(request.BusinessId)
                ?? await _unitOfWork.Businesses.FirstOrDefaultAsync(b => b.BusinessId == request.BusinessId);
            if (business == null)
                throw new ArgumentException($"Business '{request.BusinessId}' not found.");

            // Normalize to the canonical primary key for all downstream records so
            // customers, interactions and the AI call are keyed consistently — the
            // AI knowledge base is synced by `Business.Id`.
            var businessId = business.Id;

            // ── Resume existing interaction (must belong to this business) ──────
            if (!string.IsNullOrWhiteSpace(request.InteractionId))
            {
                var existing = await _unitOfWork.Interactions.GetByIdAsync(request.InteractionId);
                if (existing != null && existing.BusinessId == businessId)
                {
                    // Reject messages on a closed interaction — session_id must never be reused
                    if (existing.IsEnded == true)
                        throw new InvalidOperationException(
                            $"Interaction '{request.InteractionId}' is already ended. Start a new interaction.");

                    return (existing, false);
                }
                // wrong id or wrong business → fall through and create a new interaction
            }

            var customerId = await EnsureCustomerAsync(businessId, request.CustomerId);

            var interaction = new Interaction
            {
                InteractionId = Guid.NewGuid().ToString(),
                BusinessId    = businessId,
                CustomerId    = customerId,
                Channel       = channel,
                Status        = "Open",
                StartedAt     = DateTime.UtcNow
            };

            await _unitOfWork.Interactions.AddAsync(interaction);
            await _unitOfWork.CompleteAsync();

            return (interaction, true);
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

        #endregion
    }
}

