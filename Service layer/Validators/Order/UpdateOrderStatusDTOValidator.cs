using Domain_layer.enums;
using FluentValidation;
using Service_layer.DTOS.Order;

namespace Service_layer.Validators.Order
{
    public class UpdateOrderStatusDTOValidator : AbstractValidator<UpdateOrderStatusDTO>
    {
        private static readonly string[] ValidStatuses = Enum.GetNames(typeof(OrderStatus));

        public UpdateOrderStatusDTOValidator()
        {
            RuleFor(x => x.OrderId)
                .NotEmpty().WithMessage("OrderId is required.");

            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("Status is required.")
                .Must(s => ValidStatuses.Contains(s))
                .WithMessage($"Invalid order status. Valid values are: {string.Join(", ", Enum.GetNames(typeof(OrderStatus)))}");
        }
    }
}
