using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Service_layer.DTOS.Notification;

namespace Service_layer.Validators.Notification
{
    public class NotificationCreateDTOValidator : AbstractValidator<NotificationCreateDTO>
    {
        public NotificationCreateDTOValidator()
        {
            RuleFor(x => x.Title).NotEmpty();
            RuleFor(x => x.Message).NotEmpty();
            RuleFor(x => x.UserId).NotEmpty();
        }
    }
}
