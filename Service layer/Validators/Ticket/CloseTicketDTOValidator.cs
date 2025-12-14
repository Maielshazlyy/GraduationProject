using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Service_layer.DTOS.Ticket;

namespace Service_layer.Validators.Ticket
{
    public class CloseTicketDTOValidator: AbstractValidator<CloseTicketDTO>
    {
        public CloseTicketDTOValidator()
        {
            RuleFor(x => x.TicketId)
                .NotEmpty();

            RuleFor(x => x.ClosedByUserId)
                .NotEmpty();
        }
    }
}
