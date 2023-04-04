using Business.Abstract;
using Business.BusinessAspects;
using Business.Contans;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Performance;
using Core.Aspects.Autofac.Transaction;
using Core.Aspects.Autofac.Validation;
using Core.Entities;
using Core.Utilities.Hashing;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Business.Concrete
{

    public class UserManager : IUserService
    {
        private readonly IUserDal _userDal;
        private readonly IUserOperationClaimService _userOperationClaimService;
        private readonly IUserCompanyService _userCompanyService;
        private readonly ICompanyService _companyService;
        private readonly IOperationClaimService _operationClaimService;
        private readonly IUserReletionShipService _userReletionShipService;

        public UserManager(IUserDal userDal, IUserOperationClaimService userOperationClaimService, IUserCompanyService userCompanyService, ICompanyService companyService, IOperationClaimService operationClaimService, IUserReletionShipService userReletionShipService)
        {
            _userDal = userDal;
            _userOperationClaimService = userOperationClaimService;
            _userCompanyService = userCompanyService;
            _companyService = companyService;
            _operationClaimService = operationClaimService;
            _userReletionShipService = userReletionShipService;
        }

        [CacheRemoveAspect("IUserService.Get")]
        // bu dependecy çözmem gerekiyor.
        [ValidationAspect(typeof(UserValidator))]
        public void Add(User user)
        {
            _userDal.Add(user);
        }

        [TransactionScopeAspect]
        public IResult AddUserCompany(int userId, int companyId)
        {
            _companyService.UserCompanyAdd(userId, companyId);

            var operationClaims = _operationClaimService.GetList().Data;
            foreach (var operationClaim in operationClaims)
            {
                if (operationClaim.Name != "Admin" && operationClaim.Name != "MailParameter" && operationClaim.Name != "MailTemplate")
                {
                    UserOperationClaim addUserOperationClaim = new UserOperationClaim()
                    {
                        CompanyId = companyId,
                        AddedAt = DateTime.Now,
                        IsActive = true,
                        OperationClaimId = operationClaim.Id,
                        UserId = userId
                    };
                    _userOperationClaimService.Add(addUserOperationClaim);
                }

            }

            return new SuccessResult(Messages.AddedUserCompany);

        }

        public IDataResult<List<AdminCompaniesForUserDto>> GetAdminCompaniesForUser(int adminUserId, int userUserId)
        {
            return new SuccessDataResult<List<AdminCompaniesForUserDto>>(_userDal.GetAdminCompaniesForUser(adminUserId, userUserId));
        }

        [CacheAspect(60)]
        public User GetById(int id)
        {
            return _userDal.Get(i => i.Id == id);
        }

        [CacheAspect(60)]
        public IDataResult<User> GetByIdToResult(int id)
        {
            return new SuccessDataResult<User>(_userDal.Get(i => i.Id == id));
        }

        [CacheAspect(60)]
        public User GetByMail(string email)
        {
            return _userDal.Get(i => i.Email == email);
        }

        public User GetByMailConfirmValue(string value)
        {
            return _userDal.Get(i => i.MailConfirmValue == value);
        }

        public List<OperationClaim> GetClaims(User user, int companyId)
        {
            return _userDal.GetClaims(user, companyId);
        }

        public IDataResult<List<OperationClaimListForUserDto>> GetOperationClaimListForUser(string value, int companyId)
        {
            return new SuccessDataResult<List<OperationClaimListForUserDto>>(_userDal.GetOperationClaimListForUser(value, companyId));
        }

        public IDataResult<List<Company>> GetUserCompanyList(string value)
        {
            return new SuccessDataResult<List<Company>>(_userDal.GetUserCompanyList(value));
        }

        [PerformanceAspect(3)]
        [SecuredOperations("User.GetList,Admin")]
        public IDataResult<List<UserCompanyForListDto>> GetUserList(int companyId)
        {
            return new SuccessDataResult<List<UserCompanyForListDto>>(_userDal.GetUserList(companyId));
        }

        [CacheRemoveAspect("IUserService.Get")]
        public void Update(User user)
        {
            _userDal.Update(user);
        }

        public IResult UpdateOperationClaim(OperationClaimListForUserDto operationClaim)
        {
            if (operationClaim.Status == true)
            {
                var result = _userOperationClaimService.GetList(operationClaim.UserId, operationClaim.CompanyId).Data.FirstOrDefault();
                _userOperationClaimService.Delete(result);
            }
            else
            {
                UserOperationClaim userOperationClaim = new UserOperationClaim
                {
                    CompanyId = operationClaim.CompanyId,
                    IsActive = true,
                    UserId = operationClaim.UserId,
                    AddedAt = DateTime.Now,
                    OperationClaimId = operationClaim.Id,
                };
                _userOperationClaimService.Add(userOperationClaim);
            }
            return new SuccessResult(Messages.UpdatedUserOperationClaim);
        }

        [CacheRemoveAspect("IUserService.Get")]
        public IResult UpdateResult(UserForRegisterToSecondAccountDto userForRegister)
        {
            var findUser = _userDal.Get(i => i.Id == userForRegister.Id);
            findUser.Email = userForRegister.Email;
            findUser.Name = userForRegister.Name;
            if (userForRegister.Password != "")
            {
                byte[] passworhHash, passwordSalt;
                HashingHelper.CreatePasswordHash(userForRegister.Password, out passworhHash, out passwordSalt);
                findUser.PasswordSalt = passwordSalt;
                findUser.PasswordHash = passworhHash;
            }
            _userDal.Update(findUser);
            return new SuccessResult(Messages.UpdatedUser);
        }

        [TransactionScopeAspect]
        public IResult UserCompanyDelete(int userId, int companyId)
        {
            var userOperationClaims = _userOperationClaimService.GetList(userId, companyId).Data;
            foreach (var userOperationClaim in userOperationClaims)
            {
                _userOperationClaimService.Delete(userOperationClaim);

            }

            var result = _userCompanyService.GetByUserIdAndCompanyId(userId, companyId);
            _userCompanyService.Delete(result);
            return new SuccessResult(Messages.DeleteUserCompany);
        }

        [TransactionScopeAspect]
        public IResult UserDelete(int userId)
        {
            var userCompanies = _userCompanyService.GetList(userId);
            foreach (var userCompany in userCompanies)
            {
                var userOperationClaims = _userOperationClaimService.GetList(userId, userCompany.CompanyId).Data;
                foreach (var userOperationClaim in userOperationClaims)
                {
                    _userOperationClaimService.Delete(userOperationClaim);

                }

                var result = _userCompanyService.GetByUserIdAndCompanyId(userId, userCompany.CompanyId);
                _userCompanyService.Delete(result);
            }

            var userReletions = _userReletionShipService.GetList(userId);
            foreach (var userReletion in userReletions)
            {
                _userReletionShipService.Delete(userReletion);
            }

            var user = _userDal.Get(i => i.Id == userId);
            _userDal.Delete(user);

            return new SuccessResult(Messages.UserDelete);
        }
    }
}
