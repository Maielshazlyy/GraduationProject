using Domain_layer.Models;
using Service_layer.DTOS.Ticket;

namespace Service_layer.Services_Interfaces
{
    public interface ITicketService
    {
        Task<IEnumerable<Ticket>> GetAllAsync();
        Task<IEnumerable<Ticket>> GetByBusinessIdAsync(string businessId);
        Task<Ticket?> GetByIdAsync(string id);

        /// <summary>
        /// Get all escalated tickets (HumanEscalation) that are not yet assigned
        /// to any agent, for a specific business. This powers the agent queue
        /// on the dashboard.
        /// </summary>
        Task<IEnumerable<Ticket>> GetOpenEscalationQueueAsync(string businessId);
        Task<Ticket> CreateAsync(TicketCreateDTO dto);
        Task<Ticket?> UpdateAsync(string id, TicketUpdateDTO dto);
        Task<Ticket?> AssignTicketAsync(string id, AssignTicketDTO dto);
        Task<Ticket?> CloseTicketAsync(string id, CloseTicketDTO dto);
        Task<bool> DeleteAsync(string id);
    }
}

