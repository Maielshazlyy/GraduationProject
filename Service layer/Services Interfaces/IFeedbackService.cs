using Domain_layer.Models;
using Service_layer.DTOS.Feedback;

namespace Service_layer.Services_Interfaces
{
    public interface IFeedbackService
    {
        Task<IEnumerable<Feedback>> GetAllAsync();
        Task<IEnumerable<Feedback>> GetByCustomerIdAsync(string customerId);
        Task<Feedback?> GetByIdAsync(string id);
        Task<Feedback> CreateAsync(FeedbackCreateDTO dto);
        Task<Feedback?> UpdateAsync(string id, FeedbackUpdateDTO dto);
        Task<bool> DeleteAsync(string id);
    }
}

