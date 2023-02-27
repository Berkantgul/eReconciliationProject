using Entities.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ValidationRules.FluentValidation
{
    public class CurrencyAccountValidator : AbstractValidator<CurrencyAccount>
    {
        public CurrencyAccountValidator()
        {
            RuleFor(c => c.Name).NotEmpty().WithMessage("Firma Adı boş geçilemez.");
            RuleFor(c => c.Name).MinimumLength(4).WithMessage("Firma Adı en az 4 karakter olmalıdır.");
            RuleFor(c => c.Address).NotEmpty().WithMessage("Firma Adresi boş geçilemez.");
            RuleFor(c => c.Address).MinimumLength(4).WithMessage("Firma Adresi en az 4 karakter olmalıdır.");
        }
    }
}
