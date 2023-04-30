using Business.Abstract;
using Business.BusinessAspects;
using Business.Contans;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Performance;
using Core.Aspects.Autofac.Validation;
using Core.Entities;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class CompanyManager : ICompanyService
    {
        private readonly ICompanyDal _companyDal;

        public CompanyManager(ICompanyDal companyDal)
        {
            _companyDal = companyDal;
        }

        [CacheRemoveAspect("ICompanyService.Get")]
        [ValidationAspect(typeof(CompanyValidator))]
        public IResult Add(Company company)
        {
            _companyDal.Add(company);
            return new SuccessResult(Messages.AddedCompany);
        }

        [CacheRemoveAspect("ICompanyService.Get")]
        public IResult AddCompanyAndUser(CompanyDto companyDto)
        {
            Company company = new Company()
            {
                Id = companyDto.Id,
                Name = companyDto.Name,
                TaxDepartment = companyDto.TaxDepartment,
                TaxIdNumber = companyDto.TaxIdNumber,
                IdentityNumber = companyDto.IdentityNumber,
                Address = companyDto.Address,
                AddedAt = companyDto.AddedAt,
                IsActive = companyDto.IsActive
            };

            _companyDal.Add(company);
            _companyDal.UserCompanyAdd(companyDto.userId, company.Id);
            return new SuccessResult(Messages.AddedCompany);
        }

        public IResult CompanyExists(Company company)
        {
            var result = _companyDal.Get(i => i.Name == company.Name && i.IdentityNumber == company.IdentityNumber && i.TaxDepartment == company.TaxDepartment && i.TaxIdNumber == company.TaxIdNumber);
            if (result != null)
            {
                return new ErrorResult(Messages.CompanyAlreadyExists);
            }
            return new SuccessResult();
        }

        [CacheAspect(60)]
        public IDataResult<List<Company>> GetAll()
        {
            return new SuccessDataResult<List<Company>>(_companyDal.GetAll(), "Başarılı");
        }

        [CacheAspect(60)]
        public IDataResult<Company> GetById(int id)
        {
            return new SuccessDataResult<Company>(_companyDal.Get(i => i.Id == id));
        }

        [CacheAspect(60)]
        public IDataResult<UserCompany> GetCompany(int userId)
        {
            return new SuccessDataResult<UserCompany>(_companyDal.GetCompany(userId));
        }

        [CacheAspect(60)]
        public IDataResult<List<Company>> GetCompanyByUserId(int userId)
        {
            return new SuccessDataResult<List<Company>>(_companyDal.GetCompanyByUserId(userId));
        }

        [PerformanceAspect(3)]
        [SecuredOperations("Company.Add,Admin")]
        [CacheRemoveAspect("ICompanyService.Get")]
        public IResult Update(Company company)
        {
            _companyDal.Update(company);
            return new SuccessResult(Messages.UpdateCompany);
        }

        [CacheRemoveAspect("ICompanyService.Get")]
        public IResult UserCompanyAdd(int userId, int companyId)
        {
            _companyDal.UserCompanyAdd(userId, companyId);
            return new SuccessResult();
        }
    
    
    }
}
