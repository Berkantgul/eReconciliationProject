using Business.Abstract;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class AccountRecanciliationDetailManager : IAccountRecanciliationDetailService
    {
        private readonly IAccountRecanciliationDetailDal _accountRecanciliationDetailDal;

        public AccountRecanciliationDetailManager(IAccountRecanciliationDetailDal accountRecanciliationDetailDal)
        {
            _accountRecanciliationDetailDal = accountRecanciliationDetailDal;
        }
    }
}
