using Core.Entities;
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
    }
}
