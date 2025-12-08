using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Service_layer.DTOS.Reports;

namespace Service_layer.Validators.Report
{
    public class ReportCreateDTOValidator :AbstractValidator<ReportCreateDTO>
    {
        public ReportCreateDTOValidator()
        {
            RuleFor(x => x.Title).NotEmpty();
        }
    }
}
