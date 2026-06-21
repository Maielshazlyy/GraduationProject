using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Context;
using Domain_layer.enums;
using Domain_layer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace digital_employee
{
    public static class DataSeeder
    {
        // ── Business seed definition ──────────────────────────────────────────
        private record BusinessSeed(
            string Name,
            string Type,
            string CuisineType,
            string Description,
            string OwnerEmail,
            string OwnerName,
            string Phone,
            string City,
            List<CategorySeed> Categories,
            List<FaqSeed> Faqs,
            List<KbSeed> KbEntries
        );

        private record CategorySeed(string Name, string Description, int Order, List<ItemSeed> Items);
        private record ItemSeed(string Name, string Description, decimal Price, bool IsAvailable = true);
        private record FaqSeed(string Question, string Answer, int Order);
        private record KbSeed(string Question, string Answer);

        // ── Entry point ───────────────────────────────────────────────────────
        public static async Task SeedAllAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var context     = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // Ensure Owner role exists
            if (!await roleManager.RoleExistsAsync("Owner"))
                await roleManager.CreateAsync(new IdentityRole("Owner"));

            foreach (var seed in GetBusinessSeeds())
                await SeedBusinessAsync(context, userManager, seed);
        }

        // ── Seed one business ─────────────────────────────────────────────────
        private static async Task SeedBusinessAsync(
            AppDbContext context,
            UserManager<User> userManager,
            BusinessSeed seed)
        {
            // Skip if owner already exists
            if (await userManager.FindByEmailAsync(seed.OwnerEmail) != null) return;

            // Create business
            var business = new Business
            {
                Id           = Guid.NewGuid().ToString(),
                BusinessId   = Guid.NewGuid().ToString(),
                Name         = seed.Name,
                Type         = seed.Type,
                CuisineType  = seed.CuisineType,
                Description  = seed.Description,
                Phone        = seed.Phone,
                City         = seed.City,
                Country      = "Egypt",
                HasDelivery  = true,
                HasTakeout   = true,
                PaymentMethods = "Cash,Card",
                IsActive     = true,
                Address      = seed.City + ", Egypt"
            };
            context.Businesses.Add(business);

            // Create owner user
            var user = new User
            {
                UserName   = seed.OwnerEmail,
                Email      = seed.OwnerEmail,
                FullName   = seed.OwnerName,
                Role       = "Owner",
                BusinessId = business.Id,
                CreatedAt  = DateTime.UtcNow,
                EmailConfirmed = true
            };
            await userManager.CreateAsync(user, "Test123!");
            await userManager.AddToRoleAsync(user, "Owner");

            // Create categories + items
            foreach (var cat in seed.Categories)
            {
                var category = new MenuCategory
                {
                    MenuCategoryId = Guid.NewGuid().ToString(),
                    Name           = cat.Name,
                    Description    = cat.Description,
                    DisplayOrder   = cat.Order,
                    IsActive       = true,
                    BusinessId     = business.Id
                };
                context.MenuCategories.Add(category);

                foreach (var item in cat.Items)
                {
                    context.MenuItems.Add(new MenuItem
                    {
                        MenuItemId     = Guid.NewGuid().ToString(),
                        Name           = item.Name,
                        Description    = item.Description,
                        Price          = item.Price,
                        IsAvailable    = item.IsAvailable,
                        MenuCategoryId = category.MenuCategoryId,
                        BusinessId     = business.Id
                    });
                }
            }

            // Create FAQs
            foreach (var faq in seed.Faqs)
            {
                context.KnowledgeBases.Add(new KnowledgeBase
                {
                    KnowledgeBaseId = Guid.NewGuid().ToString(),
                    Question        = faq.Question,
                    Answer          = faq.Answer,
                    IsFAQ           = true,
                    DisplayOrder    = faq.Order,
                    IsActive        = true,
                    BusinessId      = business.Id
                });
            }

            // Create KB entries
            foreach (var kb in seed.KbEntries)
            {
                context.KnowledgeBases.Add(new KnowledgeBase
                {
                    KnowledgeBaseId = Guid.NewGuid().ToString(),
                    Question        = kb.Question,
                    Answer          = kb.Answer,
                    IsFAQ           = false,
                    DisplayOrder    = 0,
                    IsActive        = true,
                    BusinessId      = business.Id
                });
            }

            await context.SaveChangesAsync();

            // ── Setting ───────────────────────────────────────────────────────
            context.Settings.Add(new Setting
            {
                SettingId               = Guid.NewGuid().ToString(),
                BusinessId              = business.Id,
                AutoAssignTickets       = true,
                EnableNotifications     = true,
                Language                = "ar",
                TimeZone                = "Africa/Cairo",
                ChatbotEnabled          = true,
                ChatbotWelcomeMessage   = $"أهلاً بك في {seed.Name}! كيف يمكنني مساعدتك؟ | Welcome to {seed.Name}! How can I help you?",
                ChatbotPersonality      = "Friendly",
                AgentVoice              = "ar-EG-SalmaNeural",
                AgentVoiceProvider      = "azure",
                AgentVoiceSpeed         = 1.0,
                AgentVoicePitch         = 1.0,
                AgentVoiceLanguage      = "ar-EG",
                EmailNotifications      = true,
                SmsNotifications        = false,
                PushNotifications       = true
            });

            // ── WorkingHours (Sat–Thu open, Fri closed) ───────────────────────
            var workingDays = new[] { 0, 1, 2, 3, 4, 6 }; // Sun-Thu, Sat
            for (int day = 0; day <= 6; day++)
            {
                context.WorkingHours.Add(new WorkingHours
                {
                    WorkingHoursId = Guid.NewGuid().ToString(),
                    BusinessId     = business.Id,
                    DayOfWeek      = day,
                    IsClosed       = day == 5, // Friday closed
                    OpenTime       = day == 5 ? null : new TimeSpan(12, 0, 0),
                    CloseTime      = day == 5 ? null : new TimeSpan(23, 59, 0)
                });
            }

            // ── Subscription + PaymentTransaction ─────────────────────────────
            var subId  = Guid.NewGuid().ToString();
            var payId  = Guid.NewGuid().ToString();
            context.Subscriptions.Add(new Subscription
            {
                Id             = subId,
                SubscriptionId = Guid.NewGuid().ToString(),
                BusinessId     = business.Id,
                PlanName       = "Professional",
                Price          = 499m,
                StartDate      = DateTime.UtcNow.AddMonths(-2),
                EndDate        = DateTime.UtcNow.AddMonths(10),
                IsActive       = true
            });
            context.PaymentTransactions.Add(new PaymentTransaction
            {
                Id              = payId,
                PaymentId       = Guid.NewGuid().ToString(),
                SubscriptionId  = subId,
                Amount          = 499m,
                PaymentMethod   = PaymentMethod.Card,
                TransactionDate = DateTime.UtcNow.AddMonths(-2),
                Status          = PaymentStatus.Completed
            });

            await context.SaveChangesAsync();

            // ── Customers (10–15 per business) ───────────────────────────────
            var allCustomerNames = new[]
            {
                "Ahmed Hassan",   "Sara Mohamed",   "Omar Khaled",
                "Nour Ibrahim",   "Youssef Ali",    "Mariam Tarek",
                "Kareem Mahmoud", "Dina Samer",     "Mostafa Adel",
                "Rania Hossam",   "Amr Fathy",      "Hana Wael",
                "Tamer Gamal",    "Salma Nasser",   "Ziad Essam"
            };

            var rng           = new Random(business.Id.GetHashCode());
            int customerCount = rng.Next(10, 16); // 10–15 inclusive
            var emailTag      = seed.Name.Split('|')[0].Trim().Replace(" ", "").ToLower();
            var customers     = new List<Customer>();

            for (int i = 0; i < customerCount; i++)
            {
                var prefix = i % 3 == 0 ? "010" : i % 3 == 1 ? "011" : "012";
                var c = new Customer
                {
                    CustomerId = Guid.NewGuid().ToString(),
                    FullName   = allCustomerNames[i],
                    Email      = $"customer{i + 1}@{emailTag}.com",
                    Phone      = $"{prefix}{(10000000 + i * 1111111) % 100000000:D8}",
                    BusinessId = business.Id,
                    CreatedAt  = DateTime.UtcNow.AddDays(-(60 - i * 4))
                };
                context.Customers.Add(c);
                customers.Add(c);
            }
            await context.SaveChangesAsync();

            // ── Interactions + Messages + Orders + Tickets + Feedback ─────────
            var menuItems = context.MenuItems.Where(m => m.BusinessId == business.Id && m.IsAvailable).ToList();

            // ── Chat scenario templates (5 different intent types) ───────────
            var bizName = seed.Name.Split('|')[0].Trim();

            string[] addresses =
            {
                "شارع التحرير، مبنى 12، شقة 3 | El-Tahrir St, building 12, apt 3",
                "مدينة نصر، شارع عباس العقاد، برج النيل، دور 5 | Nasr City, Abbas El-Akkad St, Nile Tower, 5th floor",
                "المعادي، شارع 9، فيلا 7 | Maadi, Street 9, Villa 7",
                "الزمالك، شارع 26 يوليو، شقة 2 | Zamalek, 26 July St, apt 2",
                "مصر الجديدة، شارع الميرغني، عمارة الأهرام، دور 3 | Heliopolis, Mirghani St, Ahram building, 3rd floor",
                "الدقي، شارع البطل أحمد عبد العزيز، مبنى 5 | Dokki, Ahmed Abd El-Aziz St, building 5",
                "المهندسين، شارع سوريا، شقة 8 | Mohandiseen, Syria St, apt 8",
            };
            string[] paymentMethods =
            {
                "كاش عند الاستلام | Cash on delivery",
                "فيزا أونلاين | Visa online",
                "محفظة فودافون كاش | Vodafone Cash wallet",
                "كاش | Cash",
                "انستاباي | InstaPay",
            };

            // ── Ticket / escalation templates ─────────────────────────────────
            var ticketTemplates = new[]
            {
                new
                {
                    Subject         = "الأوردر اتأخر كتير | Order very late",
                    TicketType      = "LateDelivery",
                    Priority        = "High",
                    Resolution      = "تم التواصل مع المندوب وتم التوصيل. اعتذرنا وقدمنا كوبون خصم. | Rider contacted, order delivered. Apology and discount coupon issued.",
                    Opening         = "الأوردر بتاعي اتأخر أكتر من ساعة! | My order is over an hour late!",
                    AiAck           = "معلش جداً على التأخير. ممكن تقولي رقم الأوردر؟ | So sorry for the delay. Can you share your order number?",
                    CustomerReply   = "الرقم مش معايا بس طلبته من فترة | I don't have it but I ordered a while ago",
                    AiInvestigate   = "بشوف الأوردر... في تأخير غير متوقع بسبب ضغط الطلبات. بترفع الموضوع للدعم فوراً. | Checking now... unexpected delay due to high demand. Escalating right away.",
                    AiEscalate      = "تم فتح تذكرة دعم أولوية عالية. فريق الدعم هيتواصل معاك خلال 15 دقيقة. | High-priority ticket created. Support team will contact you within 15 minutes.",
                    CustomerClose   = "تمام، شكراً | Okay, thanks",
                    FeedbackRating  = 3,
                    FeedbackComment = "اتأخر التوصيل بس اتحل | Delivery was late but resolved",
                },
                new
                {
                    Subject         = "الأوردر وصل ناقص | Order arrived incomplete",
                    TicketType      = "MissingItem",
                    Priority        = "Normal",
                    Resolution      = "تم إرسال الأيتم الناقص مجاناً في توصيلة منفصلة. | Missing item was sent in a separate free delivery.",
                    Opening         = "الأوردر وصل بس فيه صنف ناقص! | My order arrived but something is missing!",
                    AiAck           = "معلش! إيه الصنف اللي مجاش؟ | Sorry! What item was missing?",
                    CustomerReply   = "الطبق الجانبي اللي طلبته مجاش مع الأوردر | The side dish I ordered didn't come",
                    AiInvestigate   = "آسف جداً على الإزعاج. براجع الأوردر دلوقتي... أيوه، في خطأ من جانبنا. | So sorry for the inconvenience. Reviewing your order now... yes, there was a mistake on our end.",
                    AiEscalate      = "هنبعتلك الصنف الناقص في أقرب وقت مجاناً. فتحت تذكرة علشان نتابع. | We'll send the missing item to you for free ASAP. Ticket created to follow up.",
                    CustomerClose   = "تمام جزاكم الله خير | Thank you so much",
                    FeedbackRating  = 4,
                    FeedbackComment = "حلوا المشكلة بسرعة وعوضوني | They solved it quickly and compensated me",
                },
                new
                {
                    Subject         = "الأكل وصل بارد | Food arrived cold",
                    TicketType      = "FoodQuality",
                    Priority        = "Normal",
                    Resolution      = "تم تقديم اعتذار وإرسال خصم 20% على الطلب القادم. | Apology given and 20% discount on next order.",
                    Opening         = "الأكل وصلني بارد خالص، مش مقبول! | The food arrived completely cold, unacceptable!",
                    AiAck           = "معلش جداً! الأكل لازم يوصل ساخن. ممكن تقولي إيه الأصناف اللي وصلت باردة؟ | I'm so sorry! Food should arrive hot. Which items were cold?",
                    CustomerReply   = "كل حاجة باردة، بدو إنه اتأخر كتير في الطريق | Everything was cold, seems it took too long on the way",
                    AiInvestigate   = "هذا مقبولش. بشوف المدة اللي أخدها التوصيل... فعلاً اتأخر أكتر من المعتاد. | That's not acceptable. Checking the delivery time... it was indeed longer than usual.",
                    AiEscalate      = "بفتح تذكرة لفريق الجودة وهتواصل معاك فريق الدعم. هنعوضك على الإزعاج. | Opening a quality ticket and support will contact you. We'll compensate for the inconvenience.",
                    CustomerClose   = "شكراً، استنى تواصلهم | Thanks, awaiting their call",
                    FeedbackRating  = 3,
                    FeedbackComment = "المشكلة اتحلت بعد التواصل | Issue resolved after follow-up",
                },
                new
                {
                    Subject         = "طلب إلغاء الأوردر | Order cancellation request",
                    TicketType      = "Cancellation",
                    Priority        = "Normal",
                    Resolution      = "تم إلغاء الأوردر واسترجاع المبلغ خلال 24 ساعة. | Order cancelled and refund processed within 24 hours.",
                    Opening         = "عايز ألغي الأوردر بتاعي، ممكن؟ | I want to cancel my order, is that possible?",
                    AiAck           = "أيوه ممكن نلغي لو لسه ما اتجهزش. ممكن تقولي رقم الأوردر؟ | Yes, we can cancel if it hasn't been prepared yet. Can I have your order number?",
                    CustomerReply   = "مش معايا الرقم بس طلبته من شوية بس | I don't have the number but I just ordered",
                    AiInvestigate   = "بشوف آخر أوردر عليك... لقيته، الأوردر لسه في مرحلة التجهيز. | Checking your latest order... found it. The order is still being prepared.",
                    AiEscalate      = "بفتح طلب إلغاء وبرفعه للمطبخ فوراً. هيتواصل معاك فريق الدعم للتأكيد وإرجاع الفلوس لو دفعت. | Cancellation request submitted to kitchen immediately. Support will contact you to confirm and process the refund.",
                    CustomerClose   = "شكراً جزيلاً | Thank you very much",
                    FeedbackRating  = 5,
                    FeedbackComment = "خدمة ممتازة وسريعة | Excellent and fast service",
                },
                new
                {
                    Subject         = "الأوردر وصل غلط | Wrong order delivered",
                    TicketType      = "WrongItem",
                    Priority        = "High",
                    Resolution      = "تم إرسال الأوردر الصحيح واسترجاع الغلط. | Correct order sent and wrong order retrieved.",
                    Opening         = "جالي أوردر غلط! ده مش اللي طلبته | I received the wrong order! This isn't what I ordered",
                    AiAck           = "معلش جداً على الإزعاج! إيه اللي وصلك وإيه اللي كنت طالبه؟ | So sorry! What did you receive and what did you order?",
                    CustomerReply   = "طلبت وجبة دجاج وجالي سمك! | I ordered chicken and received fish!",
                    AiInvestigate   = "ده خطأ كبير وأنا آسف عليه. بترفع الأمر فوراً لفريق التوصيل والمطبخ. | This is a serious mistake and I apologize. Escalating immediately to delivery and kitchen teams.",
                    AiEscalate      = "تم فتح تذكرة طارئة. هيبعتوا الأوردر الصح في أقرب وقت مجاناً ويستلموا الغلط. | Emergency ticket opened. Correct order will be sent ASAP for free and wrong order will be picked up.",
                    CustomerClose   = "تمام، استنى | Okay, I'll wait",
                    FeedbackRating  = 4,
                    FeedbackComment = "المشكلة اتحلت بسرعة ونفس اليوم | Resolved quickly the same day",
                },
            };

            for (int ci = 0; ci < customers.Count; ci++)
            {
                var customer  = customers[ci];
                int daysAgo   = 55 - ci * 4;
                var tmpl      = ticketTemplates[ci % ticketTemplates.Length];

                // ── Chat Interaction ──────────────────────────────────────────
                int chatScenario  = ci % 5;
                string chatType   = chatScenario == 0 || chatScenario == 2 ? "Order" : "Inquiry";
                int chatMins      = chatScenario == 0 ? 15 : chatScenario == 1 ? 8 : chatScenario == 2 ? 12 : chatScenario == 3 ? 6 : 10;

                var chatInteraction = new Interaction
                {
                    InteractionId   = Guid.NewGuid().ToString(),
                    BusinessId      = business.Id,
                    CustomerId      = customer.CustomerId,
                    Channel         = "WebChat",
                    InteractionType = chatType,
                    Status          = "Closed",
                    IsEnded         = true,
                    StartedAt       = DateTime.UtcNow.AddDays(-daysAgo),
                    EndedAt         = DateTime.UtcNow.AddDays(-daysAgo).AddMinutes(chatMins)
                };
                context.Interactions.Add(chatInteraction);
                await context.SaveChangesAsync();

                var item1     = menuItems.Count > 0 ? menuItems[ci % menuItems.Count] : null;
                var item2     = menuItems.Count > 1 ? menuItems[(ci + 1) % menuItems.Count] : null;
                var item1Name = item1?.Name ?? "وجبة";
                var item2Name = item2?.Name ?? "مشروب";
                var address   = addresses[ci % addresses.Length];
                var payment   = paymentMethods[ci % paymentMethods.Length];
                var t         = chatInteraction.StartedAt;

                List<Message> chatMsgs = chatScenario switch
                {
                    // ── Scenario 0: Place an order ────────────────────────────
                    0 => new()
                    {
                        Msg(chatInteraction.InteractionId, "Customer", "السلام عليكم، عايز أطلب | Hi, I'd like to order please", t),
                        Msg(chatInteraction.InteractionId, "AI", $"وعليكم السلام! أهلاً بيك في {bizName}. إيه اللي تحب تطلبه؟ | Welcome to {bizName}! What would you like?", t.AddSeconds(5)),
                        Msg(chatInteraction.InteractionId, "Customer", $"عايز {item1Name} و{item2Name} من فضلك | I'd like {item1Name} and {item2Name} please", t.AddSeconds(25)),
                        Msg(chatInteraction.InteractionId, "AI", $"تمام! {item1Name} و{item2Name}. عنوان التوصيل إيه؟ | Got it! What's your delivery address?", t.AddSeconds(32)),
                        Msg(chatInteraction.InteractionId, "Customer", address, t.AddMinutes(1)),
                        Msg(chatInteraction.InteractionId, "AI", "وطريقة الدفع؟ كاش ولا بطاقة؟ | Payment method? Cash or card?", t.AddMinutes(1).AddSeconds(10)),
                        Msg(chatInteraction.InteractionId, "Customer", payment, t.AddMinutes(2)),
                        Msg(chatInteraction.InteractionId, "AI", "تم تسجيل طلبك! هيوصلك خلال 30-45 دقيقة. شكراً! | Order confirmed! Arrives in 30-45 mins. Thank you!", t.AddMinutes(2).AddSeconds(15)),
                    },

                    // ── Scenario 1: Ask about menu & prices ───────────────────
                    1 => new()
                    {
                        Msg(chatInteraction.InteractionId, "Customer", $"أهلاً، عايز أعرف إيه الأصناف المتاحة وأسعارها | Hi, I'd like to know what items you have and their prices", t),
                        Msg(chatInteraction.InteractionId, "AI", $"أهلاً! في {bizName} عندنا قائمة متنوعة. تحب تعرف عن نوع معين؟ | Hi! At {bizName} we have a varied menu. Interested in a specific category?", t.AddSeconds(6)),
                        Msg(chatInteraction.InteractionId, "Customer", $"أيوه، إيه أرخص صنف وأغلى صنف عندكم؟ | Yes, what's your cheapest and most expensive item?", t.AddSeconds(30)),
                        Msg(chatInteraction.InteractionId, "AI", $"أرخص صنف هو {item2Name} وأغلى صنف هو {item1Name}. كلهم بمكونات طازجة! | Cheapest is {item2Name} and most expensive is {item1Name}. All with fresh ingredients!", t.AddSeconds(40)),
                        Msg(chatInteraction.InteractionId, "Customer", "وفي عروض أو خصومات دلوقتي؟ | Any current offers or discounts?", t.AddMinutes(1)),
                        Msg(chatInteraction.InteractionId, "AI", "أيوه! عندنا خصم 10% على أول طلب لأي عميل جديد. | Yes! We have 10% off your first order for new customers.", t.AddMinutes(1).AddSeconds(8)),
                        Msg(chatInteraction.InteractionId, "Customer", "ممتاز، هطلب قريب! | Great, I'll order soon!", t.AddMinutes(1).AddSeconds(45)),
                        Msg(chatInteraction.InteractionId, "AI", "نستنى طلبك! لو عندك أي سؤال تاني ابعتلنا. | We look forward to your order! Feel free to ask anything else.", t.AddMinutes(2)),
                    },

                    // ── Scenario 2: Order with extra customization ────────────
                    2 => new()
                    {
                        Msg(chatInteraction.InteractionId, "Customer", $"هاي! عايز أطلب بس عندي طلب خاص | Hey! I want to order but have a special request", t),
                        Msg(chatInteraction.InteractionId, "AI", $"أهلاً! اتفضل، قولي طلبك وهنحاول نوفره. | Hi! Go ahead, tell us your request and we'll do our best.", t.AddSeconds(7)),
                        Msg(chatInteraction.InteractionId, "Customer", $"عايز {item1Name} بس من غير بصل ومع صوص إضافي | I want {item1Name} but without onion and with extra sauce", t.AddSeconds(35)),
                        Msg(chatInteraction.InteractionId, "AI", $"تمام! {item1Name} بدون بصل وصوص زيادة. هتوصل إزاي؟ | Got it! {item1Name} without onion and extra sauce. How should it be delivered?", t.AddSeconds(45)),
                        Msg(chatInteraction.InteractionId, "Customer", address, t.AddMinutes(1)),
                        Msg(chatInteraction.InteractionId, "AI", "وطريقة الدفع؟ | Payment method?", t.AddMinutes(1).AddSeconds(12)),
                        Msg(chatInteraction.InteractionId, "Customer", payment, t.AddMinutes(2)),
                        Msg(chatInteraction.InteractionId, "AI", $"ممتاز! تم تسجيل طلبك مع ملاحظة التعديل. هيوصلك خلال 35 دقيقة! | Done! Order placed with your customization noted. Arrives in 35 mins!", t.AddMinutes(2).AddSeconds(10)),
                    },

                    // ── Scenario 3: Ask about delivery areas & fees ───────────
                    3 => new()
                    {
                        Msg(chatInteraction.InteractionId, "Customer", "بتوصلوا فين؟ وكام رسوم التوصيل؟ | Where do you deliver? And what are the delivery fees?", t),
                        Msg(chatInteraction.InteractionId, "AI", "بنوصل لمعظم مناطق القاهرة والجيزة. رسوم التوصيل بتبدأ من 15 جنيه حسب المنطقة. | We deliver to most areas in Cairo and Giza. Delivery fees start from 15 EGP depending on the area.", t.AddSeconds(8)),
                        Msg(chatInteraction.InteractionId, "Customer", "وبتوصلوا للمعادي؟ | Do you deliver to Maadi?", t.AddSeconds(40)),
                        Msg(chatInteraction.InteractionId, "AI", "أيوه، المعادي من مناطقنا. رسوم التوصيل 20 جنيه ووقت التوصيل 40-50 دقيقة. | Yes, Maadi is one of our areas. Delivery fee is 20 EGP and takes 40-50 minutes.", t.AddSeconds(50)),
                        Msg(chatInteraction.InteractionId, "Customer", "وفي حد أوردر minimum؟ | Is there a minimum order?", t.AddMinutes(1).AddSeconds(20)),
                        Msg(chatInteraction.InteractionId, "AI", "أقل أوردر 50 جنيه مش شامل التوصيل. | Minimum order is 50 EGP excluding delivery.", t.AddMinutes(1).AddSeconds(30)),
                        Msg(chatInteraction.InteractionId, "Customer", "تمام شكراً على المعلومات | Great, thanks for the info", t.AddMinutes(2)),
                        Msg(chatInteraction.InteractionId, "AI", "العفو! لما تحب تطلب احنا موجودين. | You're welcome! We're here whenever you're ready to order.", t.AddMinutes(2).AddSeconds(10)),
                    },

                    // ── Scenario 4: Ask about ingredients / allergens ─────────
                    _ => new()
                    {
                        Msg(chatInteraction.InteractionId, "Customer", $"مرحباً، عندي حساسية من المكسرات. {item1Name} فيها مكسرات؟ | Hi, I'm allergic to nuts. Does {item1Name} contain nuts?", t),
                        Msg(chatInteraction.InteractionId, "AI", $"أهلاً! سؤال مهم جداً. {item1Name} مش فيها مكسرات في الوصفة الأصلية، بس المطبخ بيستخدم مكسرات في أصناف تانية فممكن يحصل تلوث. | Hi! Great question. {item1Name} doesn't contain nuts in the original recipe, but our kitchen handles nuts in other dishes so cross-contamination is possible.", t.AddSeconds(9)),
                        Msg(chatInteraction.InteractionId, "Customer", "وفي أي أصناف عندكم آمنة خالص من المكسرات؟ | Which of your items are completely nut-free?", t.AddSeconds(50)),
                        Msg(chatInteraction.InteractionId, "AI", $"الأصناف دي عادةً آمنة: {item2Name}. بس دايماً ننصح بإخبار المطبخ بالحساسية عند الطلب. | These items are usually safe: {item2Name}. We always recommend informing the kitchen about allergies when ordering.", t.AddMinutes(1)),
                        Msg(chatInteraction.InteractionId, "Customer", "ينفع أنبه المطبخ في الأوردر؟ | Can I add an allergy note to my order?", t.AddMinutes(1).AddSeconds(30)),
                        Msg(chatInteraction.InteractionId, "AI", "أيوه بالطبع! في خانة الملاحظات لما بتطلب اكتب حساسيتك وهياخدوا احتياطهم. | Yes of course! There's a notes field when ordering — write your allergy and the kitchen will take precautions.", t.AddMinutes(1).AddSeconds(40)),
                        Msg(chatInteraction.InteractionId, "Customer", "شكراً جزيلاً، مطمن دلوقتي! | Thank you so much, I feel reassured now!", t.AddMinutes(2).AddSeconds(10)),
                        Msg(chatInteraction.InteractionId, "AI", "العفو وسلامتك! طلبك دايماً مهم لينا. | You're welcome, stay safe! Your wellbeing matters to us.", t.AddMinutes(2).AddSeconds(20)),
                    },
                };

                context.Messages.AddRange(chatMsgs);

                // Order only for order scenarios (0 and 2)
                if ((chatScenario == 0 || chatScenario == 2) && item1 != null && item2 != null)
                {
                    var orderId = Guid.NewGuid().ToString();
                    context.Orders.Add(new Order
                    {
                        OrderId    = orderId,
                        BusinessId = business.Id,
                        CustomerId = customer.CustomerId,
                        TotalPrice = item1.Price + item2.Price,
                        Status     = OrderStatus.Delivered,
                        CreatedAt  = chatInteraction.StartedAt.AddMinutes(2)
                    });
                    context.OrderItems.AddRange(
                        new OrderItem { OrderItemId = Guid.NewGuid().ToString(), OrderId = orderId, MenuItemId = item1.MenuItemId, Quantity = 1, UnitPrice = item1.Price },
                        new OrderItem { OrderItemId = Guid.NewGuid().ToString(), OrderId = orderId, MenuItemId = item2.MenuItemId, Quantity = 1, UnitPrice = item2.Price }
                    );
                }

                // ── Escalation Interaction + Ticket ───────────────────────────
                int escDaysAgo = Math.Max(1, daysAgo - 3);
                var escInteraction = new Interaction
                {
                    InteractionId   = Guid.NewGuid().ToString(),
                    BusinessId      = business.Id,
                    CustomerId      = customer.CustomerId,
                    Channel         = "WebChat",
                    InteractionType = "Ticket",
                    Status          = "Closed",
                    IsEnded         = true,
                    StartedAt       = DateTime.UtcNow.AddDays(-escDaysAgo),
                    EndedAt         = DateTime.UtcNow.AddDays(-escDaysAgo).AddMinutes(30)
                };
                context.Interactions.Add(escInteraction);
                await context.SaveChangesAsync();

                var ticketId = Guid.NewGuid().ToString();
                context.Tickets.Add(new Ticket
                {
                    Id              = ticketId,
                    BusinessId      = business.Id,
                    CustomerId      = customer.CustomerId,
                    InteractionId   = escInteraction.InteractionId,
                    Subject         = tmpl.Subject,
                    Status          = "Closed",
                    IsEnded         = true,
                    TicketType      = tmpl.TicketType,
                    PriorityLevel   = tmpl.Priority,
                    ResolutionNotes = tmpl.Resolution,
                    CreatedAt       = escInteraction.StartedAt,
                    ClosedAt        = escInteraction.EndedAt
                });

                context.Messages.AddRange(
                    new Message
                    {
                        MessageId     = Guid.NewGuid().ToString(),
                        InteractionId = escInteraction.InteractionId,
                        SenderType    = "Customer",
                        Content       = tmpl.Opening,
                        SentAt        = escInteraction.StartedAt
                    },
                    new Message
                    {
                        MessageId     = Guid.NewGuid().ToString(),
                        InteractionId = escInteraction.InteractionId,
                        SenderType    = "AI",
                        Content       = tmpl.AiAck,
                        SentAt        = escInteraction.StartedAt.AddSeconds(8)
                    },
                    new Message
                    {
                        MessageId     = Guid.NewGuid().ToString(),
                        InteractionId = escInteraction.InteractionId,
                        SenderType    = "Customer",
                        Content       = tmpl.CustomerReply,
                        SentAt        = escInteraction.StartedAt.AddSeconds(40)
                    },
                    new Message
                    {
                        MessageId     = Guid.NewGuid().ToString(),
                        InteractionId = escInteraction.InteractionId,
                        SenderType    = "AI",
                        Content       = tmpl.AiInvestigate,
                        SentAt        = escInteraction.StartedAt.AddMinutes(1)
                    },
                    new Message
                    {
                        MessageId     = Guid.NewGuid().ToString(),
                        InteractionId = escInteraction.InteractionId,
                        SenderType    = "AI",
                        Content       = tmpl.AiEscalate,
                        SentAt        = escInteraction.StartedAt.AddMinutes(1).AddSeconds(10)
                    },
                    new Message
                    {
                        MessageId     = Guid.NewGuid().ToString(),
                        InteractionId = escInteraction.InteractionId,
                        SenderType    = "Customer",
                        Content       = tmpl.CustomerClose,
                        SentAt        = escInteraction.StartedAt.AddMinutes(2)
                    }
                );

                context.Feedbacks.Add(new Feedback
                {
                    FeedbackId = Guid.NewGuid().ToString(),
                    CustomerId = customer.CustomerId,
                    TicketId   = ticketId,
                    Rating     = tmpl.FeedbackRating,
                    Comment    = tmpl.FeedbackComment,
                    CreatedAt  = escInteraction.EndedAt!.Value.AddHours(1)
                });
            }

            // ── Notifications (2 per business) ────────────────────────────────
            context.Notifications.AddRange(
                new Notification
                {
                    NotificationId = Guid.NewGuid().ToString(),
                    BusinessId     = business.Id,
                    UserId         = user.Id,
                    Title          = "طلب جديد | New Order",
                    Message        = "وصلك طلب جديد من أحمد حسن | New order received from Ahmed Hassan",
                    IsRead         = true,
                    CreatedAt      = DateTime.UtcNow.AddDays(-1)
                },
                new Notification
                {
                    NotificationId = Guid.NewGuid().ToString(),
                    BusinessId     = business.Id,
                    UserId         = user.Id,
                    Title          = "تذكرة جديدة | New Ticket",
                    Message        = "تم فتح تذكرة دعم جديدة | A new support ticket has been opened",
                    IsRead         = false,
                    CreatedAt      = DateTime.UtcNow.AddHours(-3)
                }
            );

            await context.SaveChangesAsync();
        }

        // ── Shared FAQs ───────────────────────────────────────────────────────
        private static List<FaqSeed> CommonFaqs(string deliveryTime = "30 لـ 45 دقيقة | 30 to 45 minutes") => new()
        {
            new("كام ساعة التوصيل؟ | How long does delivery take?",
                $"التوصيل بياخد {deliveryTime}. | Delivery takes {deliveryTime}.", 1),
            new("في حد أوردر minimum؟ | Is there a minimum order?",
                "أقل أوردر 50 جنيه. | Minimum order is 50 EGP.", 2),
            new("إيه طرق الدفع؟ | What payment methods do you accept?",
                "بنقبل كاش وبطاقات دفع إلكتروني. | We accept cash and electronic payment cards.", 3),
            new("إيه مواعيد العمل؟ | What are your working hours?",
                "شغالين يومياً من 12 ظهر لـ 12 منتصف الليل. | Open daily from 12 PM to 12 AM.", 4),
            new("ممكن أعدل الأوردر؟ | Can I modify my order?",
                "تقدر تعدل خلال 5 دقائق من التأكيد. | You can modify within 5 minutes of confirmation.", 5),
        };

        private static Message Msg(string interactionId, string senderType, string content, DateTime sentAt) =>
            new() { MessageId = Guid.NewGuid().ToString(), InteractionId = interactionId, SenderType = senderType, Content = content, SentAt = sentAt };

        private static List<KbSeed> CommonKb() => new()
        {
            new("هل الأكل حلال؟ | Is the food halal?",
                "أيوه، كل اللحوم حلال 100%. | Yes, all meats are 100% halal."),
            new("إيه سياسة الاسترجاع؟ | What is your return policy?",
                "لو الأوردر وصل غلط أو ناقص، كلمنا فوراً وهنعوضك. | If your order arrived wrong, contact us immediately."),
        };

        // ── 10 Business Definitions ───────────────────────────────────────────
        private static List<BusinessSeed> GetBusinessSeeds() => new()
        {
            // 1 ── Burger Palace
            new("Burger Palace | بيرجر بالاس", "Restaurant", "Fast Food",
                "أفضل برجر في القاهرة | Best burgers in Cairo",
                "owner@burgerpalace.com", "Mohamed Burger", "01001000001", "Cairo",
                Categories: new()
                {
                    new("Burgers | برجر", "كل أنواع البرجر | All kinds of burgers", 1, new()
                    {
                        new("Classic Smash Burger | برجر سماش كلاسيك", "لحم بجبن شيدر وصوص خاص | Beef with cheddar and special sauce", 120),
                        new("Double Smash Burger | برجر دبل سماش", "قرصتين لحم مع بيكون | Two patties with crispy bacon", 155),
                        new("Crispy Chicken Burger | برجر دجاج مقرمش", "فيليه دجاج مقلي مع كول سلو | Crispy chicken fillet with coleslaw", 110),
                        new("BBQ Burger | برجر BBQ", "برجر لحم مع صوص BBQ وبصل كراميل | Beef burger with BBQ sauce and caramelized onions", 135),
                        new("Mushroom Swiss Burger | برجر مشروم سويس", "برجر لحم مع مشروم وجبن سويس | Beef with mushrooms and Swiss cheese", 140, false),
                    }),
                    new("Sides | مقبلات", "سايدز ومقبلات | Sides and appetizers", 2, new()
                    {
                        new("Crispy Fries | فرايز مقرمشة", "بطاطس مقلية مقرمشة | Crispy fried potatoes", 35),
                        new("Onion Rings | حلقات بصل", "حلقات بصل مقلية | Fried onion rings", 40),
                        new("Coleslaw | كول سلو", "سلطة كول سلو طازجة | Fresh coleslaw salad", 25),
                    }),
                    new("Drinks | مشروبات", "مشروبات باردة | Cold beverages", 3, new()
                    {
                        new("Pepsi | بيبسي", "علبة بيبسي مبردة | Cold Pepsi can", 20),
                        new("Lemon Mint | ليمون بالنعناع", "ليمون طازج بنعناع | Fresh lemon with mint", 40),
                        new("Water | مياه", "مياه معدنية | Mineral water", 10),
                    }),
                },
                Faqs: CommonFaqs(),
                KbEntries: CommonKb()),

            // 2 ── Italian Corner
            new("Italian Corner | إيطاليان كورنر", "Restaurant", "Italian",
                "بيتزا وباستا إيطالية أصيلة | Authentic Italian pizza and pasta",
                "owner@italiancorner.com", "Sara Italian", "01001000002", "Cairo",
                Categories: new()
                {
                    new("Pizza | بيتزا", "بيتزا إيطالية بالفرن الحجري | Italian stone-oven pizza", 1, new()
                    {
                        new("Margherita | مرغريتا", "طماطم وموتزاريلا وريحان طازج | Tomato, mozzarella, fresh basil", 120),
                        new("Pepperoni | بيبروني", "بيبروني مع موتزاريلا | Pepperoni with mozzarella", 140),
                        new("Four Cheese | أربع جبن", "موتزاريلا، شيدر، بارميزان، جورجونزولا | Mozzarella, cheddar, parmesan, gorgonzola", 155),
                        new("BBQ Chicken Pizza | بيتزا دجاج BBQ", "دجاج مشوي مع صوص BBQ | Grilled chicken with BBQ sauce", 145),
                        new("Veggie Pizza | بيتزا خضار", "خضروات مشكلة مع صوص طماطم | Mixed vegetables with tomato sauce", 130),
                    }),
                    new("Pasta | باستا", "باستا إيطالية | Italian pasta", 2, new()
                    {
                        new("Spaghetti Bolognese | سباجيتي بولونيز", "سباجيتي مع صوص لحم | Spaghetti with meat sauce", 110),
                        new("Fettuccine Alfredo | فيتوتشيني ألفريدو", "فيتوتشيني بصوص قشطة وبارميزان | Fettuccine with cream and parmesan", 120),
                        new("Penne Arrabiata | بيني أرابياتا", "بيني بصوص طماطم حار | Penne with spicy tomato sauce", 100),
                        new("Lasagna | لازانيا", "لازانيا لحم بالبشاميل | Meat lasagna with béchamel", 130),
                    }),
                    new("Drinks | مشروبات", "مشروبات إيطالية | Italian beverages", 3, new()
                    {
                        new("Fresh Orange Juice | عصير برتقال طازج", "برتقال طازج | Fresh orange juice", 45),
                        new("Iced Coffee | قهوة مثلجة", "قهوة مثلجة مع لبن | Iced coffee with milk", 55),
                        new("Pepsi | بيبسي", "علبة بيبسي مبردة | Cold Pepsi can", 20),
                    }),
                },
                Faqs: CommonFaqs(),
                KbEntries: CommonKb()),

            // 3 ── Sushi World
            new("Sushi World | سوشي وورلد", "Restaurant", "Japanese",
                "سوشي طازج يومياً | Fresh sushi made daily",
                "owner@sushiworld.com", "Ahmed Sushi", "01001000003", "Cairo",
                Categories: new()
                {
                    new("Rolls | رولز", "سوشي رولز متنوعة | Variety of sushi rolls", 1, new()
                    {
                        new("California Roll | كاليفورنيا رول", "جراد البحر وأفوكادو وخيار | Crab, avocado, cucumber", 110),
                        new("Spicy Tuna Roll | تونة حار رول", "تونة طازجة مع صوص حار | Fresh tuna with spicy sauce", 130),
                        new("Dragon Roll | دراجون رول", "جمبري مقلي مع أفوكادو | Fried shrimp with avocado", 145),
                        new("Rainbow Roll | رينبو رول", "8 قطع سمك متنوع | 8 pieces of assorted fish", 160),
                        new("Vegetable Roll | خضار رول", "خيار وأفوكادو وجزر | Cucumber, avocado, carrot", 90),
                    }),
                    new("Nigiri & Sashimi | نيجيري وساشيمي", "قطع سمك طازجة | Fresh fish cuts", 2, new()
                    {
                        new("Salmon Nigiri | سالمون نيجيري", "سالمون طازج على أرز | Fresh salmon on rice", 120),
                        new("Tuna Sashimi | تونة ساشيمي", "شرائح تونة طازجة | Fresh tuna slices", 140),
                        new("Mixed Sashimi | ساشيمي مشكل", "تشكيلة سمك طازج | Assorted fresh fish", 165),
                    }),
                    new("Drinks | مشروبات", "مشروبات يابانية | Japanese beverages", 3, new()
                    {
                        new("Green Tea | شاي أخضر", "شاي أخضر ياباني | Japanese green tea", 30),
                        new("Lychee Juice | عصير ليتشي", "عصير ليتشي طازج | Fresh lychee juice", 50),
                        new("Pepsi | بيبسي", "علبة بيبسي مبردة | Cold Pepsi can", 20),
                    }),
                },
                Faqs: CommonFaqs("45 لـ 60 دقيقة | 45 to 60 minutes"),
                KbEntries: new()
                {
                    new("هل السوشي طازج؟ | Is the sushi fresh?", "أيوه، السوشي بيتعمل يومياً من سمك طازج. | Yes, sushi is made daily from fresh fish."),
                    new("هل عندكم خيارات نباتية؟ | Do you have vegetarian options?", "أيوه، عندنا رولز خضار ونيجيري خضار. | Yes, we have vegetable rolls and nigiri."),
                }),

            // 4 ── Cairo Grill
            new("Cairo Grill | كايرو جريل", "Restaurant", "Egyptian",
                "أفضل مشويات مصرية | Best Egyptian grills",
                "owner@cairogrill.com", "Khaled Grill", "01001000004", "Cairo",
                Categories: new()
                {
                    new("Grills | مشويات", "مشاوي متنوعة | Assorted grills", 1, new()
                    {
                        new("Mixed Grill | مشاوي مشكلة", "كفتة وشيش طاووق وكباب | Kofta, shish tawook, and kabab", 250),
                        new("Kofta Skewers | كفتة", "أسياخ كفتة مشوية | Grilled kofta skewers", 120),
                        new("Shish Tawook | شيش طاووق", "صدر دجاج مشوي متبل | Marinated grilled chicken breast", 130),
                        new("Grilled Lamb Chops | ضلوع ضاني مشوية", "ضلوع ضاني مشوية مع خضار | Grilled lamb chops with vegetables", 280),
                        new("Grilled Fish | سمك مشوي", "سمك طازج مشوي مع ليمون | Fresh grilled fish with lemon", 200, false),
                    }),
                    new("Sandwiches | سندوتشات", "سندوتشات مشاوي | Grill sandwiches", 2, new()
                    {
                        new("Chicken Shawarma | شاورما دجاج", "دجاج مشوي مع صوص ثوم | Grilled chicken with garlic sauce", 65),
                        new("Kofta Sandwich | سندوتش كفتة", "كفتة مشوية مع طحينة | Grilled kofta with tahini", 55),
                        new("Hawawshi | حواوشي", "لحم مفروم بالبهارات في خبز | Spiced minced meat in bread", 70),
                    }),
                    new("Sides | أطباق جانبية", "أطباق جانبية | Side dishes", 3, new()
                    {
                        new("Grilled Vegetables | خضار مشوية", "خضار مشوية متنوعة | Assorted grilled vegetables", 50),
                        new("Rice | أرز", "أرز بيض | White rice", 25),
                        new("Salad | سلطة", "سلطة خضراء طازجة | Fresh green salad", 35),
                    }),
                    new("Drinks | مشروبات", "مشروبات طازجة | Fresh beverages", 4, new()
                    {
                        new("Lemon Mint | ليمون بالنعناع", "ليمون طازج بنعناع | Fresh lemon with mint", 40),
                        new("Tamarind | تمر هندي", "عصير تمر هندي طازج | Fresh tamarind juice", 35),
                        new("Pepsi | بيبسي", "علبة بيبسي مبردة | Cold Pepsi can", 20),
                    }),
                },
                Faqs: CommonFaqs(),
                KbEntries: CommonKb()),

            // 5 ── The Bakery Café
            new("The Bakery Café | ذا بيكري كافيه", "Cafe", "Bakery",
                "مخبوزات طازجة وقهوة مميزة | Fresh baked goods and specialty coffee",
                "owner@thebakeryCafe.com", "Nour Bakery", "01001000005", "Alexandria",
                Categories: new()
                {
                    new("Coffee | قهوة", "قهوة مميزة | Specialty coffee", 1, new()
                    {
                        new("Espresso | إسبريسو", "شوت إسبريسو مركز | Concentrated espresso shot", 35),
                        new("Cappuccino | كابتشينو", "إسبريسو مع حليب مبخر | Espresso with steamed milk", 55),
                        new("Latte | لاتيه", "إسبريسو مع حليب مبخر وفوم | Espresso with steamed milk and foam", 60),
                        new("Iced Latte | لاتيه مثلج", "لاتيه بارد مع ثلج | Cold latte over ice", 65),
                        new("Mocha | موكا", "إسبريسو مع شوكولاتة وحليب | Espresso with chocolate and milk", 65),
                    }),
                    new("Pastries | مخبوزات", "مخبوزات طازجة يومياً | Fresh daily pastries", 2, new()
                    {
                        new("Croissant | كروسان", "كروسان زبدة طازج | Fresh butter croissant", 45),
                        new("Cheese Croissant | كروسان جبن", "كروسان محشي جبن | Cheese-filled croissant", 55),
                        new("Chocolate Muffin | مافن شوكولاتة", "مافن شوكولاتة طازج | Fresh chocolate muffin", 40),
                        new("Cinnamon Roll | سينامون رول", "رول قرفة بصوص كريمة | Cinnamon roll with cream sauce", 50),
                        new("Banana Bread | خبز موز", "خبز موز محلي الصنع | Homemade banana bread", 45),
                    }),
                    new("Cakes | تورتات", "قطع تورتة | Cake slices", 3, new()
                    {
                        new("Cheesecake | تشيز كيك", "تشيز كيك نيويورك | New York cheesecake slice", 75),
                        new("Chocolate Cake | تورتة شوكولاتة", "تورتة شوكولاتة غنية | Rich chocolate cake slice", 70),
                        new("Red Velvet | ريد فيلفيت", "كيكة ريد فيلفيت بكريمة | Red velvet with cream cheese frosting", 75),
                    }),
                },
                Faqs: new()
                {
                    new("كام ساعة التوصيل؟ | How long does delivery take?", "التوصيل بياخد 20 لـ 35 دقيقة. | Delivery takes 20 to 35 minutes.", 1),
                    new("في حد أوردر minimum؟ | Is there a minimum order?", "أقل أوردر 60 جنيه. | Minimum order is 60 EGP.", 2),
                    new("إيه مواعيد العمل؟ | What are your working hours?", "شغالين من 8 الصبح لـ 11 بالليل. | Open from 8 AM to 11 PM.", 3),
                    new("هل المخبوزات طازجة كل يوم؟ | Are pastries fresh daily?", "أيوه، بنخبز كل يوم الصبح. | Yes, we bake fresh every morning.", 4),
                    new("إيه طرق الدفع؟ | What payment methods do you accept?", "كاش وبطاقات دفع إلكتروني. | Cash and electronic payment cards.", 5),
                },
                KbEntries: new()
                {
                    new("هل القهوة عندكم specialty؟ | Is your coffee specialty?", "أيوه، بنستخدم حبوب قهوة مميزة ومحمصة طازجة. | Yes, we use specialty freshly roasted coffee beans."),
                    new("هل عندكم خيارات vegan؟ | Do you have vegan options?", "عندنا كروسان وخبز موز خالي من منتجات الحيوانية. | We have vegan croissants and banana bread."),
                }),

            // 6 ── Seafood Bay
            new("Seafood Bay | سيفود باي", "Restaurant", "Seafood",
                "أطازج مأكولات بحرية | Freshest seafood in town",
                "owner@seafoodbay.com", "Omar Seafood", "01001000006", "Alexandria",
                Categories: new()
                {
                    new("Grilled Seafood | مشويات بحرية", "مأكولات بحرية مشوية طازجة | Fresh grilled seafood", 1, new()
                    {
                        new("Grilled Sea Bass | قاروص مشوي", "قاروص طازج مشوي مع ليمون وأعشاب | Fresh sea bass grilled with lemon and herbs", 220),
                        new("Grilled Shrimp | جمبري مشوي", "جمبري كبير مشوي بالثوم والزبدة | Large shrimp grilled with garlic butter", 185),
                        new("Grilled Calamari | كالاماري مشوي", "حبار مشوي مع صوص طرطور | Grilled squid with tartar sauce", 150),
                        new("Mixed Seafood Platter | مشاوي بحرية مشكلة", "سمك وجمبري وكالاماري | Fish, shrimp, and calamari", 320),
                    }),
                    new("Fried Seafood | مقليات بحرية", "مأكولات بحرية مقلية | Fried seafood", 2, new()
                    {
                        new("Fried Calamari | كالاماري مقلي", "حبار مقلي مقرمش | Crispy fried calamari", 130),
                        new("Fried Shrimp | جمبري مقلي", "جمبري مقلي مقرمش | Crispy fried shrimp", 160),
                        new("Fish & Chips | فيش وشيبس", "فيليه سمك مقلي مع بطاطس | Fried fish fillet with chips", 140),
                    }),
                    new("Sides & Salads | جانبية وسلطات", "مقبلات وسلطات | Appetizers and salads", 3, new()
                    {
                        new("Seafood Soup | شوربة مأكولات بحرية", "شوربة كريمة بحرية | Creamy seafood soup", 75),
                        new("Greek Salad | سلطة يونانية", "خيار وطماطم وزيتون وجبن فيتا | Cucumber, tomato, olives, feta", 65),
                        new("Garlic Bread | خبز ثوم", "خبز مشوي بزبدة الثوم | Toasted garlic butter bread", 40),
                    }),
                    new("Drinks | مشروبات", "مشروبات طازجة | Fresh beverages", 4, new()
                    {
                        new("Lemon Mint | ليمون بالنعناع", "ليمون طازج بنعناع | Fresh lemon with mint", 40),
                        new("Mango Juice | عصير مانجو", "مانجو طازج | Fresh mango juice", 50),
                        new("Pepsi | بيبسي", "علبة بيبسي مبردة | Cold Pepsi can", 20),
                    }),
                },
                Faqs: CommonFaqs("45 لـ 60 دقيقة | 45 to 60 minutes"),
                KbEntries: new()
                {
                    new("هل السمك طازج؟ | Is the fish fresh?", "أيوه، بنستلم سمك طازج يومياً من السوق. | Yes, we receive fresh fish daily from the market."),
                    new("هل عندكم حساسية من المأكولات البحرية؟ | Seafood allergy warning?", "جميع أطباقنا تحتوي على مأكولات بحرية. يرجى التنبيه لأي حساسية. | All our dishes contain seafood. Please inform us of any allergies."),
                }),

            // 7 ── Koshary El Tahrir
            new("Koshary El Tahrir | كشري التحرير", "Restaurant", "Egyptian Street Food",
                "كشري مصري أصيل | Authentic Egyptian koshary",
                "owner@kosharytahrir.com", "Hassan Koshary", "01001000007", "Cairo",
                Categories: new()
                {
                    new("Koshary | كشري", "كشري بأحجام مختلفة | Koshary in different sizes", 1, new()
                    {
                        new("Small Koshary | كشري صغير", "وجبة كشري صغيرة | Small koshary serving", 25),
                        new("Medium Koshary | كشري وسط", "وجبة كشري متوسطة | Medium koshary serving", 35),
                        new("Large Koshary | كشري كبير", "وجبة كشري كبيرة | Large koshary serving", 45),
                        new("Extra Large Koshary | كشري كبير جداً", "وجبة كشري للجوعانين | Extra large koshary serving", 60),
                    }),
                    new("Extras | إضافات", "إضافات على الكشري | Koshary extras", 2, new()
                    {
                        new("Extra Sauce | صوص زيادة", "صوص طماطم إضافي | Extra tomato sauce", 5),
                        new("Extra Dakka | دقة زيادة", "دقة حارة إضافية | Extra spicy dakka", 5),
                        new("Extra Onion | بصل زيادة", "بصل مقلي إضافي | Extra fried onion", 10),
                        new("Hard Boiled Egg | بيضة مسلوقة", "بيضة مسلوقة إضافية | Extra hard boiled egg", 10),
                    }),
                    new("Drinks | مشروبات", "مشروبات | Beverages", 3, new()
                    {
                        new("Pepsi | بيبسي", "علبة بيبسي | Pepsi can", 15),
                        new("Water | مياه", "مياه معدنية | Mineral water", 10),
                        new("Lemon Juice | ليمون", "عصير ليمون | Lemon juice", 20),
                    }),
                },
                Faqs: new()
                {
                    new("كام ساعة التوصيل؟ | How long is delivery?", "التوصيل بياخد 20 لـ 30 دقيقة. | Delivery takes 20 to 30 minutes.", 1),
                    new("في حد أوردر minimum؟ | Minimum order?", "أقل أوردر 30 جنيه. | Minimum order is 30 EGP.", 2),
                    new("إيه مواعيد العمل؟ | Working hours?", "شغالين من 12 الظهر لـ 2 الصبح. | Open from 12 PM to 2 AM.", 3),
                    new("هل الكشري طازج؟ | Is the koshary fresh?", "أيوه، الكشري بيتعمل كل يوم طازج. | Yes, koshary is made fresh daily.", 4),
                },
                KbEntries: new()
                {
                    new("إيه مكونات الكشري؟ | What are koshary ingredients?", "الكشري بيتعمل من أرز وعدس ومكرونة وصوص طماطم وبصل مقلي. | Koshary is made of rice, lentils, pasta, tomato sauce, and fried onions."),
                }),

            // 8 ── Fresh & Healthy
            new("Fresh & Healthy | فريش وهيلثي", "Restaurant", "Healthy Food",
                "أكل صحي وخفيف | Healthy and light food",
                "owner@freshandhealthy.com", "Laila Healthy", "01001000008", "Cairo",
                Categories: new()
                {
                    new("Salads | سلطات", "سلطات طازجة ومغذية | Fresh nutritious salads", 1, new()
                    {
                        new("Caesar Salad | سلطة سيزر", "خس وكروتون وجبن بارميزان وصوص سيزر | Lettuce, croutons, parmesan, Caesar dressing", 85),
                        new("Greek Salad | سلطة يونانية", "خيار وطماطم وزيتون وفيتا | Cucumber, tomato, olives, feta", 80),
                        new("Quinoa Bowl | كينوا بول", "كينوا مع خضار مشوية وأفوكادو | Quinoa with grilled vegetables and avocado", 110),
                        new("Grilled Chicken Salad | سلطة دجاج مشوي", "صدر دجاج مشوي مع سلطة خضار | Grilled chicken breast with vegetable salad", 100),
                    }),
                    new("Wraps | رابس", "رابس صحية | Healthy wraps", 2, new()
                    {
                        new("Chicken Wrap | راب دجاج", "دجاج مشوي مع خضار في توست صحي | Grilled chicken with veggies in whole wheat wrap", 90),
                        new("Falafel Wrap | راب فلافل", "فلافل مع طحينة وخضار | Falafel with tahini and vegetables", 70),
                        new("Tuna Wrap | راب تونة", "تونة مع خضار في خبز صحي | Tuna with vegetables in healthy bread", 85),
                    }),
                    new("Juices & Smoothies | عصائر وسموذي", "عصائر ومشروبات صحية | Healthy juices and smoothies", 3, new()
                    {
                        new("Green Smoothie | سموذي أخضر", "سبانخ وموز وتفاح وليمون | Spinach, banana, apple, lemon", 65),
                        new("Berry Smoothie | سموذي توت", "توت مشكل مع لبن | Mixed berries with yogurt", 70),
                        new("Fresh Orange Juice | عصير برتقال طازج", "برتقال طازج | Fresh orange juice", 50),
                        new("Detox Water | ماء ديتوكس", "ماء مع خيار وليمون ونعناع | Water with cucumber, lemon, mint", 30),
                    }),
                },
                Faqs: CommonFaqs("30 لـ 40 دقيقة | 30 to 40 minutes"),
                KbEntries: new()
                {
                    new("هل الأكل عندكم صحي فعلاً؟ | Is your food actually healthy?", "أيوه، بنستخدم مكونات طازجة بدون إضافات صناعية. | Yes, we use fresh ingredients with no artificial additives."),
                    new("هل عندكم خيارات vegan وvegetarian؟ | Vegan/vegetarian options?", "عندنا قائمة كاملة للنباتيين والـ vegan. | We have a full menu for vegetarians and vegans."),
                }),

            // 9 ── Pizza Corner
            new("Pizza Corner | بيتزا كورنر", "Restaurant", "Pizza",
                "بيتزا أمريكية وإيطالية | American and Italian pizza",
                "owner@pizzacorner.com", "Amr Pizza", "01001000009", "Cairo",
                Categories: new()
                {
                    new("Classic Pizzas | بيتزا كلاسيك", "بيتزا على الطريقة الكلاسيكية | Classic style pizza", 1, new()
                    {
                        new("Pepperoni Pizza | بيتزا بيبروني", "بيبروني وموتزاريلا | Pepperoni and mozzarella", 130),
                        new("BBQ Chicken | بيتزا دجاج BBQ", "دجاج وبصل وفلفل وصوص BBQ | Chicken, onion, peppers, BBQ sauce", 140),
                        new("Meat Lover | ميت لفر", "لحم وبيبروني وسجق | Beef, pepperoni, sausage", 155),
                        new("Veggie Supreme | فيجي سوبريم", "فطر وفلفل وبصل وزيتون | Mushroom, peppers, onion, olives", 120),
                        new("Hawaiian | هاواياني", "دجاج وأناناس | Chicken and pineapple", 130, false),
                    }),
                    new("Pasta | باستا", "باستا إيطالية | Italian pasta", 2, new()
                    {
                        new("Mac & Cheese | ماك وجبن", "مكرونة بصوص جبن كريمي | Pasta with creamy cheese sauce", 90),
                        new("Penne Arrabiata | بيني أرابياتا", "بيني بصوص طماطم حار | Penne with spicy tomato sauce", 95),
                        new("Spaghetti Bolognese | سباجيتي بولونيز", "سباجيتي مع صوص لحم | Spaghetti with meat sauce", 105),
                    }),
                    new("Sides | سايدز", "مقبلات وإضافات | Appetizers and sides", 3, new()
                    {
                        new("Garlic Bread | خبز ثوم", "خبز مشوي بزبدة الثوم | Garlic butter toast", 35),
                        new("Mozzarella Sticks | عيدان موتزاريلا", "موتزاريلا مقلية مقرمشة | Crispy fried mozzarella", 65),
                        new("Crispy Fries | فرايز مقرمشة", "بطاطس مقلية مقرمشة | Crispy fried fries", 35),
                    }),
                    new("Drinks | مشروبات", "مشروبات | Beverages", 4, new()
                    {
                        new("Pepsi | بيبسي", "علبة بيبسي | Pepsi can", 20),
                        new("7Up | سيفن أب", "علبة سيفن أب | 7Up can", 20),
                        new("Lemon Mint | ليمون بالنعناع", "ليمون طازج بنعناع | Fresh lemon with mint", 40),
                    }),
                },
                Faqs: CommonFaqs(),
                KbEntries: CommonKb()),

            // 10 ── Sweet Dreams
            new("Sweet Dreams | سويت دريمز", "Desserts", "Desserts",
                "أحلى الحلويات والآيس كريم | Best desserts and ice cream",
                "owner@sweetdreams.com", "Dina Sweets", "01001000010", "Cairo",
                Categories: new()
                {
                    new("Waffles | وافل", "وافل بإضافات متنوعة | Waffles with various toppings", 1, new()
                    {
                        new("Nutella Waffle | وافل نوتيلا", "وافل مع نوتيلا وموز | Waffle with Nutella and banana", 85),
                        new("Lotus Waffle | وافل لوتس", "وافل مع كريمة لوتس | Waffle with Lotus cream", 95),
                        new("Fruit Waffle | وافل فواكه", "وافل مع فواكه طازجة وعسل | Waffle with fresh fruits and honey", 80),
                        new("Ice Cream Waffle | وافل آيس كريم", "وافل مع كرتين آيس كريم | Waffle with two scoops of ice cream", 90),
                    }),
                    new("Pancakes | بان كيك", "بان كيك هش وطري | Fluffy soft pancakes", 2, new()
                    {
                        new("Classic Pancakes | بان كيك كلاسيك", "بان كيك مع شراب قيقب وزبدة | Pancakes with maple syrup and butter", 70),
                        new("Nutella Pancakes | بان كيك نوتيلا", "بان كيك مع نوتيلا وموز | Pancakes with Nutella and banana", 85),
                        new("Berry Pancakes | بان كيك توت", "بان كيك مع توت مشكل | Pancakes with mixed berries", 80),
                    }),
                    new("Ice Cream | آيس كريم", "آيس كريم بنكهات متنوعة | Ice cream in various flavors", 3, new()
                    {
                        new("Single Scoop | كورنيت صغير", "كورنيت آيس كريم بنكهة واحدة | Single flavor ice cream cone", 30),
                        new("Double Scoop | كورنيت كبير", "كورنيت آيس كريم بنكهتين | Double flavor ice cream cone", 45),
                        new("Ice Cream Sundae | سنداي", "آيس كريم مع شراب شوكولاتة ومكسرات | Ice cream with chocolate sauce and nuts", 75),
                        new("Banana Split | بانانا سبليت", "موز مع ثلاث كرات آيس كريم | Banana with three ice cream scoops", 90),
                    }),
                    new("Hot Drinks | مشروبات ساخنة", "مشروبات ساخنة | Hot beverages", 4, new()
                    {
                        new("Hot Chocolate | شوكولاتة ساخنة", "شوكولاتة ساخنة غنية مع مارشميلو | Rich hot chocolate with marshmallow", 55),
                        new("Nutella Hot Drink | مشروب نوتيلا", "مشروب نوتيلا ساخن مع حليب | Hot Nutella drink with milk", 60),
                        new("Cappuccino | كابتشينو", "كابتشينو مميز | Specialty cappuccino", 55),
                    }),
                },
                Faqs: new()
                {
                    new("كام ساعة التوصيل؟ | Delivery time?", "التوصيل بياخد 25 لـ 40 دقيقة. | Delivery takes 25 to 40 minutes.", 1),
                    new("في حد أوردر minimum؟ | Minimum order?", "أقل أوردر 60 جنيه. | Minimum order is 60 EGP.", 2),
                    new("إيه مواعيد العمل؟ | Working hours?", "شغالين من 12 الظهر لـ 1 الصبح. | Open from 12 PM to 1 AM.", 3),
                    new("هل الحلويات طازجة؟ | Are desserts fresh?", "أيوه، كل حاجة بتتعمل عند الطلب. | Yes, everything is made fresh to order.", 4),
                    new("إيه طرق الدفع؟ | Payment methods?", "كاش وبطاقات دفع إلكتروني. | Cash and electronic payment cards.", 5),
                },
                KbEntries: new()
                {
                    new("هل عندكم خيارات خالية من الجلوتين؟ | Gluten-free options?", "بعض الحلويات متاحة بدون جلوتين. سألنا عن التفاصيل. | Some desserts are available gluten-free. Ask us for details."),
                    new("هل ممكن طلب تورتة خاصة؟ | Custom cake orders?", "أيوه، بنقبل طلبات تورتات خاصة مع إشعار يوم مسبقاً. | Yes, we accept custom cake orders with one day notice."),
                }),
        };
    }
}
