using Core.DataAccess.EntityFramework;
using Core.Entities;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Context;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfUserOperationClaimDal : EfEntityRepositoryBase<UserOperationClaim, ContextDb>, IUserOperationClaimDal
    {
        public List<UserOperationClaimDto> GetListDto(int userId, int companyId)
        {
            using (var context = new ContextDb())
            {
                var result = from userOperationClaim in context.UserOperationClaims.Where(p => p.UserId == userId && p.CompanyId == companyId)
                             join operationClaim in context.OperationClaims on userOperationClaim.OperationClaimId equals operationClaim.Id
                             select new UserOperationClaimDto
                             {
                                 UserId = userId,
                                 CompanyId = companyId,
                                 OperationClaimId = operationClaim.Id,
                                 OperationDescription = operationClaim.Description,
                                 OperationName = operationClaim.Name,
                                 Id = operationClaim.Id
                             };
                return result.ToList();
            }
        }
    }
}
