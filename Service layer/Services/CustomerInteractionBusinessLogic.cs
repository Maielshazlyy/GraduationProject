using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Domain_layer.Interfaces;
using Domain_layer.Models;
using Domain_layer.enums;
using Service_layer.DTOS.Chat;

namespace Service_layer.Services
{
    /// <summary>
    /// Shared business logic for both Chat and Voice interactions.
    /// Handles orders, tickets, recommendations, and general queries.
    /// </summary>
    public class CustomerInteractionBusinessLogic
    {
        private readonly IUnitOfWork _unitOfWork;

        public CustomerInteractionBusinessLogic(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<(Order? Order, ChatCartSummaryDTO? Cart, List<RecommendationItemDTO> Recommendations)>
            HandleCreateOrderAsync(Interaction interaction, DetectedIntentResultDTO intent)
        {
            var menuItems = (await _unitOfWork.MenuItems
                    .GetByBusinessIdAsync(interaction.BusinessId))
                .Where(mi => mi.IsAvailable)
                .ToList();

            if (!menuItems.Any())
            {
                return (null, null, new List<RecommendationItemDTO>());
            }

            var mainItem = menuItems.First();

            var order = new Order
            {
                OrderId = Guid.NewGuid().ToString(),
                BusinessId = interaction.BusinessId,
                CustomerId = interaction.CustomerId,
                CreatedAt = DateTime.UtcNow,
                Status = OrderStatus.Pending,
                TotalPrice = mainItem.Price,
                OrderItems = new List<OrderItem>
                {
                    new OrderItem
                    {
                        OrderItemId = Guid.NewGuid().ToString(),
                        MenuItemId = mainItem.MenuItemId,
                        Quantity = 1,
                        UnitPrice = mainItem.Price
                    }
                }
            };

            await _unitOfWork.Orders.AddAsync(order);
            await _unitOfWork.CompleteAsync();

            var cart = new ChatCartSummaryDTO
            {
                TotalPrice = order.TotalPrice,
                Items = new List<ChatCartItemDTO>
                {
                    new ChatCartItemDTO
                    {
                        MenuItemId = mainItem.MenuItemId,
                        Name = mainItem.Name,
                        Quantity = 1,
                        UnitPrice = mainItem.Price
                    }
                }
            };

            var recommendations = BuildRecommendationsForOrder(menuItems, mainItem, order);

            return (order, cart, recommendations);
        }

        public async Task<(Order? Order, ChatCartSummaryDTO? Cart, List<RecommendationItemDTO> Recommendations, bool HasDeliveryDelay, List<string>? AlternativeTimeSlots)>
            HandleCreateOrderWithDeliveryCheckAsync(Interaction interaction, DetectedIntentResultDTO intent)
        {
            var orderResult = await HandleCreateOrderAsync(interaction, intent);
            
            // Check delivery capacity (for Voice orders)
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

            return $"تم إلغاء الطلب رقم {orderId} بنجاح.";
        }

        public async Task<Ticket> HandleTicketAsync(Interaction interaction, DetectedIntentResultDTO intent)
        {
            var ticketType = intent.Intent == "RequestHumanAgent"
                ? "HumanEscalation"
                : intent.Entities.TryGetValue("ticketType", out var type) ? type : "QualityIssue";

            var subject = intent.Intent == "RequestHumanAgent"
                ? "Customer requested human agent"
                : "Customer complaint from chat/voice";

            var ticket = new Ticket
            {
                Id = Guid.NewGuid().ToString(),
                TicketId = Guid.NewGuid().ToString(),
                BusinessId = interaction.BusinessId,
                CustomerId = interaction.CustomerId,
                InteractionId = interaction.InteractionId,
                RelatedOrderId = interaction.RelatedOrderId,
                TicketType = ticketType,
                Subject = subject,
                Status = "Open",
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Tickets.AddAsync(ticket);
            await _unitOfWork.CompleteAsync();

            return ticket;
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
            
            if (intent == "RequestHumanAgent")
            {
                return $"{greeting}، حولت المحادثة لموظف خدمة عملاء. رقم التذكرة: {ticket.TicketId}. هيتم التواصل معاك في أقرب وقت.";
            }

            return $"{greeting}، تم فتح تذكرة شكوى برقم: {ticket.TicketId}. هنراجع المشكلة ونتواصل معاك لحلّها في أقرب وقت ممكن.";
        }
    }
}

