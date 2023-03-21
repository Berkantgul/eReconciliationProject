using Business.Abstract;
using Business.Contans;
using Core.Entities;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete;
using DataAccess.Abstract;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class UserOperationClaimManager : IUserOperationClaimService
    {
        private readonly IUserOperationClaimDal _userOperationClaimDal;

        public UserOperationClaimManager(IUserOperationClaimDal userOperationClaimDal)
        {
            _userOperationClaimDal = userOperationClaimDal;
        }

        public IResult Add(UserOperationClaim operationClaim)
        {
            _userOperationClaimDal.Add(operationClaim);
            return new SuccessResult(Messages.AddedUserOperationClaim);
        }

        public IResult Delete(UserOperationClaim operationClaim)
        {
            _userOperationClaimDal.Delete(operationClaim);
            return new SuccessResult(Messages.DeletedUserOperationClaim);
        }

        public IDataResult<UserOperationClaim> Get(int id)
        {
            return new SuccessDataResult<UserOperationClaim>(_userOperationClaimDal.Get(i => i.Id == id));
        }

        public IDataResult<List<UserOperationClaim>> GetList()
        {
            return new SuccessDataResult<List<UserOperationClaim>>(_userOperationClaimDal.GetAll());
        }

        public IDataResult<List<UserOperationClaimDto>> GetListDto(int userId, int companyId)
        {
            return new SuccessDataResult<List<UserOperationClaimDto>>(_userOperationClaimDal.GetListDto(userId, companyId));
        }

        public IResult Update(UserOperationClaim operationClaim)
        {
            _userOperationClaimDal.Update(operationClaim);
            return new SuccessResult(Messages.UpdatedUserOperationClaim);
        }
    }
}
