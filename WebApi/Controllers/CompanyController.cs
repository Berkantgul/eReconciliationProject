using Business.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;

        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpGet("getcompanylist")]
        public IActionResult GetCompanyList()
        {
            var result = _companyService.GetAll();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }
        [HttpPost("addcompany")]
        public IActionResult AddCompany(Company company)
        {
            var result = _companyService.Add(company);
            if (result.Success)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }

        [HttpPost("addCompanyAndUser")]
        public IActionResult AddCompanyAndUser(CompanyDto companyDto)
        {
            var result = _companyService.AddCompanyAndUser(companyDto);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpPost("companyChangeStatus")]
        public IActionResult CompanyChangeStatus(Company company)
        {
            if (company.IsActive)
            {
                company.IsActive = false;
            }
            else
            {
                company.IsActive = true;
            }
            var result = _companyService.Update(company);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpGet("getById")]
        public IActionResult GetById(int companyId)
        {
            var result = _companyService.GetById(companyId);
            if (result.Success)
            {

                return Ok(result);
            }
            return BadRequest(result.Message);
        }
        [HttpPut("updateCompany")]
        public IActionResult UpdateCompany(Company company)
        {
            var result = _companyService.Update(company);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpGet("getCompanyByUserId")]
        public IActionResult GetCompanyByUserId(int userId)
        {
            var result = _companyService.GetCompanyByUserId(userId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

    }
}
