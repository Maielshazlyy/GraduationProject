using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Service_layer.DTOS.Auth;

namespace Service_layer.Validators.Auth
{
    public class RegisterDTOValidator: AbstractValidator<RegisterDTO>
    {
        public RegisterDTOValidator()
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

            // التحقق من أن الـ BusinessId ليس فارغاً (لأنه string)
            RuleFor(x => x.BusinessId)
                .NotEmpty().WithMessage("BusinessId is required.");

            // التحقق من الأدوار المسموحة
            RuleFor(x => x.Role)
                .NotEmpty()
                .Must(r => r == "Owner" || r == "Admin" || r == "Agent")
                .WithMessage("Role must be Owner, Admin, or Agent.");
        }
    }
}
