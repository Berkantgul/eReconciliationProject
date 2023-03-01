using Core.Utilities.Results.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IBaBsRecanciliationService
    {
        IResult Add(BaBsRecanciliation baBsRecanciliation);
        IDataResult<List<BaBsRecanciliation>> GetList(int companyId);
        IDataResult<BaBsRecanciliation> Get(int id);
        IResult Update(BaBsRecanciliation baBsRecanciliation);
        IResult Delete(BaBsRecanciliation baBsRecanciliation);
        IResult AddToExcel(string filePath, int companyId);
    }
}
