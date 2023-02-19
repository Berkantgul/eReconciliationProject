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
    public class BaBsRecanciliationManager : IBaBsRecanciliationService
    {
        private readonly IBaBsRecanciliationDal _baBsRecanciliationDal;

        public BaBsRecanciliationManager(IBaBsRecanciliationDal baBsRecanciliationDal)
        {
            _baBsRecanciliationDal = baBsRecanciliationDal;
        }
    }
}
