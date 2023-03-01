using Core.Utilities.Results.Abstract;
using Entities.Concrete;
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
        IDataResult<AccountRecanciliation> Get(int id);
        IResult Update(AccountRecanciliation accountRecanciliation);
        IResult Delete(AccountRecanciliation accountRecanciliation);
        IResult AddToExcel(string filePath, int companyId);
        
    }
}
