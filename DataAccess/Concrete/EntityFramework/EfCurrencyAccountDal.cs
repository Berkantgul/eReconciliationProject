using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Context;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfCurrencyAccountDal : EfEntityRepositoryBase<CurrencyAccount, ContextDb>, ICurrencyAccountDal
    {
        public bool CheckCurrencyAccountReconciliation(int currencyAccountId)
        {
            using (var context = new ContextDb())
            {
                var accountReconciliation = context.AccountRecanciliations.Where(i => i.CurrencyAccountId == currencyAccountId).ToList();
                if (accountReconciliation.Count() > 0)
                {
                    return false;
                }
                var baBsReconciliation = context.BaBsRecanciliations.Where(i => i.CurrencyAccountId == currencyAccountId).ToList();
                if (baBsReconciliation.Count() > 0)
                {
                    return false;
                }
                return true;
            }
        }
    }
}
