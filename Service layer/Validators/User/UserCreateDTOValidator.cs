using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Service_layer.DTOS.User;

namespace Service_layer.Validators.User
{
    public class UserCreateDTOValidator : AbstractValidator<UserCreateDTO>
    {
        public UserCreateDTOValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full name is required.")
                .MaximumLength(50);

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Email format is invalid.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters.");

            RuleFor(x => x.BusinessId)
                .GreaterThan(0).WithMessage("BusinessId is required.");

            RuleFor(x => x.Role)
                .NotEmpty()
                .Must(r => r == "Owner" || r == "Admin" || r == "Agent")
                .WithMessage("Role must be Owner, Admin, or Agent.");
        }
    }
}
