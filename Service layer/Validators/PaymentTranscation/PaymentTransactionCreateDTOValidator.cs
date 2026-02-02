using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Service_layer.DTOS.PaymentTranscation;

namespace Service_layer.Validators.PaymentTranscation
{
    public class PaymentTransactionCreateDTOValidator : AbstractValidator<PaymentTransactionCreateDTO>
    {
        public PaymentTransactionCreateDTOValidator()
        {
            RuleFor(x => x.Amount).GreaterThan(0);
            RuleFor(x => x.SubscriptionId).NotEmpty();
            RuleFor(x => x.PaymentMethod).NotEmpty();
        }
    }
}
