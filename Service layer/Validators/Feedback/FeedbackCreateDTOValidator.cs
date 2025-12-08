using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Service_layer.DTOS.Feedback;

namespace Service_layer.Validators.Feedback
{
    public class FeedbackCreateDTOValidator: AbstractValidator<FeedbackCreateDTO>
    {
        public FeedbackCreateDTOValidator()
        {
            RuleFor(x => x.TicketId)
                .GreaterThan(0)
                .WithMessage("TicketId is required");

            RuleFor(x => x.CustomerId)
                .GreaterThan(0)
                .WithMessage("CustomerId is required");

            RuleFor(x => x.Rating)
                .InclusiveBetween(1, 5)
                .WithMessage("Rating must be between 1 and 5");

            RuleFor(x => x.Comment)
                .MaximumLength(500)
                .WithMessage("Comment must not exceed 500 characters");
        }
    }
}
