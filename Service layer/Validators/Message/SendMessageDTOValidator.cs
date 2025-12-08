using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Service_layer.DTOS.Message;

namespace Service_layer.Validators.Message
{
    public class SendMessageDTOValidator: AbstractValidator<MessageCreateDTO>
    {
        public SendMessageDTOValidator()
        {
            RuleFor(x => x.InteractionId)
                .GreaterThan(0);

            RuleFor(x => x.SenderType)
                .NotEmpty()
                .Must(t => t == "AI" || t == "Customer" || t == "Agent")
                .WithMessage("SenderType must be AI, Customer, or Agent.");

            RuleFor(x => x.Content)
                .NotEmpty()
                .MaximumLength(1000);

            When(x => x.SenderType == "Agent", () =>
            {
                RuleFor(x => x.UserId)
                    .NotNull()
                    .GreaterThan(0);
            });
        }
        }
}
