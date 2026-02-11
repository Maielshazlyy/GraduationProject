using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Context;
using DAL.Repositories;
using Domain_layer.Interfaces;

namespace DAL.UnitOfWork
{
  public class UnitOfWork:IUnitOfWork
    {
        private readonly AppDbContext _context;
        
        public IBusinessRepository Businesses { get; }
        public ICustomerRepository Customers { get; }
        public IOrderRepository Orders { get; }
        public IOrderItemRepository OrderItems { get; }
        public ITicketRepository Tickets { get; }
        public IMenuItemRepository MenuItems { get; }
        public IMenuCategoryRepository MenuCategories { get; }
        public IWorkingHoursRepository WorkingHours { get; }
        public IFeedbackRepository Feedbacks { get; }
        public IMessageRepository Messages { get; }
        public INotificationRepository Notifications { get; }
        public IReportRepository Reports { get; }
        public IKnowledgeBaseRepository KnowledgeBases { get; }
        public IInteractionRepository Interactions { get; }
        public ISubscriptionRepository Subscriptions { get; }
        public IPaymentTransactionRepository PaymentTransactions { get; }
        public ISettingRepository Settings { get; }
        public IIntegrationRepository Integrations { get; }
        public IAuditLogRepository AuditLogs { get; }
        public ISentimentRepository Sentiments { get; }
        
        public UnitOfWork(
            AppDbContext context,
            IBusinessRepository businessRepository,
            ICustomerRepository customerRepository,
            IOrderRepository orderRepository,
            IOrderItemRepository orderItemRepository,
            ITicketRepository ticketRepository,
            IMenuItemRepository menuItemRepository,
            IMenuCategoryRepository menuCategoryRepository,
            IWorkingHoursRepository workingHoursRepository,
            IFeedbackRepository feedbackRepository,
            IMessageRepository messageRepository,
            INotificationRepository notificationRepository,
            IReportRepository reportRepository,
            IKnowledgeBaseRepository knowledgeBaseRepository,
            IInteractionRepository interactionRepository,
            ISubscriptionRepository subscriptionRepository,
            IPaymentTransactionRepository paymentTransactionRepository,
            ISettingRepository settingRepository,
            IIntegrationRepository integrationRepository,
            IAuditLogRepository auditLogRepository,
            ISentimentRepository sentimentRepository)
        {
            _context = context;
            Businesses = businessRepository;
            Customers = customerRepository;
            Orders = orderRepository;
            OrderItems = orderItemRepository;
            Tickets = ticketRepository;
            MenuItems = menuItemRepository;
            MenuCategories = menuCategoryRepository;
            WorkingHours = workingHoursRepository;
            Feedbacks = feedbackRepository;
            Messages = messageRepository;
            Notifications = notificationRepository;
            Reports = reportRepository;
            KnowledgeBases = knowledgeBaseRepository;
            Interactions = interactionRepository;
            Subscriptions = subscriptionRepository;
            PaymentTransactions = paymentTransactionRepository;
            Settings = settingRepository;
            Integrations = integrationRepository;
            AuditLogs = auditLogRepository;
            Sentiments = sentimentRepository;
        }
        
        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
