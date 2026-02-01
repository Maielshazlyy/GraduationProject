using System;
using Domain_layer.Interfaces;
using Domain_layer.Models;
using Service_layer.DTOS.Feedback;
using Service_layer.Services_Interfaces;

namespace Service_layer.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IRepository<Feedback> _feedbackRepository;
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<Ticket> _ticketRepository;
        private readonly IUnitOfWork _unitOfWork;

        public FeedbackService(
            IRepository<Feedback> feedbackRepository,
            IRepository<Customer> customerRepository,
            IRepository<Ticket> ticketRepository,
            IUnitOfWork unitOfWork)
        {
            _feedbackRepository = feedbackRepository;
            _customerRepository = customerRepository;
            _ticketRepository = ticketRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Feedback>> GetAllAsync()
        {
            return await _feedbackRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Feedback>> GetByCustomerIdAsync(string customerId)
        {
            var allFeedbacks = await _feedbackRepository.GetAllAsync();
            return allFeedbacks.Where(f => f.CustomerId == customerId);
        }

        public async Task<Feedback?> GetByIdAsync(string id)
        {
            return await _feedbackRepository.GetByIdAsync(id);
        }

        public async Task<Feedback> CreateAsync(FeedbackCreateDTO dto)
        {
            var customer = await _customerRepository.GetByIdAsync(dto.CustomerId);
            if (customer == null)
                throw new ArgumentException($"Customer with id '{dto.CustomerId}' not found.");

            if (!string.IsNullOrEmpty(dto.TicketId))
            {
                var ticket = await _ticketRepository.GetByIdAsync(dto.TicketId);
                if (ticket == null)
                    throw new ArgumentException($"Ticket with id '{dto.TicketId}' not found.");
            }

            if (dto.Rating < 1 || dto.Rating > 5)
                throw new ArgumentException("Rating must be between 1 and 5.");

            var feedback = new Feedback
            {
                FeedbackId = Guid.NewGuid().ToString(),
                CustomerId = dto.CustomerId,
                TicketId = string.IsNullOrEmpty(dto.TicketId) ? null : dto.TicketId,
                Rating = dto.Rating,
                Comment = dto.Comment,
                CreatedAt = DateTime.UtcNow
            };

            await _feedbackRepository.AddAsync(feedback);
            await _unitOfWork.CompleteAsync();
            return feedback;
        }

        public async Task<Feedback?> UpdateAsync(string id, FeedbackUpdateDTO dto)
        {
            var feedback = await _feedbackRepository.GetByIdAsync(id);
            if (feedback == null) return null;

            if (dto.Rating < 1 || dto.Rating > 5)
                throw new ArgumentException("Rating must be between 1 and 5.");

            feedback.Rating = dto.Rating;
            feedback.Comment = dto.Comment;

            _feedbackRepository.Update(feedback);
            await _unitOfWork.CompleteAsync();
            return feedback;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var feedback = await _feedbackRepository.GetByIdAsync(id);
            if (feedback == null) return false;

            _feedbackRepository.Delete(feedback);
            await _unitOfWork.CompleteAsync();
            return true;
        }
    }
}

