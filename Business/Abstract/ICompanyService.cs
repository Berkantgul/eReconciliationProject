using Core.Entities;
using Core.Utilities.Results.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Business.Abstract
{
    public interface ICompanyService
    {
        IResult Add(Company company);
        IDataResult<List<Company>> GetAll();
        IResult CompanyExists(Company company);
        IResult UserCompanyAdd(int userId, int companyId);
        IDataResult<UserCompany> GetCompany(int userId);
        IResult AddCompanyAndUser(CompanyDto companyDto);
        IResult Update(Company company);
        IDataResult<Company> GetById(int id);
        IDataResult<List<Company>> GetCompanyByUserId(int userId);
    }
}
