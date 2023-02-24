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
    public class MailTemplateManager : IMailTemplateService
    {
        private readonly IMailTemplateDal _mailTemplateDal;

        public MailTemplateManager(IMailTemplateDal mailTemplateDal)
        {
            _mailTemplateDal = mailTemplateDal;
        }

        public IResult Add(MailTemplate mailTemplate)
        {
            _mailTemplateDal.Add(mailTemplate);
            return new SuccessResult(Messages.MailTemplateAdd);
        }

        public IResult Delete(MailTemplate mailTemplate)
        {
            _mailTemplateDal.Delete(mailTemplate);
            return new SuccessResult(Messages.RemovedMailTemplate);
        }

        public IDataResult<MailTemplate> Get(int id)
        {
            return new SuccessDataResult<MailTemplate>(_mailTemplateDal.Get(i => i.Id == id));
        }

        public IDataResult<List<MailTemplate>> GetAll(int companyId)
        {
            return new SuccessDataResult<List<MailTemplate>>(_mailTemplateDal.GetAll(i => i.CompanyId == companyId));
        }

        public IDataResult<MailTemplate> GetByTemplateName(int companyId, string type)
        {
            return new SuccessDataResult<MailTemplate>(_mailTemplateDal.Get(i => i.CompanyId == companyId && i.Type == type));
        }

        public IResult Update(MailTemplate mailTemplate)
        {
            _mailTemplateDal.Update(mailTemplate);
            return new SuccessResult(Messages.MailTemplateUpdated);
        }
    }
}
