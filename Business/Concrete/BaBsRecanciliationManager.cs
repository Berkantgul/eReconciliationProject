using Business.Abstract;
using Business.Contans;
using Core.Aspects.Autofac.Caching;
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
    public class BaBsRecanciliationManager : IBaBsRecanciliationService
    {
        private readonly IBaBsRecanciliationDal _baBsRecanciliationDal;
        private readonly ICurrencyAccountService _currencyAccountService;

        public BaBsRecanciliationManager(IBaBsRecanciliationDal baBsRecanciliationDal, ICurrencyAccountService currencyAccountService)
        {
            _baBsRecanciliationDal = baBsRecanciliationDal;
            _currencyAccountService = currencyAccountService;
        }

        [CacheRemoveAspect("IBaBsRecanciliationService.Get")]
        public IResult Add(BaBsRecanciliation baBsRecanciliation)
        {
            _baBsRecanciliationDal.Add(baBsRecanciliation);
            return new SuccessResult(Messages.AddedBaBsRecanciliation);
        }

        [CacheRemoveAspect("IBaBsRecanciliationService.Get")]
        [TransactionScopeAspect]
        public IResult AddToExcel(string filePath, int companyId)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    while (reader.Read())
                    {
                        string code = reader.GetString(0);


                        if (code != "Cari Kodu" && code != null)
                        {
                            string type = reader.GetString(1);
                            double month = reader.GetDouble(2);
                            double year = reader.GetDouble(3);
                            double quantity = reader.GetDouble(4);
                            double total = reader.GetDouble(5);
                            int currencyAccountId = _currencyAccountService.GetCode(code, companyId).Data.Id;

                            BaBsRecanciliation baBsRecanciliation = new BaBsRecanciliation
                            {
                                CurrencyAccountId = currencyAccountId,
                                CompanyId = companyId,
                                Mounth = Convert.ToInt16(month),
                                Year = Convert.ToInt16(year),
                                Quantity = Convert.ToInt16(quantity),
                                Total = Convert.ToInt16(total),
                                Type = type
                            };

                            _baBsRecanciliationDal.Add(baBsRecanciliation);
                        }
                    }
                }

            }
            return new SuccessResult(Messages.FromExcelAddToBaBsReconciliaiton);
        }

        [CacheRemoveAspect("IBaBsRecanciliationService.Get")]
        public IResult Delete(BaBsRecanciliation baBsRecanciliation)
        {
            _baBsRecanciliationDal.Delete(baBsRecanciliation);
            return new SuccessResult(Messages.DeletedBaBsRecanciliation);
        }

        [CacheAspect(60)]
        public IDataResult<BaBsRecanciliation> Get(int id)
        {
            return new SuccessDataResult<BaBsRecanciliation>(_baBsRecanciliationDal.Get(i => i.Id == id));
        }

        [CacheAspect(60)]
        public IDataResult<List<BaBsRecanciliation>> GetList(int companyId)
        {
            return new SuccessDataResult<List<BaBsRecanciliation>>(_baBsRecanciliationDal.GetAll(i => i.CompanyId == companyId));
        }

        [CacheRemoveAspect("IBaBsRecanciliationService.Get")]
        public IResult Update(BaBsRecanciliation baBsRecanciliation)
        {
            _baBsRecanciliationDal.Update(baBsRecanciliation);
            return new SuccessResult(Messages.UpdatedBaBsRecanciliation);
        }
    }
}
