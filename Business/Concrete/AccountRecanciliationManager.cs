using Business.Abstract;
using Business.BusinessAspects;
using Business.Contans;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Performance;
using Core.Aspects.Autofac.Transaction;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using Entities.Dtos;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class AccountRecanciliationManager : IAccountRecanciliationService
    {
        private readonly IAccountRecanciliationDal _accountRecanciliationDal;
        private readonly ICurrencyAccountService _currencyAccountService;
        private readonly IMailService _mailService;
        private readonly IMailTemplateService _mailTemplateService;
        private readonly IMailParameterService _mailParameterService;

        public AccountRecanciliationManager(IAccountRecanciliationDal accountRecanciliationDal, ICurrencyAccountService currencyAccountService, IMailService mailService, IMailTemplateService mailTemplateService, IMailParameterService mailParameterService)
        {
            _accountRecanciliationDal = accountRecanciliationDal;
            _currencyAccountService = currencyAccountService;
            _mailService = mailService;
            _mailTemplateService = mailTemplateService;
            _mailParameterService = mailParameterService;
        }

        [PerformanceAspect(3)]
        [SecuredOperations("AccountReconciliation.Add,Admin")]
        [CacheRemoveAspect("IAccountRecanciliationService.Get")]
        public IResult Add(AccountRecanciliation accountRecanciliation)
        {
            string guid = Guid.NewGuid().ToString();
            accountRecanciliation.Guid = guid;
            _accountRecanciliationDal.Add(accountRecanciliation);
            return new SuccessResult(Messages.AddedAccountRecanciliation);
        }

        [SecuredOperations("AccountReconciliation.Add,Admin")]
        [PerformanceAspect(3)]
        [CacheRemoveAspect("IAccountRecanciliationService.Get")]
        [TransactionScopeAspect]
        public IResult AddToExcel(string filePath, int companyId)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    while (reader.Read())
                    {
                        string code = reader.GetString(0);


                        if (code != "Cari Kodu" && code != null)
                        {
                            DateTime startingDate = reader.GetDateTime(1);
                            DateTime endingDate = reader.GetDateTime(2);
                            double currencyId = reader.GetDouble(3);
                            double debit = reader.GetDouble(4);
                            double credit = reader.GetDouble(5);
                            int currencyAccountId = _currencyAccountService.GetCode(code, companyId).Data.Id;
                            string guid = Guid .NewGuid().ToString();

                            AccountRecanciliation accountRecanciliation = new AccountRecanciliation
                            {
                                CurrencyAccountId = currencyAccountId,
                                StartingDate = startingDate,
                                EndingDate = endingDate,
                                CurrencyDebit = Convert.ToDecimal(debit),
                                CurrencyId = Convert.ToInt16(currencyId),
                                CurrencyCredit = Convert.ToDecimal(credit),
                                CompanyId = companyId,
                                SendEmailDate = DateTime.Now,
                                EmailReadDate = DateTime.Now,
                                Guid = guid
                            };

                            _accountRecanciliationDal.Add(accountRecanciliation);
                        }
                    }
                }
            }
            return new SuccessResult(Messages.FromExcelAddToAccountReconciliaiton);
        }

        [PerformanceAspect(3)]
        [SecuredOperations("AccountReconciliation.Delete,Admin")]
        [CacheRemoveAspect("IAccountRecanciliationService.Get")]
        public IResult Delete(AccountRecanciliation accountRecanciliation)
        {
            _accountRecanciliationDal.Delete(accountRecanciliation);
            return new SuccessResult(Messages.DeletedAccountRecanciliation);
        }

        [PerformanceAspect(3)]
        public IDataResult<AccountRecanciliation> GetByCode(string code)
        {
            // guid mutabakta ulaşabilmek için eklendi.
            return new SuccessDataResult<AccountRecanciliation>(_accountRecanciliationDal.Get(i => i.Guid == code));
        }

        [SecuredOperations("AccountReconciliation.Get,Admin")]
        [PerformanceAspect(3)]
        [CacheAspect(60)]
        public IDataResult<AccountRecanciliation> GetById(int id)
        {
            return new SuccessDataResult<AccountRecanciliation>(_accountRecanciliationDal.Get(i => i.Id == id));
        }

        [PerformanceAspect(3)]
        [SecuredOperations("AccountReconciliation.GetList,Admin")]
        [CacheAspect(60)]
        public IDataResult<List<AccountRecanciliation>> GetList(int companyId)
        {
            return new SuccessDataResult<List<AccountRecanciliation>>(_accountRecanciliationDal.GetAll(i => i.CompanyId == companyId));
        }

        [PerformanceAspect(3)]
        [SecuredOperations("AccountReconciliation.GetList,Admin")]
        [CacheAspect(60)]
        public IDataResult<List<Entities.Dtos.AccountRecanciliationDto>> GetListDto(int companyId)
        {
            return new SuccessDataResult<List<AccountRecanciliationDto>>(_accountRecanciliationDal.GetAllDto(companyId));
        }

        [PerformanceAspect(3)]
        [SecuredOperations("AccountRecanciliation.SendMail,Admin")]
        public IResult SendReconciliationMail(AccountRecanciliationDto accountRecanciliationDto)
        {
            string subject = "Mutabakat Mail";
            string body = $"Şirket Adımız : {accountRecanciliationDto.CompanyName} <br />" +
                          $"Şirket Vergi Dairesi : {accountRecanciliationDto.CompanyTaxDepartment} <br />" +
                          $"Şirket Vergi Numarası : {accountRecanciliationDto.CompanyTaxIdNumber} - {accountRecanciliationDto.CompanyIdentityNumber} <br/ ><hr />" +
                          $"Sizin Şirket : {accountRecanciliationDto.AccountName} <br />" +
                          $"Sizin Şirket Vergi Dairesi : {accountRecanciliationDto.AccountIdentityNumber} <br />" +
                          $"Sizin şirket Vergi Numarası : {accountRecanciliationDto.AccountTaxIdNumber} - {accountRecanciliationDto.AccountIdentityNumber} <br /><hr />" +
                          $"Borç : {accountRecanciliationDto.CurrencyCredit} {accountRecanciliationDto.CurrencyCode} <br />" +
                          $"Alacak : {accountRecanciliationDto.CurrencyDebit} {accountRecanciliationDto.CurrencyCode}";

            string link = "https://localhost:7127/api/AccountReconciliation/GetByCode?code=" + accountRecanciliationDto.Guid;
            string linkDescription = "Mutabakatı cevaplamak için tıklayın";

            var mailTemplate = _mailTemplateService.GetByTemplateName(13, "Kayıt");
            string templateBody = mailTemplate.Data.Value;
            templateBody = templateBody.Replace("{{title}}", subject);
            templateBody = templateBody.Replace("{{message}}", body);
            templateBody = templateBody.Replace("{{link}}", link);
            templateBody = templateBody.Replace("{{linkDescription}}", linkDescription);

            var mailParameter = _mailParameterService.Get(13);
            Entities.Dtos.SendMailDto sendMailDto = new Entities.Dtos.SendMailDto
            {
                email = accountRecanciliationDto.AccountEmail,
                subject = subject,
                body = templateBody,
                mailParameter = mailParameter.Data
            };

            _mailService.SendMail(sendMailDto);
            return new SuccessResult(Messages.MailSendSuccess);
        }

        [SecuredOperations("AccountReconciliation.Update,Admin")]
        [PerformanceAspect(3)]
        [CacheRemoveAspect("IAccountRecanciliationService.Get")]
        public IResult Update(AccountRecanciliation accountRecanciliation)
        {
            _accountRecanciliationDal.Update(accountRecanciliation);
            return new SuccessResult(Messages.UpdatedAccountRecanciliation);
        }
    }
}
