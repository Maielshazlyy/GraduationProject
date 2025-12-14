using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Service_layer.DTOS.Integration;

namespace Service_layer.Validators.Integration
{
    public class IntegrationConnectDTOValidator : AbstractValidator<IntegrationConnectDTO>
    {
        public IntegrationConnectDTOValidator()
        {
            RuleFor(x => x.BusinessId).NotEmpty();
            RuleFor(x => x.PlatformName).NotEmpty();
            RuleFor(x => x.ApiKeyOrConfig).NotEmpty();
        }
    }
}
