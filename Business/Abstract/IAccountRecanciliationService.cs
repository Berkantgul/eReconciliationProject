using Core.Utilities.Results.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IAccountRecanciliationService
    {
        IResult Add(AccountRecanciliation accountRecanciliation);
        IDataResult<List<AccountRecanciliation>> GetList(int companyId);
        IDataResult<AccountRecanciliation> GetById(int id);
        IResult Update(AccountRecanciliation accountRecanciliation);
        IResult Delete(AccountRecanciliation accountRecanciliation);
        IResult AddToExcel(string filePath, int companyId);
        IDataResult<AccountRecanciliation> GetByCode(string code);
        IDataResult<List<AccountRecanciliationDto>> GetListDto(int companyId);
        IResult SendReconciliationMail(AccountRecanciliationDto accountRecanciliationDto);

    }
}
