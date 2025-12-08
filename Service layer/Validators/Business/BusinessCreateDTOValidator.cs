using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Service_layer.DTOS.Business;

namespace Service_layer.Validators.Business
{
    public class BusinessCreateDTOValidator : AbstractValidator<BusinessCreateDTO>
    {
        public BusinessCreateDTOValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Type).NotEmpty();
            RuleFor(x => x.Address).NotEmpty();
            RuleFor(x => x.Phone).NotEmpty().MaximumLength(20); ;
        }
    }
}
