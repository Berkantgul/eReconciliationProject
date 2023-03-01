using Core.Utilities.Results.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IBaBsRecanciliationDetailService
    {
        IResult Add(BaBsRecanciliationDetail baBsRecanciliationDetail);
        IDataResult<List<BaBsRecanciliationDetail>> GetList(int BaBsRecanciliationId);
        IDataResult<BaBsRecanciliationDetail> Get(int id);
        IResult Update(BaBsRecanciliationDetail baBsRecanciliationDetail);
        IResult Delete(BaBsRecanciliationDetail BaBsRecanciliationDetail);
        IResult AddToExcel(string filePath, int BaBsRecanciliationId);
    }
}
