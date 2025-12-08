using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Service_layer.DTOS.menuItem;

namespace Service_layer.Validators.MenuItem
{
    public class MenuItemUpdateDTOValidator : AbstractValidator<MenuItemUpdateDTO>
    {
        public MenuItemUpdateDTOValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Price).GreaterThan(0);
            RuleFor(x => x.Category).NotEmpty();
        }
    }
}
