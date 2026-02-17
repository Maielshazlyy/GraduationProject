using FluentValidation;
using Service_layer.DTOS.Chatbot;

namespace Service_layer.Validators.Chatbot
{
    public class ChatbotRequestDTOValidator : AbstractValidator<ChatbotRequestDTO>
    {
        public ChatbotRequestDTOValidator()
        {
            RuleFor(x => x.Question)
                .NotEmpty().WithMessage("Question is required.")
                .MaximumLength(1000).WithMessage("Question must not exceed 1000 characters.");
        }
    }
}

