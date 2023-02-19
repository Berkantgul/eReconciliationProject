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
    public class BaBsRecanciliationDetailManager : IBaBsRecanciliationDetailService
    {
        private readonly IBaBsRecanciliationDetailDal _baBsRecanciliationDetailDal;

        public BaBsRecanciliationDetailManager(IBaBsRecanciliationDetailDal baBsRecanciliationDetailDal)
        {
            _baBsRecanciliationDetailDal = baBsRecanciliationDetailDal;
        }
    }
}
