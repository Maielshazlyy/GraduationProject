using Domain_layer.Models;

namespace Domain_layer.Interfaces
{
    public interface ITicketRepository : IRepository<Ticket>
    {
        Task<IEnumerable<Ticket>> GetByBusinessIdAsync(string businessId);
        Task<IEnumerable<Ticket>> GetByCustomerIdAsync(string customerId);
        Task<IEnumerable<Ticket>> GetByStatusAsync(string status);
        Task<IEnumerable<Ticket>> GetByAssignedUserIdAsync(string userId);
    }
}

