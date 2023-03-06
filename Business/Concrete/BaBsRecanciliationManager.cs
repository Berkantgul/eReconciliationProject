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
    public class BaBsRecanciliationManager : IBaBsRecanciliationService
    {
        private readonly IBaBsRecanciliationDal _baBsRecanciliationDal;
        private readonly ICurrencyAccountService _currencyAccountService;
        private readonly IMailService _mailService;
        private readonly IMailTemplateService _mailTemplateService;
        private readonly IMailParameterService _mailParameterService;

        public BaBsRecanciliationManager(IBaBsRecanciliationDal baBsRecanciliationDal, ICurrencyAccountService currencyAccountService, IMailService mailService, IMailTemplateService mailTemplateService, IMailParameterService mailParameterService)
        {
            _baBsRecanciliationDal = baBsRecanciliationDal;
            _currencyAccountService = currencyAccountService;
            _mailService = mailService;
            _mailTemplateService = mailTemplateService;
            _mailParameterService = mailParameterService;
        }

        [SecuredOperations("BaBsRecanciliation.Add,Admin")]
        [PerformanceAspect(3)]
        [CacheRemoveAspect("IBaBsRecanciliationService.Get")]
        public IResult Add(BaBsRecanciliation baBsRecanciliation)
        {
            _baBsRecanciliationDal.Add(baBsRecanciliation);
            return new SuccessResult(Messages.AddedBaBsRecanciliation);
        }

        [SecuredOperations("BaBsRecanciliation.Add,Admin")]
        [PerformanceAspect(3)]
        [CacheRemoveAspect("IBaBsRecanciliationService.Get")]
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
                            string type = reader.GetString(1);
                            double month = reader.GetDouble(2);
                            double year = reader.GetDouble(3);
                            double quantity = reader.GetDouble(4);
                            double total = reader.GetDouble(5);
                            int currencyAccountId = _currencyAccountService.GetCode(code, companyId).Data.Id;

                            BaBsRecanciliation baBsRecanciliation = new BaBsRecanciliation
                            {
                                CurrencyAccountId = currencyAccountId,
                                CompanyId = companyId,
                                Mounth = Convert.ToInt16(month),
                                Year = Convert.ToInt16(year),
                                Quantity = Convert.ToInt16(quantity),
                                Total = Convert.ToInt16(total),
                                Type = type
                            };

                            _baBsRecanciliationDal.Add(baBsRecanciliation);
                        }
                    }
                }

            }
            return new SuccessResult(Messages.FromExcelAddToBaBsReconciliaiton);
        }

        [PerformanceAspect(3)]
        [SecuredOperations("BaBsRecanciliation.Delete,Admin")]
        [CacheRemoveAspect("IBaBsRecanciliationService.Get")]
        public IResult Delete(BaBsRecanciliation baBsRecanciliation)
        {
            _baBsRecanciliationDal.Delete(baBsRecanciliation);
            return new SuccessResult(Messages.DeletedBaBsRecanciliation);
        }

        [PerformanceAspect(3)]
        [SecuredOperations("BaBsRecanciliation.Get,Admin")]
        [CacheAspect(60)]
        public IDataResult<BaBsRecanciliation> Get(int id)
        {
            return new SuccessDataResult<BaBsRecanciliation>(_baBsRecanciliationDal.Get(i => i.Id == id));
        }

        [PerformanceAspect(3)]
        [CacheAspect(60)]
        public IDataResult<BaBsRecanciliation> GetByCode(string code)
        {
            return new SuccessDataResult<BaBsRecanciliation>(_baBsRecanciliationDal.Get(p => p.Guid == code));
        }

        [SecuredOperations("BaBsRecanciliation.GetList,Admin")]
        [PerformanceAspect(3)]
        [CacheAspect(60)]
        public IDataResult<List<BaBsRecanciliation>> GetList(int companyId)
        {
            return new SuccessDataResult<List<BaBsRecanciliation>>(_baBsRecanciliationDal.GetAll(i => i.CompanyId == companyId));
        }

        [PerformanceAspect(3)]
        [SecuredOperations("BaBsReconciliation.GetList,Admin")]
        [CacheAspect(60)]
        public IDataResult<List<BaBsReconciliationDto>> GetListDto(int companyId)
        {
            return new SuccessDataResult<List<BaBsReconciliationDto>>(_baBsRecanciliationDal.GetAllDto(companyId));
        }

        [PerformanceAspect(3)]
        [SecuredOperations("BaBsReconciliation.SendMail,Admin")]
        public IResult SendReconciliationMail(BaBsReconciliationDto baBsReconciliationDto)
        {
            string subject = "Mutabakat Mail";
            string body = $"Şirket Adımız : {baBsReconciliationDto.CompanyName} <br />" +
                          $"Şirket Vergi Dairesi : {baBsReconciliationDto.CompanyTaxDepartment} <br />" +
                          $"Şirket Vergi Numarası : {baBsReconciliationDto.CompanyTaxIdNumber} - {baBsReconciliationDto.CompanyIdentityNumber} <br/ ><hr />" +
                          $"Sizin Şirket : {baBsReconciliationDto.AccountName} <br />" +
                          $"Sizin Şirket Vergi Dairesi : {baBsReconciliationDto.AccountIdentityNumber} <br />" +
                          $"Sizin şirket Vergi Numarası : {baBsReconciliationDto.AccountTaxIdNumber} - {baBsReconciliationDto.AccountIdentityNumber} <br /><hr />" +
                          $"Ay / Yıl : {baBsReconciliationDto.Mounth} / {baBsReconciliationDto.Year} <br />" +
                          $"Adet : {baBsReconciliationDto.Quantity} <br />" +
                          $"Tutar : {baBsReconciliationDto.Total} {baBsReconciliationDto.CurrencyCode}";

            string link = "https://localhost:7127/api/BaBsReconciliation/GetByCode?code=" + baBsReconciliationDto.Guid;
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
                email = baBsReconciliationDto.AccountEmail,
                subject = subject,
                body = templateBody,
                mailParameter = mailParameter.Data
            };

            _mailService.SendMail(sendMailDto);
            return new SuccessResult(Messages.MailSendSuccess);
        }

        [SecuredOperations("BaBsRecanciliation.Update,Admin")]
        [PerformanceAspect(3)]
        [CacheRemoveAspect("IBaBsRecanciliationService.Get")]
        public IResult Update(BaBsRecanciliation baBsRecanciliation)
        {
            _baBsRecanciliationDal.Update(baBsRecanciliation);
            return new SuccessResult(Messages.UpdatedBaBsRecanciliation);
        }
    }
}
