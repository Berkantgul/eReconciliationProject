using Entities.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ValidationRules.FluentValidation
{
    public class CompanyValidator : AbstractValidator<Company>
    {
        public CompanyValidator()
        {
            RuleFor(i=>i.Name).NotEmpty().WithMessage("Şirket ismi boş geçilemez!");
            RuleFor(i => i.Name).MinimumLength(4).WithMessage("Şirket adı minimum 4 karakter olmalıdır!");


        }
    }
}
