using Core.DataAccess.EntityFramework;
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
    public class EfUserReletionShipDal : EfEntityRepositoryBase<UserReletionShip, ContextDb>, IUserReletionShipDal
    {
        public UserReletionShipDto GetById(int userUserId)
        {
            using (var context = new ContextDb())
            {
                var result = from userReletionShip in context.UserReletionShips.Where(i => i.UserUserId == userUserId)
                             join adminUser in context.Users on userReletionShip.AdminUserId equals adminUser.Id
                             join userUser in context.Users on userReletionShip.UserUserId equals userUser.Id
                             select new UserReletionShipDto
                             {
                                 AdminUserId = adminUser.Id,
                                 AdminAddedAt = adminUser.AddedAt,
                                 AdminMail = adminUser.Email,
                                 AdminIsActive = adminUser.IsActive,
                                 AdminUserName = adminUser.Name,
                                 UserUserId = userUser.Id,
                                 UserMail = userUser.Email,
                                 UserAddedAt = userUser.AddedAt,
                                 UserIsActive = userUser.IsActive,
                                 UserMailValue = userUser.MailConfirmValue,
                                 UserUserName = userUser.Name,
                                 Companies = (from userCompany in context.UserCompanies.Where(p => p.UserId == userUser.Id)
                                              join user in context.Users on userCompany.UserId equals user.Id
                                              join company in context.Companies on userCompany.CompanyId equals company.Id
                                              select new Company
                                              {
                                                  Id = company.Id,
                                                  Name = company.Name,
                                                  TaxDepartment = company.TaxDepartment,
                                                  AddedAt = company.AddedAt,
                                                  Address = company.Address,
                                                  IdentityNumber = company.IdentityNumber,
                                                  IsActive = company.IsActive,
                                                  TaxIdNumber = company.TaxIdNumber
                                              }).ToList(),
                                 Id = userReletionShip.Id
                             };
                return result.FirstOrDefault();
            }
        }

        public List<UserReletionShipDto> GetListDto(int adminUserId)
        {
            using (var context = new ContextDb())
            {
                var result = from userReletionShip in context.UserReletionShips.Where(i => i.AdminUserId == adminUserId)
                             join adminUser in context.Users on userReletionShip.AdminUserId equals adminUser.Id
                             join userUser in context.Users on userReletionShip.UserUserId equals userUser.Id
                             select new UserReletionShipDto
                             {
                                 AdminUserId = adminUserId,
                                 AdminAddedAt = adminUser.AddedAt,
                                 AdminMail = adminUser.Email,
                                 AdminIsActive = adminUser.IsActive,
                                 AdminUserName = adminUser.Name,
                                 UserUserId = userUser.Id,
                                 UserMail = userUser.Email,
                                 UserAddedAt = userUser.AddedAt,
                                 UserIsActive = userUser.IsActive,
                                 UserMailValue = userUser.MailConfirmValue,
                                 UserUserName = userUser.Name,
                                 Companies = (from userCompany in context.UserCompanies.Where(p => p.UserId == userUser.Id)
                                              join user in context.Users on userCompany.UserId equals user.Id
                                              join company in context.Companies on userCompany.CompanyId equals company.Id
                                              select new Company
                                              {
                                                  Id = company.Id,
                                                  Name = company.Name,
                                                  TaxDepartment = company.TaxDepartment,
                                                  AddedAt = company.AddedAt,
                                                  Address = company.Address,
                                                  IdentityNumber = company.IdentityNumber,
                                                  IsActive = company.IsActive,
                                                  TaxIdNumber = company.TaxIdNumber
                                              }).ToList(),
                                 Id = userReletionShip.Id
                             };
                return result.OrderBy(p=>p.UserUserName).ToList();
            }
        }
    }
}
