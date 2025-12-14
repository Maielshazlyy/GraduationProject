using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Service_layer.DTOS.Interaction;

namespace Service_layer.Validators.Interaction
{
    public class StartInteractionDTOValidator : AbstractValidator<StartInteractionDTO>
    {
        public StartInteractionDTOValidator()
        {
            RuleFor(x => x.CustomerId)
                .NotEmpty();

            RuleFor(x => x.BusinessId)
                .NotEmpty();

            RuleFor(x => x.Channel)
                .NotEmpty()
                .Must(c => c == "WhatsApp" || c == "Web" || c == "Voice")
                .WithMessage("Channel must be WhatsApp, Web, or Voice.");
        }
    }
}
