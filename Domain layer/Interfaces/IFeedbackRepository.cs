using Domain_layer.Models;

namespace Domain_layer.Interfaces
{
    public interface IFeedbackRepository : IRepository<Feedback>
    {
        Task<IEnumerable<Feedback>> GetByCustomerIdAsync(string customerId);
        Task<IEnumerable<Feedback>> GetByTicketIdAsync(string ticketId);
        Task<double> GetAverageRatingAsync(string businessId);
    }
}

