using System;
using Domain_layer.Interfaces;
using Domain_layer.Models;
using Service_layer.DTOS.Ticket;
using Service_layer.Services_Interfaces;

namespace Service_layer.Services
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IBusinessRepository _businessRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TicketService(
            ITicketRepository ticketRepository,
            IBusinessRepository businessRepository,
            ICustomerRepository customerRepository,
            IRepository<User> userRepository,
            IUnitOfWork unitOfWork)
        {
            _ticketRepository = ticketRepository;
            _businessRepository = businessRepository;
            _customerRepository = customerRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Ticket>> GetAllAsync()
        {
            return await _ticketRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Ticket>> GetByBusinessIdAsync(string businessId)
        {
            return await _ticketRepository.GetByBusinessIdAsync(businessId);
        }

        public async Task<IEnumerable<Ticket>> GetOpenEscalationQueueAsync(string businessId)
        {
            var all = await _ticketRepository.GetByBusinessIdAsync(businessId);

            // Queue = HumanEscalation tickets that are not ended and not assigned yet
            return all.Where(t =>
                !t.IsEnded &&
                string.Equals(t.TicketType, "HumanEscalation", StringComparison.OrdinalIgnoreCase) &&
                string.IsNullOrEmpty(t.AssignedToUserId));
        }

        public async Task<Ticket?> GetByIdAsync(string id)
        {
            return await _ticketRepository.GetByIdAsync(id);
        }

        public async Task<Ticket> CreateAsync(TicketCreateDTO dto)
        {
            var business = await _businessRepository.GetByIdAsync(dto.BusinessId);
            if (business == null)
                throw new ArgumentException($"Business with id '{dto.BusinessId}' not found.");

            var customer = await _customerRepository.GetByIdAsync(dto.CustomerId);
            if (customer == null)
                throw new ArgumentException($"Customer with id '{dto.CustomerId}' not found.");

            var ticket = new Ticket
            {
                Id = Guid.NewGuid().ToString(),
                Subject = dto.Subject,
                Status = "Open",
                IsEnded = false,
                BusinessId = dto.BusinessId,
                CustomerId = dto.CustomerId,
                CreatedAt = DateTime.UtcNow
            };

            await _ticketRepository.AddAsync(ticket);
            await _unitOfWork.CompleteAsync();
            return ticket;
        }

        public async Task<Ticket?> UpdateAsync(string id, TicketUpdateDTO dto)
        {
            var ticket = await _ticketRepository.GetByIdAsync(id);
            if (ticket == null) return null;

            ticket.Subject = dto.Subject;
            ticket.Status = dto.Status;

            _ticketRepository.Update(ticket);
            await _unitOfWork.CompleteAsync();
            return ticket;
        }

        public async Task<Ticket?> AssignTicketAsync(string id, AssignTicketDTO dto)
        {
            var ticket = await _ticketRepository.GetByIdAsync(id);
            if (ticket == null) return null;

            if (!string.IsNullOrEmpty(dto.UserId))
            {
                var user = await _userRepository.GetByIdAsync(dto.UserId);
                if (user == null)
                    throw new ArgumentException($"User with id '{dto.UserId}' not found.");

                ticket.AssignedToUserId = dto.UserId;
            }

            // When an agent is assigned, ticket moves into active work state.
            ticket.Status = "InProgress";

            _ticketRepository.Update(ticket);
            await _unitOfWork.CompleteAsync();
            return ticket;
        }

        public async Task<Ticket?> CloseTicketAsync(string id, CloseTicketDTO dto)
        {
            var ticket = await _ticketRepository.GetByIdAsync(id);
            if (ticket == null) return null;

            // Mark ticket as closed
            ticket.Status = "Closed";
            ticket.IsEnded = true;
            ticket.ClosedAt = DateTime.UtcNow;

            // If ClosedByUserId is provided, ensure the user exists and store as assignee/handler
            if (!string.IsNullOrWhiteSpace(dto.ClosedByUserId))
            {
                var user = await _userRepository.GetByIdAsync(dto.ClosedByUserId);
                if (user == null)
                    throw new ArgumentException($"User with id '{dto.ClosedByUserId}' not found.");

                ticket.AssignedToUserId = dto.ClosedByUserId;
            }

            _ticketRepository.Update(ticket);

            // Also close the related interaction if any
            if (!string.IsNullOrWhiteSpace(ticket.InteractionId))
            {
                var interaction = await _unitOfWork.Interactions.GetByIdAsync(ticket.InteractionId);
                if (interaction != null)
                {
                    interaction.Status = "Closed";
                    interaction.IsEnded = true;
                    interaction.EndedAt = DateTime.UtcNow;
                    _unitOfWork.Interactions.Update(interaction);
                }
            }

            await _unitOfWork.CompleteAsync();
            return ticket;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var ticket = await _ticketRepository.GetByIdAsync(id);
            if (ticket == null) return false;

            _ticketRepository.Delete(ticket);
            await _unitOfWork.CompleteAsync();
            return true;
        }
    }
}

