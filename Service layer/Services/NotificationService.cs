using System;
using Domain_layer.Interfaces;
using Domain_layer.Models;
using Service_layer.DTOS.Notification;
using Service_layer.Services_Interfaces;

namespace Service_layer.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IRepository<Notification> _notificationRepository;
        private readonly IRepository<Business> _businessRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public NotificationService(
            IRepository<Notification> notificationRepository,
            IRepository<Business> businessRepository,
            IRepository<User> userRepository,
            IUnitOfWork unitOfWork)
        {
            _notificationRepository = notificationRepository;
            _businessRepository = businessRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Notification>> GetAllAsync()
        {
            return await _notificationRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Notification>> GetByBusinessIdAsync(string businessId)
        {
            var allNotifications = await _notificationRepository.GetAllAsync();
            return allNotifications.Where(n => n.BusinessId == businessId);
        }

        public async Task<IEnumerable<Notification>> GetByUserIdAsync(string userId)
        {
            var allNotifications = await _notificationRepository.GetAllAsync();
            return allNotifications.Where(n => n.UserId == userId);
        }

        public async Task<Notification?> GetByIdAsync(string id)
        {
            return await _notificationRepository.GetByIdAsync(id);
        }

        public async Task<Notification> CreateAsync(NotificationCreateDTO dto)
        {
            var business = await _businessRepository.GetByIdAsync(dto.BusinessId);
            if (business == null)
                throw new ArgumentException($"Business with id '{dto.BusinessId}' not found.");

            if (!string.IsNullOrEmpty(dto.UserId))
            {
                var user = await _userRepository.GetByIdAsync(dto.UserId);
                if (user == null)
                    throw new ArgumentException($"User with id '{dto.UserId}' not found.");
            }

            var notification = new Notification
            {
                NotificationId = Guid.NewGuid().ToString(),
                Title = dto.Title,
                Message = dto.Message,
                IsRead = false,
                BusinessId = dto.BusinessId,
                UserId = string.IsNullOrEmpty(dto.UserId) ? null : dto.UserId,
                CreatedAt = DateTime.UtcNow
            };

            await _notificationRepository.AddAsync(notification);
            await _unitOfWork.CompleteAsync();
            return notification;
        }

        public async Task<Notification?> MarkAsReadAsync(string id)
        {
            var notification = await _notificationRepository.GetByIdAsync(id);
            if (notification == null) return null;

            notification.IsRead = true;

            _notificationRepository.Update(notification);
            await _unitOfWork.CompleteAsync();
            return notification;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var notification = await _notificationRepository.GetByIdAsync(id);
            if (notification == null) return false;

            _notificationRepository.Delete(notification);
            await _unitOfWork.CompleteAsync();
            return true;
        }
    }
}

