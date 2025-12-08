using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Service_layer.DTOS.Order;

namespace Service_layer.Validators.Order
{
    public class OrderCreateDTOValidator: AbstractValidator<OrderCreateDTO>
    {
        public OrderCreateDTOValidator()
        {
            RuleFor(x => x.CustomerId)
                .GreaterThan(0);

            RuleFor(x => x.BusinessId)
                .GreaterThan(0);

            RuleFor(x => x.Items)
                .NotEmpty().WithMessage("Order must contain at least 1 item");
        }
        }
}
