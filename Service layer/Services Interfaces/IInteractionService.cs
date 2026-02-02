using Domain_layer.Models;
using Service_layer.DTOS.Interaction;

namespace Service_layer.Services_Interfaces
{
    public interface IInteractionService
    {
        Task<IEnumerable<Interaction>> GetAllAsync();
        Task<IEnumerable<Interaction>> GetByBusinessIdAsync(string businessId);
        Task<IEnumerable<Interaction>> GetByCustomerIdAsync(string customerId);
        Task<IEnumerable<Interaction>> GetByUserIdAsync(string userId);
        Task<Interaction?> GetByIdAsync(string id);
        Task<Interaction> StartInteractionAsync(StartInteractionDTO dto);
        Task<Interaction?> EndInteractionAsync(string id, EndInteractionDTO dto);
        Task<bool> DeleteAsync(string id);
    }
}

