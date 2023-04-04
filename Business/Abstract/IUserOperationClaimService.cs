using Core.Entities;
using Core.Utilities.Results.Abstract;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IUserOperationClaimService
    {
        IResult Update(UserOperationClaim operationClaim);
        IDataResult<List<UserOperationClaim>> GetList(int userId,int companyId);
        IDataResult<UserOperationClaim> Get(int id);
        IResult Add(UserOperationClaim operationClaim);
        IDataResult<List<UserOperationClaimDto>> GetListDto(int userId, int companyId);
        IResult Delete(UserOperationClaim operationClaim);
    }
}
