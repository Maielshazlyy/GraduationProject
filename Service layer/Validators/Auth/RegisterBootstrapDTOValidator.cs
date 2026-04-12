using FluentValidation;
using Service_layer.DTOS.Auth;

namespace Service_layer.Validators.Auth
{
    public class RegisterBootstrapDTOValidator : AbstractValidator<RegisterBootstrapDTO>
    {
        public RegisterBootstrapDTOValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full name is required.")
                .MaximumLength(50);

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters.");
        }
    }
}
