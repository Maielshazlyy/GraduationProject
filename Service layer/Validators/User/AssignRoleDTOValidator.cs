using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Service_layer.DTOS.User;

namespace Service_layer.Validators.User
{
    public class AssignRoleDTOValidator: AbstractValidator<AssignRoleDTO>
    {
        public AssignRoleDTOValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0);

            RuleFor(x => x.NewRole)
                .NotEmpty()
                .Must(r => r == "Owner" || r == "Admin" || r == "Agent")
                .WithMessage("Invalid role.");
        }
    }
}
