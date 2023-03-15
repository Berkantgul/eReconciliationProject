using Business.Abstract;
using Business.Contans;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class TermsAndConditionManager : ITermsAndConditionService
    {
        private readonly ITermsAndConditionDal _termsAndConditionDal;

        public TermsAndConditionManager(ITermsAndConditionDal termsAndConditionDal)
        {
            _termsAndConditionDal = termsAndConditionDal;
        }

        public IDataResult<TermsAndCondition> Get()
        {
            return new SuccessDataResult<TermsAndCondition>(_termsAndConditionDal.GetAll().FirstOrDefault());
        }

        public IResult Update(TermsAndCondition termsAndCondition)
        {
            var exists = _termsAndConditionDal.GetAll().FirstOrDefault();
            if (exists != null)
            {
                exists.Description = termsAndCondition.Description;
                _termsAndConditionDal.Update(exists);
            }
            else
            {
                _termsAndConditionDal.Add(termsAndCondition);
            }
            return new SuccessResult(Messages.AddedTermsAndCondition);
        }
    }
}
