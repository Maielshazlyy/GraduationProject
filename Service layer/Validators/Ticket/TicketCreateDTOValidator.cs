using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Service_layer.DTOS.Ticket;

namespace Service_layer.Validators.Ticket
{
    public class TicketCreateDTOValidator: AbstractValidator<TicketCreateDTO>
    {
        public TicketCreateDTOValidator()
        {
            RuleFor(x => x.Subject)
                .NotEmpty().WithMessage("Subject is required")
                .MaximumLength(200);

            RuleFor(x => x.CustomerId)
                .NotEmpty().WithMessage("CustomerId is required");

            RuleFor(x => x.BusinessId)
                .NotEmpty().WithMessage("BusinessId is required");
        }
    }
}
