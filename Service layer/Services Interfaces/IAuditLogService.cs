using Domain_layer.Models;

namespace Service_layer.Services_Interfaces
{
    public interface IAuditLogService
    {
        Task<IEnumerable<AuditLog>> GetAllAsync();
        Task<IEnumerable<AuditLog>> GetByBusinessIdAsync(string businessId);
        Task<IEnumerable<AuditLog>> GetByUserIdAsync(string userId);
        Task<AuditLog?> GetByIdAsync(string id);
        
        /// <summary>
        /// Create a new audit log entry.
        /// </summary>
        Task<AuditLog> CreateAsync(string businessId, string action, string entity, string entityId, string? userId = null);
        
        /// <summary>
        /// Log a user action (e.g., CreateUser, AssignRole, DeleteUser).
        /// </summary>
        Task LogUserActionAsync(string businessId, string action, string userId, string? targetUserId = null);
        
        /// <summary>
        /// Log a ticket action (e.g., AssignTicket, CloseTicket, EscalateTicket).
        /// </summary>
        Task LogTicketActionAsync(string businessId, string action, string ticketId, string? userId = null);
        
        /// <summary>
        /// Log an interaction/escalation event.
        /// </summary>
        Task LogInteractionActionAsync(string businessId, string action, string interactionId, string? userId = null);
        
        /// <summary>
        /// Log an order action (e.g., CreateOrder, UpdateOrderStatus, DeleteOrder).
        /// </summary>
        Task LogOrderActionAsync(string businessId, string action, string orderId, string? userId = null);
        
        /// <summary>
        /// Log a business action (e.g., CreateBusiness, UpdateBusiness, DeleteBusiness).
        /// </summary>
        Task LogBusinessActionAsync(string businessId, string action, string? userId = null);
        
        /// <summary>
        /// Log a menu item action (e.g., CreateMenuItem, UpdateMenuItem, DeleteMenuItem).
        /// </summary>
        Task LogMenuItemActionAsync(string businessId, string action, string menuItemId, string? userId = null);
        
        /// <summary>
        /// Log a menu category action (e.g., CreateMenuCategory, UpdateMenuCategory, DeleteMenuCategory).
        /// </summary>
        Task LogMenuCategoryActionAsync(string businessId, string action, string menuCategoryId, string? userId = null);
        
        /// <summary>
        /// Log a payment transaction action (e.g., CreatePaymentTransaction).
        /// </summary>
        Task LogPaymentTransactionActionAsync(string businessId, string action, string paymentTransactionId, string? userId = null);
        
        /// <summary>
        /// Log a knowledge base action (e.g., CreateKnowledgeBase, UpdateKnowledgeBase, DeleteKnowledgeBase).
        /// </summary>
        Task LogKnowledgeBaseActionAsync(string businessId, string action, string knowledgeBaseId, string? userId = null);
        
        /// <summary>
        /// Log a settings update action.
        /// </summary>
        Task LogSettingsActionAsync(string businessId, string action, string? userId = null);
    }
}

