using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Service_layer.DTOS.Subscription;

namespace Service_layer.Validators.Subscription
{
    public class SubscriptionRenewDTOValidator : AbstractValidator<SubscriptionRenewDTO>
    {
        public SubscriptionRenewDTOValidator()
        {
            RuleFor(x => x.SubscriptionId)
                .GreaterThan(0)
                .WithMessage("SubscriptionId is required");

            RuleFor(x => x.NewStartDate)
                .NotEmpty()
                .WithMessage("NewStartDate is required");

            RuleFor(x => x.NewEndDate)
                .GreaterThan(x => x.NewStartDate)
                .When(x => x.NewEndDate.HasValue)
                .WithMessage("NewEndDate must be after NewStartDate");

            RuleFor(x => x.NewPrice)
                .GreaterThan(0)
                .When(x => x.NewPrice.HasValue)
                .WithMessage("New price must be greater than 0");

            RuleFor(x => x.NewPlanName)
                .NotEmpty()
                .When(x => x.NewPrice.HasValue)
                .WithMessage("PlanName is required when changing price");
        }
    }
}
