using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Service_layer.DTOS.Integration;

namespace Service_layer.Validators.Integration
{
    public class IntegrationSyncDTOValidator : AbstractValidator<IntegrationSyncDTO>
    {
        public IntegrationSyncDTOValidator()
        {
            RuleFor(x => x.IntegrationId).NotEmpty();
            RuleFor(x => x.SyncType).NotEmpty();
        }
    }
}
