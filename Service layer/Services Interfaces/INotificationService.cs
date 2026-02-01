using Domain_layer.Models;
using Service_layer.DTOS.Notification;

namespace Service_layer.Services_Interfaces
{
    public interface INotificationService
    {
        Task<IEnumerable<Notification>> GetAllAsync();
        Task<IEnumerable<Notification>> GetByBusinessIdAsync(string businessId);
        Task<IEnumerable<Notification>> GetByUserIdAsync(string userId);
        Task<Notification?> GetByIdAsync(string id);
        Task<Notification> CreateAsync(NotificationCreateDTO dto);
        Task<Notification?> MarkAsReadAsync(string id);
        Task<bool> DeleteAsync(string id);
    }
}

