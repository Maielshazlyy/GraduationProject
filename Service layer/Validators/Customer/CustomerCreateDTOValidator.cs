using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Service_layer.DTOS.Customer;

namespace Service_layer.Validators.Customer
{
    public class CustomerCreateDTOValidator: AbstractValidator<CustomerCreateDTO>
    {
        public CustomerCreateDTOValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full name is required")
                .MaximumLength(100);

            RuleFor(x => x.BusinessId)
                .GreaterThan(0).WithMessage("BusinessId is required");

            // Optional but must have at least one (Email OR Phone)
            RuleFor(x => x)
                .Must(c => !string.IsNullOrWhiteSpace(c.Email) || !string.IsNullOrWhiteSpace(c.Phone))
                .WithMessage("Either Email or Phone must be provided");

            RuleFor(x => x.Email)
                .EmailAddress().When(x => !string.IsNullOrEmpty(x.Email))
                .WithMessage("Invalid Email format");

            RuleFor(x => x.Phone)
                .Matches(@"^[0-9+\- ]+$")
                .When(x => !string.IsNullOrEmpty(x.Phone))
                .WithMessage("Invalid Phone number format");
        }
    }
}
