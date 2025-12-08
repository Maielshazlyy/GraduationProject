using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Service_layer.DTOS.Setting;

namespace Service_layer.Validators.Setting
{
    public class SettingUpdateDTOValidator : AbstractValidator<SettingUpdateDTO>
    {
        public SettingUpdateDTOValidator()
        {
            RuleFor(x => x.Language).NotEmpty();
        }
    }
}
