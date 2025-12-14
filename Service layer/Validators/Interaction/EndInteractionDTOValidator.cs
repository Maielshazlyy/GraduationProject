using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Service_layer.DTOS.Interaction;

namespace Service_layer.Validators.Interaction
{
    public class EndInteractionDTOValidator:AbstractValidator<EndInteractionDTO>
    {
        public EndInteractionDTOValidator()
        {
            RuleFor(x => x.InteractionId)
                .NotEmpty();
        }
    }
}
