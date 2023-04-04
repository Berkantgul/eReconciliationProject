using Core.DataAccess.EntityFramework;
using Core.Entities;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Context;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfUserDal : EfEntityRepositoryBase<User, ContextDb>, IUserDal
    {
        public List<AdminCompaniesForUserDto> GetAdminCompaniesForUser(int adminUserId, int userUserId)
        {
            using (var context = new ContextDb())
            {
                var result = from userCompany in context.UserCompanies.Where(p => p.UserId == adminUserId)
                             join company in context.Companies on userCompany.CompanyId equals company.Id
                             select new AdminCompaniesForUserDto
                             {
                                 Id = company.Id,
                                 AddedAt = company.AddedAt,
                                 Address = company.Address,
                                 IdentityNumber = company.IdentityNumber,
                                 IsActive = company.IsActive,
                                 IsTrue = (context.UserCompanies.Where(p => p.UserId == userUserId && p.CompanyId == company.Id).Count() > 0 ? true : false),
                                 Name = company.Name,
                                 TaxDepartment = company.TaxDepartment,
                                 TaxIdNumber = company.TaxIdNumber

                             };
                return result.Where(p => p.IsTrue == false).OrderBy(p => p.Name).ToList();
            }
        }

        public List<OperationClaim> GetClaims(User user, int companyId)
        {
            using (var context = new ContextDb())
            {
                var result = from operationClaim in context.OperationClaims
                             join userOperationClaim in context.UserOperationClaims
                             on operationClaim.Id equals userOperationClaim.OperationClaimId
                             where userOperationClaim.CompanyId == companyId && userOperationClaim.UserId == user.Id
                             select new OperationClaim
                             {
                                 Id = operationClaim.Id,
                                 Name = operationClaim.Name,
                                 AddedAt = operationClaim.AddedAt,
                                 IsActive = operationClaim.IsActive,
                             };

                var deneme = result.ToList();
                return result.ToList();
            }
        }

        public List<OperationClaimListForUserDto> GetOperationClaimListForUser(string value, int companyId)
        {
            using (var context = new ContextDb())
            {
                var findUser = context.Users.Where(p => p.MailConfirmValue == value).FirstOrDefault();
                var result = from operationClaim in context.OperationClaims
                             where operationClaim.Name != "Admin" && !operationClaim.Name.Contains("UserOperationClaim")
                             select new OperationClaimListForUserDto
                             {
                                 Id = operationClaim.Id,
                                 Name = operationClaim.Name,
                                 Description = operationClaim.Description,
                                 Status = (context.UserOperationClaims.Where(p => p.UserId == findUser.Id && p.CompanyId == companyId && p.OperationClaimId == operationClaim.Id).Count() > 0 ? true : false),
                                 UserName = findUser.Name,
                                 UserId = findUser.Id,
                                 CompanyId = companyId
                             };
                return result.OrderBy(p => p.Name).ToList();
            }
        }

        public List<Company> GetUserCompanyList(string value)
        {
            using (var context = new ContextDb())
            {
                var user = context.Users.Where(p => p.MailConfirmValue == value).FirstOrDefault();
                var result = from userCompany in context.UserCompanies.Where(p => p.UserId == user.Id)
                             join company in context.Companies on userCompany.CompanyId equals company.Id
                             select new Company
                             {
                                 AddedAt = company.AddedAt,
                                 Id = company.Id,
                                 Name = company.Name,
                                 Address = company.Address,
                                 IdentityNumber = company.IdentityNumber,
                                 IsActive = company.IsActive,
                                 TaxDepartment = company.TaxDepartment,
                                 TaxIdNumber = company.TaxIdNumber,
                             };

                return result.OrderBy(p => p.Name).ToList();
            }
        }

        public List<UserCompanyForListDto> GetUserList(int companyId)
        {
            using (var context = new ContextDb())
            {
                var result = from userCompany in context.UserCompanies.Where(x => x.CompanyId == companyId && x.IsActive == true)
                             join user in context.Users on userCompany.UserId equals user.Id
                             join company in context.Companies on userCompany.CompanyId equals company.Id
                             select new UserCompanyForListDto
                             {
                                 CompanyId = companyId,
                                 Id = userCompany.Id,
                                 Email = user.Email,
                                 UserAddetAt = user.AddedAt,
                                 Name = user.Name,
                                 UserId = userCompany.UserId,
                                 UserIsActive = user.IsActive,
                                 Value = user.MailConfirmValue,
                                 CompanyName = company.Name

                             };
                return result.OrderBy(x => x.Name).ToList();
            }
        }
    }
}
