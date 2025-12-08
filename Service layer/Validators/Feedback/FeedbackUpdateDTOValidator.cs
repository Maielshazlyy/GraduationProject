using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Service_layer.DTOS.Feedback;

namespace Service_layer.Validators.Feedback
{
    public class FeedbackUpdateDTOValidator: AbstractValidator<FeedbackUpdateDTO>
    {
        public FeedbackUpdateDTOValidator()
        {
            RuleFor(x => x.Rating)
                .InclusiveBetween(1, 5);

            RuleFor(x => x.Comment)
                .MaximumLength(500);
        }
    }
}
