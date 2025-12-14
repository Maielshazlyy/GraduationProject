using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Service_layer.DTOS.KnowledgeBase;

namespace Service_layer.Validators.KnowledgeBase
{
    public class KnowledgeBaseCreateDTOValidator : AbstractValidator<KnowledgeBaseCreateDTO>
    {
        public KnowledgeBaseCreateDTOValidator()
        {
            RuleFor(x => x.Question).NotEmpty();
            RuleFor(x => x.Answer).NotEmpty();
            RuleFor(x => x.BusinessId).NotEmpty();
        }
    }
}
