using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain_layer.Constants;
using FluentValidation;
using Service_layer.DTOS.User;

namespace Service_layer.Validators.User
{
    public class AssignRoleDTOValidator: AbstractValidator<AssignRoleDTO>
    {
        public AssignRoleDTOValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty();

            RuleFor(x => x.NewRole)
        .Must(r => r == Roles.Owner || r == Roles.Admin || r == Roles.Agent) // ✅ كود نظيف وآمن
         .WithMessage("Invalid role selected.");
        }
    }
}
