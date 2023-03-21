using Business.Abstract;
using Business.BusinessAspects;
using Business.Contans;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Performance;
using Core.Entities;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete;
using DataAccess.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class OperationClaimManager : IOperationClaimService
    {
        private readonly IOperationClaimDal _operatinClaimDal;

        public OperationClaimManager(IOperationClaimDal operatinClaimDal)
        {
            _operatinClaimDal = operatinClaimDal;
        }

        [PerformanceAspect(3)]
        [CacheRemoveAspect("IOperationClaimService.Get")]
        [SecuredOperations("OperationClaim.Add")]
        public IResult Add(OperationClaim operationClaim)
        {
            _operatinClaimDal.Add(operationClaim);
            return new SuccessResult(Messages.AddedOperationClaim);
        }

        [PerformanceAspect(3)]
        [CacheRemoveAspect("IOperationClaimService.Get")]
        [SecuredOperations("OperationClaim.Delete")]
        public IResult Delete(OperationClaim operationClaim)
        {
            _operatinClaimDal.Delete(operationClaim);
            return new SuccessResult(Messages.DeletedOperationClaim);
        }

        [PerformanceAspect(3)]
        [SecuredOperations("OperationClaim.Get")]
        [CacheAspect(60)]
        public IDataResult<OperationClaim> Get(int id)
        {
            return new SuccessDataResult<OperationClaim>(_operatinClaimDal.Get(i => i.Id == id));
        }

       // [SecuredOperations("OperationClaim.GetList")]
        [PerformanceAspect(3)]
        [CacheAspect(60)]
        public IDataResult<List<OperationClaim>> GetList()
        {
            return new SuccessDataResult<List<OperationClaim>>(_operatinClaimDal.GetAll());
        }

        [CacheRemoveAspect("IOperationClaimService.Get")]
        [PerformanceAspect(3)]
        [SecuredOperations("OperationClaim.Update")]
        public IResult Update(OperationClaim operationClaim)
        {
            _operatinClaimDal.Update(operationClaim);
            return new SuccessResult(Messages.UpdateOperationClaim);
        }
    }
}
