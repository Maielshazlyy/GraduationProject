using System;
using Domain_layer.Interfaces;
using Domain_layer.Models;
using Service_layer.DTOS.Message;
using Service_layer.Services_Interfaces;

namespace Service_layer.Services
{
    public class MessageService : IMessageService
    {
        private readonly IRepository<Message> _messageRepository;
        private readonly IRepository<Interaction> _interactionRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public MessageService(
            IRepository<Message> messageRepository,
            IRepository<Interaction> interactionRepository,
            IRepository<User> userRepository,
            IUnitOfWork unitOfWork)
        {
            _messageRepository = messageRepository;
            _interactionRepository = interactionRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Message>> GetAllAsync()
        {
            return await _messageRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Message>> GetByInteractionIdAsync(string interactionId)
        {
            var allMessages = await _messageRepository.GetAllAsync();
            return allMessages.Where(m => m.InteractionId == interactionId);
        }

        public async Task<Message?> GetByIdAsync(string id)
        {
            return await _messageRepository.GetByIdAsync(id);
        }

        public async Task<Message> CreateAsync(MessageCreateDTO dto)
        {
            var interaction = await _interactionRepository.GetByIdAsync(dto.InteractionId);
            if (interaction == null)
                throw new ArgumentException($"Interaction with id '{dto.InteractionId}' not found.");

            if (!string.IsNullOrEmpty(dto.UserId))
            {
                var user = await _userRepository.GetByIdAsync(dto.UserId);
                if (user == null)
                    throw new ArgumentException($"User with id '{dto.UserId}' not found.");
            }

            var message = new Message
            {
                MessageId = Guid.NewGuid().ToString(),
                InteractionId = dto.InteractionId,
                UserId = string.IsNullOrEmpty(dto.UserId) ? null : dto.UserId,
                SenderType = dto.SenderType, // "Customer" or "Agent"
                Content = dto.Content,
                SentAt = DateTime.UtcNow
            };

            await _messageRepository.AddAsync(message);
            await _unitOfWork.CompleteAsync();
            return message;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var message = await _messageRepository.GetByIdAsync(id);
            if (message == null) return false;

            _messageRepository.Delete(message);
            await _unitOfWork.CompleteAsync();
            return true;
        }
    }
}

