using System;
using Domain_layer.Interfaces;
using Domain_layer.Models;
using Service_layer.Services_Interfaces;

namespace Service_layer.Services
{
    public class AuditLogService : IAuditLogService
    {
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AuditLogService(IAuditLogRepository auditLogRepository, IUnitOfWork unitOfWork)
        {
            _auditLogRepository = auditLogRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<AuditLog>> GetAllAsync()
        {
            return await _auditLogRepository.GetAllAsync();
        }

        public async Task<IEnumerable<AuditLog>> GetByBusinessIdAsync(string businessId)
        {
            return await _auditLogRepository.GetByBusinessIdAsync(businessId);
        }

        public async Task<IEnumerable<AuditLog>> GetByUserIdAsync(string userId)
        {
            return await _auditLogRepository.GetByUserIdAsync(userId);
        }

        public async Task<AuditLog?> GetByIdAsync(string id)
        {
            return await _auditLogRepository.GetByIdAsync(id);
        }

        public async Task<AuditLog> CreateAsync(string businessId, string action, string entity, string entityId, string? userId = null)
        {
            var auditLog = new AuditLog
            {
                AuditLogId = Guid.NewGuid().ToString(),
                BusinessId = businessId,
                Action = action,
                Entity = entity,
                EntityId = entityId,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            await _auditLogRepository.AddAsync(auditLog);
            await _unitOfWork.CompleteAsync();
            return auditLog;
        }

        public async Task LogUserActionAsync(string businessId, string action, string userId, string? targetUserId = null)
        {
            await CreateAsync(
                businessId: businessId,
                action: action,
                entity: "User",
                entityId: targetUserId ?? userId,
                userId: userId
            );
        }

        public async Task LogTicketActionAsync(string businessId, string action, string ticketId, string? userId = null)
        {
            await CreateAsync(
                businessId: businessId,
                action: action,
                entity: "Ticket",
                entityId: ticketId,
                userId: userId
            );
        }

        public async Task LogInteractionActionAsync(string businessId, string action, string interactionId, string? userId = null)
        {
            await CreateAsync(
                businessId: businessId,
                action: action,
                entity: "Interaction",
                entityId: interactionId,
                userId: userId
            );
        }

        public async Task LogOrderActionAsync(string businessId, string action, string orderId, string? userId = null)
        {
            await CreateAsync(
                businessId: businessId,
                action: action,
                entity: "Order",
                entityId: orderId,
                userId: userId
            );
        }

        public async Task LogBusinessActionAsync(string businessId, string action, string? userId = null)
        {
            await CreateAsync(
                businessId: businessId,
                action: action,
                entity: "Business",
                entityId: businessId,
                userId: userId
            );
        }

        public async Task LogMenuItemActionAsync(string businessId, string action, string menuItemId, string? userId = null)
        {
            await CreateAsync(
                businessId: businessId,
                action: action,
                entity: "MenuItem",
                entityId: menuItemId,
                userId: userId
            );
        }

        public async Task LogMenuCategoryActionAsync(string businessId, string action, string menuCategoryId, string? userId = null)
        {
            await CreateAsync(
                businessId: businessId,
                action: action,
                entity: "MenuCategory",
                entityId: menuCategoryId,
                userId: userId
            );
        }

        public async Task LogPaymentTransactionActionAsync(string businessId, string action, string paymentTransactionId, string? userId = null)
        {
            await CreateAsync(
                businessId: businessId,
                action: action,
                entity: "PaymentTransaction",
                entityId: paymentTransactionId,
                userId: userId
            );
        }

        public async Task LogKnowledgeBaseActionAsync(string businessId, string action, string knowledgeBaseId, string? userId = null)
        {
            await CreateAsync(
                businessId: businessId,
                action: action,
                entity: "KnowledgeBase",
                entityId: knowledgeBaseId,
                userId: userId
            );
        }

        public async Task LogSettingsActionAsync(string businessId, string action, string? userId = null)
        {
            await CreateAsync(
                businessId: businessId,
                action: action,
                entity: "Settings",
                entityId: businessId,
                userId: userId
            );
        }
    }
}

