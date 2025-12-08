using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Service_layer.DTOS.Customer;

namespace Service_layer.Validators.Customer
{
    public class CustomerUpdateDTOValidator: AbstractValidator<CustomerUpdateDTO>
    {
        public CustomerUpdateDTOValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Email)
                .EmailAddress().When(x => !string.IsNullOrEmpty(x.Email));

            RuleFor(x => x.Phone)
                .Matches(@"^[0-9+\- ]+$")
                .When(x => !string.IsNullOrEmpty(x.Phone));
        }
    }
}
