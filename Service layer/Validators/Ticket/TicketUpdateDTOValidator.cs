using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Service_layer.DTOS.Ticket;

namespace Service_layer.Validators.Ticket
{
    public class  TicketUpdateDTOValidator: AbstractValidator<TicketUpdateDTO>
    {
        public TicketUpdateDTOValidator()
        {
            RuleFor(x => x.Subject)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.Status)
                .NotEmpty()
                .Must(s => s == "Open" || s == "InProgress" || s == "Closed")
                .WithMessage("Status must be Open, InProgress, or Closed");
        }
    }
}
