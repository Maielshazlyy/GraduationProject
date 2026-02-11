using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_layer.Interfaces
{
    public interface IUnitOfWork
    {
        IBusinessRepository Businesses { get; }
        ICustomerRepository Customers { get; }
        IOrderRepository Orders { get; }
        IOrderItemRepository OrderItems { get; }
        ITicketRepository Tickets { get; }
        IMenuItemRepository MenuItems { get; }
        IMenuCategoryRepository MenuCategories { get; }
        IWorkingHoursRepository WorkingHours { get; }
        IFeedbackRepository Feedbacks { get; }
        IMessageRepository Messages { get; }
        INotificationRepository Notifications { get; }
        IReportRepository Reports { get; }
        IKnowledgeBaseRepository KnowledgeBases { get; }
        IInteractionRepository Interactions { get; }
        ISubscriptionRepository Subscriptions { get; }
        IPaymentTransactionRepository PaymentTransactions { get; }
        ISettingRepository Settings { get; }
        IIntegrationRepository Integrations { get; }
        IAuditLogRepository AuditLogs { get; }
        ISentimentRepository Sentiments { get; }
        
        Task<int> CompleteAsync();
    }
}
