using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Service_layer.DTOS.Order;

namespace Service_layer.Validators.Order
{
    public class UpdateOrderStatusDTOValidator: AbstractValidator<UpdateOrderStatusDTO>
    {
        public UpdateOrderStatusDTOValidator()
        {
            RuleFor(x => x.OrderId)
                .GreaterThan(0);

            RuleFor(x => x.Status)
                .Must(s => s == "Pending" || s == "InProgress" || s == "Completed" || s == "Cancelled")
                .WithMessage("Invalid order status");
        }
    }
}
