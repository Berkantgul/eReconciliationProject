using Core.Entities;
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
    public interface IUserService
    {
        List<OperationClaim> GetClaims(User user, int companyId);
        void Add(User user);
        User GetByMail(string email);
        void Update(User user);
        User GetByMailConfirmValue(string value);
        User GetById(int id);
        IDataResult<User> GetByIdToResult(int id);
        IResult UpdateResult(UserForRegisterToSecondAccountDto userForRegister);
        IResult UpdateOperationClaim(OperationClaimListForUserDto operationClaim);
        IDataResult<List<UserCompanyForListDto>> GetUserList(int companyId);
        IDataResult<List<OperationClaimListForUserDto>> GetOperationClaimListForUser(string value, int companyId);
        IResult UserCompanyDelete(int userId, int companyId);
        IDataResult<List<AdminCompaniesForUserDto>> GetAdminCompaniesForUser(int adminUserId, int userUserId);
        IResult AddUserCompany(int userId, int companyId);
        IResult UserDelete(int userId);
        IDataResult<List<Company>> GetUserCompanyList(string value);
    }
}
