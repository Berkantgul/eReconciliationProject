﻿using Core.Utilities.Results.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IMailTemplateService
    {
        IResult Add(MailTemplate mailTemplate);
        IResult Delete(MailTemplate mailTemplate);
        IResult Update(MailTemplate mailTemplate);
        IDataResult<List<MailTemplate>> GetAll(int companyId);
        IDataResult<MailTemplate> Get(int id);
        IDataResult<MailTemplate> GetByTemplateName(int companyId, string type);
    }
}
