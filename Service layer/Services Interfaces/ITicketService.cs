using Domain_layer.Models;
using Service_layer.DTOS.Ticket;

namespace Service_layer.Services_Interfaces
{
    public interface ITicketService
    {
        Task<IEnumerable<Ticket>> GetAllAsync();
        Task<IEnumerable<Ticket>> GetByBusinessIdAsync(string businessId);
        Task<Ticket?> GetByIdAsync(string id);
        Task<Ticket> CreateAsync(TicketCreateDTO dto);
        Task<Ticket?> UpdateAsync(string id, TicketUpdateDTO dto);
        Task<Ticket?> AssignTicketAsync(string id, AssignTicketDTO dto);
        Task<Ticket?> CloseTicketAsync(string id, CloseTicketDTO dto);
        Task<bool> DeleteAsync(string id);
    }
}

