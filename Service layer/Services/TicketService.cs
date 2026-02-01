using System;
using Domain_layer.Interfaces;
using Domain_layer.Models;
using Service_layer.DTOS.Ticket;
using Service_layer.Services_Interfaces;

namespace Service_layer.Services
{
    public class TicketService : ITicketService
    {
        private readonly IRepository<Ticket> _ticketRepository;
        private readonly IRepository<Business> _businessRepository;
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TicketService(
            IRepository<Ticket> ticketRepository,
            IRepository<Business> businessRepository,
            IRepository<Customer> customerRepository,
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
            var allTickets = await _ticketRepository.GetAllAsync();
            return allTickets.Where(t => t.BusinessId == businessId);
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

            ticket.Status = "In Progress";

            _ticketRepository.Update(ticket);
            await _unitOfWork.CompleteAsync();
            return ticket;
        }

        public async Task<Ticket?> CloseTicketAsync(string id, CloseTicketDTO dto)
        {
            var ticket = await _ticketRepository.GetByIdAsync(id);
            if (ticket == null) return null;

            ticket.Status = "Closed";
            ticket.IsEnded = true;
            ticket.ClosedAt = DateTime.UtcNow;

            _ticketRepository.Update(ticket);
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

