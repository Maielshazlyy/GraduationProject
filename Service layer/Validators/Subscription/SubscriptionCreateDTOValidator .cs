using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Service_layer.DTOS.Subscription;

namespace Service_layer.Validators.Subscription
{
    public class SubscriptionCreateDTOValidator : AbstractValidator<SubscriptionCreateDTO>
    {
        public SubscriptionCreateDTOValidator()
        {
            RuleFor(x => x.PlanName).NotEmpty();
            RuleFor(x => x.Price).GreaterThan(0);
            RuleFor(x => x.BusinessId).NotEmpty();
        }
    }
}
