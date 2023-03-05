using Business.Abstract;
using Business.BusinessAspects;
using Business.Contans;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Performance;
using Core.Aspects.Autofac.Transaction;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using ExcelDataReader;
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
        
        [PerformanceAspect(3)]
        [SecuredOperations("BaBsRecanciliationDetail.Add,Admin")]
        [CacheRemoveAspect("IBaBsRecanciliationDetailService.Get")]
        public IResult Add(BaBsRecanciliationDetail baBsRecanciliationDetail)
        {
            _baBsRecanciliationDetailDal.Add(baBsRecanciliationDetail);
            return new SuccessResult(Messages.AddedBaBsReconciliatonDetail);
        }

        [PerformanceAspect(3)]
        [SecuredOperations("BaBsRecanciliationDetail.Add,Admin")]
        [CacheRemoveAspect("IBaBsRecanciliationDetailService.Get")]
        [TransactionScopeAspect]
        public IResult AddToExcel(string filePath, int BaBsRecanciliationId)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    while (reader.Read())
                    {
                        string description = reader.GetString(1);


                        if (description != "Açıklama" && description != null)
                        {
                            DateTime date = reader.GetDateTime(0);
                            double amount = reader.GetDouble(2);

                            BaBsRecanciliationDetail baBsRecanciliationDetail = new BaBsRecanciliationDetail
                            {
                                BaBsRecanciliationId = BaBsRecanciliationId,
                                Description = description,
                                Amount = Convert.ToInt16(amount)
                            };

                            _baBsRecanciliationDetailDal.Add(baBsRecanciliationDetail);
                        }
                    }
                }
                File.Delete(filePath);
            }
            return new SuccessResult(Messages.FromExcelAddToBaBsReconciliationDetail);
        }

        [SecuredOperations("BaBsRecanciliationDetail.Delete,Admin")]
        [PerformanceAspect(3)]
        [CacheRemoveAspect("IBaBsRecanciliationDetailService.Get")]
        public IResult Delete(BaBsRecanciliationDetail BaBsRecanciliationDetail)
        {
            _baBsRecanciliationDetailDal.Delete(BaBsRecanciliationDetail);
            return new SuccessResult(Messages.DeletedBaBsRecanciliationDetail);
        }

        [PerformanceAspect(3)]
        [SecuredOperations("BaBsRecanciliationDetail.Get,Admin")]
        [CacheAspect(60)]
        public IDataResult<BaBsRecanciliationDetail> Get(int id)
        {
            return new SuccessDataResult<BaBsRecanciliationDetail>(_baBsRecanciliationDetailDal.Get(i => i.Id == id));
        }

        [SecuredOperations("BaBsRecanciliationDetail.GetList,Admin")]
        [PerformanceAspect(3)]
        [CacheAspect(60)]
        public IDataResult<List<BaBsRecanciliationDetail>> GetList(int BaBsRecanciliationId)
        {
            return new SuccessDataResult<List<BaBsRecanciliationDetail>>(_baBsRecanciliationDetailDal.GetAll(i => i.BaBsRecanciliationId == BaBsRecanciliationId));
        }

        [PerformanceAspect(3)]
        [SecuredOperations("BaBsRecanciliationDetail.Update,Admin")]
        [CacheRemoveAspect("IBaBsRecanciliationDetailService.Get")]
        public IResult Update(BaBsRecanciliationDetail baBsRecanciliationDetail)
        {
            _baBsRecanciliationDetailDal.Update(baBsRecanciliationDetail);
            return new SuccessResult(Messages.UpdatedBaBsRecanciliationDetail);
        }
    }
}
