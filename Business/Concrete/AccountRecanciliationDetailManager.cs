﻿using Business.Abstract;
using Business.Contans;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class AccountRecanciliationDetailManager : IAccountRecanciliationDetailService
    {
        private readonly IAccountRecanciliationDetailDal _accountRecanciliationDetailDal;

        public AccountRecanciliationDetailManager(IAccountRecanciliationDetailDal accountRecanciliationDetailDal)
        {
            _accountRecanciliationDetailDal = accountRecanciliationDetailDal;
        }

        public IResult Add(AccountRecanciliationDetail accountRecanciliationDetail)
        {
            _accountRecanciliationDetailDal.Add(accountRecanciliationDetail);
            return new SuccessResult(Messages.AddedAccountReconciliaitonDetail);
        }

        public IResult AddToExcel(string filePath, int accountReconciliaitonId)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    while (reader.Read())
                    {
                        string description = reader.GetString(1);


                        if (description != "Açıklama" && description != null)
                        {
                            DateTime date = reader.GetDateTime(0);
                            double currencyId = reader.GetDouble(2);
                            double debit = reader.GetDouble(3);
                            double credit = reader.GetDouble(4);

                            AccountRecanciliationDetail accountRecanciliationDetail = new AccountRecanciliationDetail
                            {
                                AccountRecanciliationId = accountReconciliaitonId,
                                Date = date,
                                Description = description,
                                CurrencyDebit = Convert.ToDecimal(debit),
                                CurrencyId = Convert.ToInt16(currencyId),
                                CurrencyCredit = Convert.ToDecimal(credit)
                            };

                            _accountRecanciliationDetailDal.Add(accountRecanciliationDetail);
                        }
                    }
                }
            }
            return new SuccessResult(Messages.FromExcelAddToAccountReconciliationDetail);
        }

        public IResult Delete(AccountRecanciliationDetail accountRecanciliationDetail)
        {
            _accountRecanciliationDetailDal.Delete(accountRecanciliationDetail);
            return new SuccessResult(Messages.DeletedAccountReconciliaitonDetail);
        }

        public IDataResult<AccountRecanciliationDetail> Get(int id)
        {
            return new SuccessDataResult<AccountRecanciliationDetail>(_accountRecanciliationDetailDal.Get(i => i.Id == id));
        }

        public IDataResult<List<AccountRecanciliationDetail>> GetList(int accountReconciliaitonId)
        {
            return new SuccessDataResult<List<AccountRecanciliationDetail>>(_accountRecanciliationDetailDal.GetAll(i => i.AccountRecanciliationId == accountReconciliaitonId));
        }

        public IResult Update(AccountRecanciliationDetail accountRecanciliationDetail)
        {
            _accountRecanciliationDetailDal.Update(accountRecanciliationDetail);
            return new SuccessResult(Messages.UpdateAccountReconciliaitonDetail);
        }
    }
}
