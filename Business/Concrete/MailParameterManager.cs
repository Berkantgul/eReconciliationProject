﻿using Business.Abstract;
using Business.BusinessAspects;
using Business.Contans;
using Core.Aspects.Autofac.Performance;
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
    public class MailParameterManager : IMailParameterService
    {
        private readonly IMailParameterDal _mailParameterDal;

        public MailParameterManager(IMailParameterDal mailParameterDal)
        {
            _mailParameterDal = mailParameterDal;
        }

        public IDataResult<MailParameter> Get(int companyId)
        {
            return new SuccessDataResult<MailParameter>(_mailParameterDal.Get(i => i.CompanyId == companyId));
        }

        [PerformanceAspect(3)]
        [SecuredOperations("MailParameter.Update,Admin")]
        public IResult Update(MailParameter mailParameter)
        {
            var result = Get(mailParameter.CompanyId);
            if (result.Data == null)
            {
                _mailParameterDal.Add(mailParameter);
            }
            else
            {
                result.Data.Port = mailParameter.Port;
                result.Data.SSL = mailParameter.SSL;
                result.Data.Email = mailParameter.Email;
                result.Data.SMTP = mailParameter.SMTP;
                result.Data.Password = mailParameter.Password;

                _mailParameterDal.Update(result.Data);
            }
            return new SuccessResult(Messages.MailParameterUpdated);
        }
    }
}
