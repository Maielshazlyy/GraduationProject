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
    /// Shared business logic for both Chat and Voice interactions.
    /// Handles orders, tickets, recommendations, and general queries.
    /// </summary>
    public class CustomerInteractionBusinessLogic
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAiChatService? _aiChatService;
        private readonly IAuditLogService? _auditLogService;

        public CustomerInteractionBusinessLogic(
            IUnitOfWork unitOfWork,
            IAiChatService? aiChatService = null,
            IAuditLogService? auditLogService = null)
        {
            _unitOfWork = unitOfWork;
            _aiChatService = aiChatService;
            _auditLogService = auditLogService;
        }

        /// <summary>
        /// Loads the business's menu + knowledge base from the DB and pushes them to
        /// the AI to prime a new session. Called once when a conversation starts.
        /// Failure is non-fatal — the conversation proceeds even if priming fails.
        /// </summary>
        public async Task PrimeAiSessionAsync(Interaction interaction)
        {
            if (_aiChatService == null) return;

            var business  = await _unitOfWork.Businesses.GetByIdAsync(interaction.BusinessId);

            var menuItems = (await _unitOfWork.MenuItems.GetByBusinessIdAsync(interaction.BusinessId))
                .Select(mi => new AiMenuItemDTO
                {
                    MenuItemId  = mi.MenuItemId,
                    Name        = mi.Name,
                    Description = mi.Description,
                    Price       = mi.Price,
                    Category    = mi.MenuCategory?.Name,
                    IsAvailable = mi.IsAvailable
                })
                .ToList();

            var knowledge = (await _unitOfWork.KnowledgeBases.GetByBusinessIdAsync(interaction.BusinessId))
                .Where(k => k.IsActive)
                .Select(k => new AiKnowledgeEntryDTO
                {
                    Question = k.Question,
                    Answer   = k.Answer,
                    IsFaq    = k.IsFAQ
                })
                .ToList();

            var init = new AiSessionInitDTO
            {
                SessionId     = interaction.InteractionId,
                BusinessId    = interaction.BusinessId,
                BusinessName  = business?.Name,
                Menu          = menuItems,
                KnowledgeBase = knowledge
            };

            await _aiChatService.InitSessionAsync(init);
        }

        /// <summary>
        /// Creates an order in the DB using the confirmed cart from the AI response.
        /// Item names are matched against the business menu to resolve MenuItemId.
        /// Authoritative prices always come from the menu DB, not from the AI.
        /// </summary>
        public async Task<(Order? Order, ChatCartSummaryDTO? Cart, List<RecommendationItemDTO> Recommendations)>
            HandleCreateOrderAsync(Interaction interaction, AiOrderDetailsDTO aiOrderDetails)
        {
            var menuItems = (await _unitOfWork.MenuItems
                    .GetByBusinessIdAsync(interaction.BusinessId))
                .Where(mi => mi.IsAvailable)
                .ToList();

            if (!menuItems.Any())
                return (null, null, new List<RecommendationItemDTO>());

            var orderItems  = new List<OrderItem>();
            var cartItems   = new List<ChatCartItemDTO>();
            decimal total   = 0;
            MenuItem? firstItem = null;

            foreach (var aiItem in aiOrderDetails.Items)
            {
                if (string.IsNullOrWhiteSpace(aiItem.Name)) continue;

                // Exact match only (trimmed, case-insensitive). The AI is given the
                // menu and must echo the exact Name — no fuzzy/substring matching,
                // which could silently resolve to the wrong item.
                var menuItem = menuItems.FirstOrDefault(mi =>
                    mi.Name.Trim().Equals(aiItem.Name.Trim(), StringComparison.OrdinalIgnoreCase));

                if (menuItem == null) continue; // name not found in menu → skip

                firstItem ??= menuItem;

                var linePrice = menuItem.Price * aiItem.Quantity;
                total += linePrice;

                orderItems.Add(new OrderItem
                {
                    OrderItemId = Guid.NewGuid().ToString(),
                    MenuItemId  = menuItem.MenuItemId,
                    Quantity    = aiItem.Quantity,
                    UnitPrice   = menuItem.Price
                });

                cartItems.Add(new ChatCartItemDTO
                {
                    MenuItemId = menuItem.MenuItemId,
                    Name       = menuItem.Name,
                    Quantity   = aiItem.Quantity,
                    UnitPrice  = menuItem.Price
                });
            }

            if (!orderItems.Any())
                return (null, null, new List<RecommendationItemDTO>());

            var order = new Order
            {
                OrderId    = Guid.NewGuid().ToString(),
                BusinessId = interaction.BusinessId,
                CustomerId = interaction.CustomerId,
                CreatedAt  = DateTime.UtcNow,
                Status     = OrderStatus.Pending,
                TotalPrice = total,
                OrderItems = orderItems
            };

            await _unitOfWork.Orders.AddAsync(order);
            await _unitOfWork.CompleteAsync();

            if (_auditLogService != null)
            {
                await _auditLogService.LogOrderActionAsync(
                    businessId: interaction.BusinessId,
                    action:     "CreateOrderFromChat",
                    orderId:    order.OrderId,
                    userId:     null);
            }

            var cart = new ChatCartSummaryDTO
            {
                TotalPrice = total,
                Items      = cartItems
            };

            var recommendations = firstItem != null
                ? BuildRecommendationsForOrder(menuItems, firstItem, order)
                : new List<RecommendationItemDTO>();

            return (order, cart, recommendations);
        }

        /// <summary>
        /// Legacy overload — kept for Voice path and tests that still pass DetectedIntentResultDTO.
        /// Falls back to picking the first available menu item (to be improved when Voice AI is updated).
        /// </summary>
        public async Task<(Order? Order, ChatCartSummaryDTO? Cart, List<RecommendationItemDTO> Recommendations)>
            HandleCreateOrderAsync(Interaction interaction, DetectedIntentResultDTO intent)
        {
            var menuItems = (await _unitOfWork.MenuItems
                    .GetByBusinessIdAsync(interaction.BusinessId))
                .Where(mi => mi.IsAvailable)
                .ToList();

            if (!menuItems.Any())
                return (null, null, new List<RecommendationItemDTO>());

            var mainItem = menuItems.First();

            var order = new Order
            {
                OrderId    = Guid.NewGuid().ToString(),
                BusinessId = interaction.BusinessId,
                CustomerId = interaction.CustomerId,
                CreatedAt  = DateTime.UtcNow,
                Status     = OrderStatus.Pending,
                TotalPrice = mainItem.Price,
                OrderItems = new List<OrderItem>
                {
                    new OrderItem
                    {
                        OrderItemId = Guid.NewGuid().ToString(),
                        MenuItemId  = mainItem.MenuItemId,
                        Quantity    = 1,
                        UnitPrice   = mainItem.Price
                    }
                }
            };

            await _unitOfWork.Orders.AddAsync(order);
            await _unitOfWork.CompleteAsync();

            if (_auditLogService != null)
            {
                await _auditLogService.LogOrderActionAsync(
                    businessId: interaction.BusinessId,
                    action:     "CreateOrderFromChat",
                    orderId:    order.OrderId,
                    userId:     null);
            }

            var cart = new ChatCartSummaryDTO
            {
                TotalPrice = mainItem.Price,
                Items      = new List<ChatCartItemDTO>
                {
                    new ChatCartItemDTO
                    {
                        MenuItemId = mainItem.MenuItemId,
                        Name       = mainItem.Name,
                        Quantity   = 1,
                        UnitPrice  = mainItem.Price
                    }
                }
            };

            return (order, cart, BuildRecommendationsForOrder(menuItems, mainItem, order));
        }

        /// <summary>
        /// Creates an order from the AI-confirmed cart and runs a delivery-capacity
        /// check (Voice flow). Marks the order DelayedRisk if a delay is detected.
        /// </summary>
        public async Task<(Order? Order, ChatCartSummaryDTO? Cart, List<RecommendationItemDTO> Recommendations, bool HasDeliveryDelay, List<string>? AlternativeTimeSlots)>
            HandleCreateOrderWithDeliveryCheckAsync(Interaction interaction, AiOrderDetailsDTO aiOrderDetails)
        {
            var orderResult = await HandleCreateOrderAsync(interaction, aiOrderDetails);

            var delayCheck = await CheckDeliveryCapacityAsync(interaction.BusinessId);

            if (delayCheck.HasDelay && orderResult.Order != null)
            {
                orderResult.Order.Status = OrderStatus.DelayedRisk;
                _unitOfWork.Orders.Update(orderResult.Order);
                await _unitOfWork.CompleteAsync();
            }

            return (orderResult.Order, orderResult.Cart, orderResult.Recommendations, delayCheck.HasDelay, delayCheck.AlternativeTimeSlots);
        }

        /// <summary>Legacy overload — kept for callers still passing DetectedIntentResultDTO.</summary>
        public async Task<(Order? Order, ChatCartSummaryDTO? Cart, List<RecommendationItemDTO> Recommendations, bool HasDeliveryDelay, List<string>? AlternativeTimeSlots)>
            HandleCreateOrderWithDeliveryCheckAsync(Interaction interaction, DetectedIntentResultDTO intent)
        {
            var orderResult = await HandleCreateOrderAsync(interaction, intent);

            var delayCheck = await CheckDeliveryCapacityAsync(interaction.BusinessId);

            if (delayCheck.HasDelay && orderResult.Order != null)
            {
                orderResult.Order.Status = OrderStatus.DelayedRisk;
                _unitOfWork.Orders.Update(orderResult.Order);
                await _unitOfWork.CompleteAsync();
            }

            return (orderResult.Order, orderResult.Cart, orderResult.Recommendations, delayCheck.HasDelay, delayCheck.AlternativeTimeSlots);
        }

        private async Task<(bool HasDelay, List<string>? AlternativeTimeSlots)> CheckDeliveryCapacityAsync(string businessId)
        {
            // TODO: Implement actual delivery capacity check
            // - Check current order volume
            // - Check peak hours
            // - Check delivery team availability
            await Task.CompletedTask;
            
            // Example: Check if it's peak hour (simplified)
            var currentHour = DateTime.UtcNow.Hour;
            bool isPeakHour = currentHour >= 18 && currentHour <= 22; // 6 PM - 10 PM

            if (isPeakHour)
            {
                return (true, new List<string> { "30 minutes later", "1 hour later", "Tomorrow morning" });
            }

            return (false, null);
        }

        public List<RecommendationItemDTO> BuildRecommendationsForOrder(
            List<MenuItem> allItems,
            MenuItem mainItem,
            Order order)
        {
            // Backend-driven recommendations
            var recs = new List<RecommendationItemDTO>();
            var lowerName = mainItem.Name.ToLowerInvariant();

            if (lowerName.Contains("burger"))
            {
                var sides = allItems
                    .Where(mi => mi.MenuItemId != mainItem.MenuItemId &&
                                 (mi.Name.ToLower().Contains("fries") ||
                                  mi.Name.ToLower().Contains("بطاطس")))
                    .Take(1)
                    .ToList();

                var drinks = allItems
                    .Where(mi => mi.MenuItemId != mainItem.MenuItemId &&
                                 (mi.Name.ToLower().Contains("cola") ||
                                  mi.Name.ToLower().Contains("coke") ||
                                  mi.Name.ToLower().Contains("بيبسي")))
                    .Take(1)
                    .ToList();

                foreach (var side in sides)
                {
                    recs.Add(new RecommendationItemDTO
                    {
                        MenuItemId = side.MenuItemId,
                        Name = side.Name,
                        Price = side.Price,
                        Reason = "Perfect side with your burger (بطاطس مناسبة مع البرجر)."
                    });
                }

                foreach (var drink in drinks)
                {
                    recs.Add(new RecommendationItemDTO
                    {
                        MenuItemId = drink.MenuItemId,
                        Name = drink.Name,
                        Price = drink.Price,
                        Reason = "Popular drink with burgers (مشروب مناسب مع البرجر)."
                    });
                }
            }

            return recs;
        }

        public async Task<string> HandleModifyOrderAsync(Interaction interaction, DetectedIntentResultDTO intent)
        {
            // TODO: Implement order modification logic
            await Task.CompletedTask;
            return "نقدر نعدل الطلب. قولي إيه اللي عايز تغيره.";
        }

        public async Task<string> HandleCancelOrderAsync(Interaction interaction, DetectedIntentResultDTO intent)
        {
            intent.Entities.TryGetValue("orderId", out var orderId);
            orderId ??= interaction.RelatedOrderId;

            if (string.IsNullOrWhiteSpace(orderId))
            {
                return "مش لاقي رقم الطلب. من فضلك ابعتلي رقم الطلب علشان أقدر ألغيه.";
            }

            var order = await _unitOfWork.Orders.GetByIdAsync(orderId);
            if (order == null)
            {
                return $"مش لاقي طلب بالرقم: {orderId}.";
            }

            order.Status = OrderStatus.Cancelled;
            _unitOfWork.Orders.Update(order);
            await _unitOfWork.CompleteAsync();

            // Log order cancellation from chat/voice
            if (_auditLogService != null)
            {
                await _auditLogService.LogOrderActionAsync(
                    businessId: interaction.BusinessId,
                    action: "CancelOrderFromChat",
                    orderId: orderId,
                    userId: null // Customer-initiated
                );
            }

            return $"تم إلغاء الطلب رقم {orderId} بنجاح.";
        }

        /// <summary>
        /// Creates a support ticket or escalation ticket using AI-supplied details.
        /// When isEscalation is true (or AI flags escalation_requested), creates a HumanEscalation ticket.
        /// </summary>
        public async Task<Ticket> HandleTicketAsync(
            Interaction interaction,
            DetectedIntentResultDTO intent,
            AiTicketDetailsDTO? aiTicketDetails = null,
            bool isEscalation = false)
        {
            var isHumanEscalation = isEscalation ||
                intent.Intent == "RequestHumanAgent" ||
                intent.RequiresEscalation ||
                string.Equals(intent.ComplexityLevel, "High", StringComparison.OrdinalIgnoreCase);

            // Subject: prefer AI-supplied, fall back to generic
            var subject = !string.IsNullOrWhiteSpace(aiTicketDetails?.Subject)
                ? aiTicketDetails!.Subject
                : (isHumanEscalation ? "Customer escalated to human agent" : "Customer complaint from chat/voice");

            // TicketType: map AI category to domain type, default to QualityIssue
            var ticketType = isHumanEscalation
                ? "HumanEscalation"
                : MapCategoryToTicketType(aiTicketDetails?.Category);

            // Priority: prefer AI-supplied, fall back to intent priority
            var rawPriority = !string.IsNullOrWhiteSpace(aiTicketDetails?.Priority)
                ? aiTicketDetails!.Priority
                : (intent.PriorityLevel ?? (isHumanEscalation ? "High" : "Normal"));

            // Normalize to PascalCase (AI returns lowercase "high" etc.)
            var priority = char.ToUpper(rawPriority[0]) + rawPriority.Substring(1).ToLower();

            var ticket = new Ticket
            {
                Id                  = Guid.NewGuid().ToString(),
                TicketId            = Guid.NewGuid().ToString(),
                BusinessId          = interaction.BusinessId,
                CustomerId          = interaction.CustomerId,
                InteractionId       = interaction.InteractionId,
                RelatedOrderId      = interaction.RelatedOrderId,
                TicketType          = ticketType,
                Subject             = subject,
                Status              = isHumanEscalation ? "Escalated" : "Open",
                PriorityLevel       = priority,
                EscalationConfidence = isHumanEscalation ? intent.Confidence : null,
                EscalationReason    = isHumanEscalation
                    ? (intent.EscalationReason ?? aiTicketDetails?.Description ?? "Escalated by AI decision logic.")
                    : null,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Tickets.AddAsync(ticket);
            await _unitOfWork.CompleteAsync();

            if (isHumanEscalation)
            {
                interaction.InteractionType  = "Ticket";
                interaction.RelatedTicketId  = ticket.TicketId;
                interaction.Status           = "Escalated";
                _unitOfWork.Interactions.Update(interaction);
                await _unitOfWork.CompleteAsync();
            }

            return ticket;
        }

        /// <summary>Maps AI category strings to Ticket.TicketType domain values.</summary>
        private static string MapCategoryToTicketType(string? category)
        {
            if (string.IsNullOrWhiteSpace(category)) return "QualityIssue";
            return category.ToLowerInvariant() switch
            {
                "complaint"   => "QualityIssue",
                "quality"     => "QualityIssue",
                "delivery"    => "LateDelivery",
                "late"        => "LateDelivery",
                "wrong_order" => "WrongOrder",
                "missing"     => "MissingItem",
                "payment"     => "PaymentIssue",
                _             => "QualityIssue"
            };
        }

        public async Task<string> HandleAskOrderStatusAsync(Interaction interaction, DetectedIntentResultDTO intent)
        {
            intent.Entities.TryGetValue("orderId", out var orderId);
            orderId ??= interaction.RelatedOrderId;

            if (string.IsNullOrWhiteSpace(orderId))
            {
                return "مش لاقي رقم الطلب. من فضلك ابعتلي رقم الطلب علشان أقدر أساعدك في تتبع حالته.";
            }

            var order = await _unitOfWork.Orders.GetByIdAsync(orderId);
            if (order == null)
            {
                return $"مش لاقي طلب بالرقم: {orderId}. تأكد من الرقم وحاول تاني.";
            }

            return $"حالة طلبك الحالي هي: {order.Status}. الإجمالي: {order.TotalPrice}.";
        }

        public async Task<string> HandleAskProductsAsync(Interaction interaction, DetectedIntentResultDTO intent)
        {
            var items = (await _unitOfWork.MenuItems.GetByBusinessIdAsync(interaction.BusinessId))
                .Where(mi => mi.IsAvailable)
                .Take(10)
                .ToList();

            if (!items.Any())
            {
                return "حالياً مفيش منتجات متاحة في المنيو.";
            }

            var lines = items
                .Select(mi => $"- {mi.Name}: {mi.Price} ({mi.Description})");

            return "دي بعض الأصناف المتاحة في المنيو عندنا:\n" + string.Join("\n", lines);
        }

        public Task<string> HandleGeneralQuestionAsync(Interaction interaction, DetectedIntentResultDTO intent)
        {
            // Placeholder for richer logic (FAQs, working hours, etc.)
            return Task.FromResult("أقدر أساعدك في الأسئلة عن المنيو، حالة الطلب، أو فتح تذكرة شكوى. كلمّني تقول إيه اللي محتاجه 😊");
        }

        public string BuildOrderReply(
            (Order? Order, ChatCartSummaryDTO? Cart, List<RecommendationItemDTO> Recommendations) result,
            bool hasDelay,
            List<string>? timeSlots,
            string? dialect)
        {
            if (result.Order == null || result.Cart == null)
            {
                return "حصل مشكلة وأنا بحاول أعمل الطلب. حاول تاني من فضلك.";
            }

            var order = result.Order;
            var cart = result.Cart;

            var greeting = dialect == "Egyptian" ? "تمام" : "تم";
            var text = $"{greeting}، سجلت لك طلب جديد بقيمة إجمالية {cart.TotalPrice}.\n" +
                      $"الطلب رقم (ID): {order.OrderId}.\n";

            if (hasDelay && timeSlots != null && timeSlots.Any())
            {
                text += "\n⚠️ ملاحظة: في تأخير محتمل في التوصيل بسبب الكثافة الحالية.\n";
                text += "عندنا أوقات بديلة:\n";
                foreach (var slot in timeSlots)
                {
                    text += $"- {slot}\n";
                }
                text += "أو ممكن نرفع أولوية الطلب. عايز إيه؟";
            }

            if (result.Recommendations.Any())
            {
                text += "\nعندي شوية إضافات ممكن تعجبك مع الطلب:\n";
                foreach (var rec in result.Recommendations)
                {
                    text += $"- {rec.Name} ({rec.Price}): {rec.Reason}\n";
                }
            }

            return text;
        }

        public string BuildTicketReply(Ticket ticket, string intent, string? dialect)
        {
            var greeting = dialect == "Egyptian" ? "تمام" : "تم";

            if (ticket.TicketType == "HumanEscalation" || intent == "RequestHumanAgent")
            {
                return $"{greeting}، هحوّل المحادثة دلوقتي لمتخصص من خدمة العملاء. رقم التذكرة: {ticket.TicketId}. من فضلك انتظر لحظات لحد ما حد من الفريق ينضم ليك.";
            }

            return $"{greeting}، تم فتح تذكرة شكوى برقم: {ticket.TicketId}. هنراجع المشكلة ونتواصل معاك لحلّها في أقرب وقت ممكن.";
        }
    }
}

