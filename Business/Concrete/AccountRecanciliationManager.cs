using Business.Abstract;
using Business.Contans;
using Core.Aspects.Autofac.Transaction;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
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

        public AccountRecanciliationManager(IAccountRecanciliationDal accountRecanciliationDal, ICurrencyAccountService currencyAccountService)
        {
            _accountRecanciliationDal = accountRecanciliationDal;
            _currencyAccountService = currencyAccountService;
        }

        public IResult Add(AccountRecanciliation accountRecanciliation)
        {
            _accountRecanciliationDal.Add(accountRecanciliation);
            return new SuccessResult(Messages.AddedAccountRecanciliation);
        }
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
                                EmailReadDate = DateTime.Now

                            };

                            _accountRecanciliationDal.Add(accountRecanciliation);
                        }
                    }
                }
            }
            return new SuccessResult(Messages.FromExcelAddToAccountReconciliaiton);
        }

        public IResult Delete(AccountRecanciliation accountRecanciliation)
        {
            _accountRecanciliationDal.Delete(accountRecanciliation);
            return new SuccessResult(Messages.DeletedAccountRecanciliation);
        }

        public IDataResult<AccountRecanciliation> Get(int id)
        {
            return new SuccessDataResult<AccountRecanciliation>(_accountRecanciliationDal.Get(i => i.Id == id));
        }

        public IDataResult<List<AccountRecanciliation>> GetList(int companyId)
        {
            return new SuccessDataResult<List<AccountRecanciliation>>(_accountRecanciliationDal.GetAll(i => i.CompanyId == companyId));
        }

        public IResult Update(AccountRecanciliation accountRecanciliation)
        {
            _accountRecanciliationDal.Update(accountRecanciliation);
            return new SuccessResult(Messages.UpdatedAccountRecanciliation);
        }
    }
}
