using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Service_layer.DTOS.menuItem;

namespace Service_layer.Validators.MenuItem
{
    public class MenuItemCreateDTOValidator : AbstractValidator<MenuItemCreateDTO>
    {
        public MenuItemCreateDTOValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Price).GreaterThan(0);
            RuleFor(x => x.Category).NotEmpty();
            RuleFor(x => x.BusinessId).NotEmpty();
        }
    }
}
