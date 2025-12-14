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
                .NotEmpty();

            RuleFor(x => x.BusinessId)
                .NotEmpty();

            RuleFor(x => x.Items)
                .NotEmpty().WithMessage("Order must contain at least 1 item");
        }
        }
}
