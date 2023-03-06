using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Context;
using Entities.Concrete;
using Entities.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfAccountRecanciliationDal : EfEntityRepositoryBase<AccountRecanciliation, ContextDb>, IAccountRecanciliationDal
    {
        public List<AccountRecanciliationDto> GetAllDto(int companyId)
        {
            using (var context = new ContextDb())
            {
                var result = from reconciliation in context.AccountRecanciliations
                             join company in context.Companies on reconciliation.CompanyId equals company.Id
                             join account in context.CurrencyAccounts on reconciliation.CurrencyAccountId equals account.Id
                             join currency in context.Currencies on reconciliation.CurrencyId equals currency.Id
                             select new AccountRecanciliationDto
                             {
                                 CompanyId = companyId,
                                 CurrencyAccountId = account.Id,
                                 AccountIdentityNumber = account.IdentityNumber,
                                 AccountName = account.Name,
                                 AccountTaxDepartment = account.TaxDepartment,
                                 AccountTaxIdNumber = account.IdentityNumber,
                                 CompanyIdentityNumber = company.IdentityNumber,
                                 CompanyName = company.Name,
                                 CompanyTaxDepartment = company.TaxDepartment,
                                 CompanyTaxIdNumber = company.TaxIdNumber,
                                 CurrencyCredit = reconciliation.CurrencyCredit,
                                 CurrencyDebit = reconciliation.CurrencyDebit,
                                 CurrencyId = reconciliation.CurrencyId,
                                 EmailReadDate = reconciliation.EmailReadDate,
                                 EndingDate = reconciliation.EndingDate,
                                 Guid = reconciliation.Guid,
                                 Id = reconciliation.Id,
                                 IsEmailRead = reconciliation.IsEmailRead,
                                 IsResultSucceed = reconciliation.IsResultSucceed,
                                 IsSendEmail = reconciliation.IsSendEmail,
                                 ResultDate = reconciliation.ResultDate,
                                 ResultNote = reconciliation.ResultNote,
                                 SendEmailDate = reconciliation.SendEmailDate,
                                 StartingDate = reconciliation.StartingDate,
                                 CurrencyCode = currency.Code,
                                 AccountEmail = account.Email
                             };

                return result.ToList();
            }
        }
    }
}
