using Core.Utilities.Results.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IAccountRecanciliationDetailService
    {
        IResult Add(AccountRecanciliationDetail accountRecanciliationDetail);
        IDataResult<List<AccountRecanciliationDetail>> GetList(int accountReconciliaitonId);
        IDataResult<AccountRecanciliationDetail> Get(int id);
        IResult Update(AccountRecanciliationDetail accountRecanciliationDetail);
        IResult Delete(AccountRecanciliationDetail accountRecanciliationDetail);
        IResult AddToExcel(string filePath, int accountReconciliaitonId);
    }
}
