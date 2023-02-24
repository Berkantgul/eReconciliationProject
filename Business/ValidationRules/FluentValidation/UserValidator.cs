using Core.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ValidationRules.FluentValidation
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(i => i.Name).NotEmpty().WithMessage("Ad kısmı boş geçilemez!");
            RuleFor(i => i.Name).MinimumLength(4).WithMessage("En az 4 karakter olmalıdır!");

            RuleFor(i => i.Email).NotEmpty().WithMessage("Mail kısmı boş geçilemez!");
            RuleFor(i => i.Email).EmailAddress().WithMessage("Mail kurallarını uyunuz!");
        }
    }
}
