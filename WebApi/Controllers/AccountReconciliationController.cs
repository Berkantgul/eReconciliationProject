﻿using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountReconciliationController : ControllerBase
    {
        private readonly IAccountRecanciliationService _accountRecanciliationService;

        public AccountReconciliationController(IAccountRecanciliationService accountRecanciliationService)
        {
            _accountRecanciliationService = accountRecanciliationService;
        }

        [HttpGet("getList")]
        public IActionResult GetList(int companyId)
        {
            var result = _accountRecanciliationService.GetList(companyId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpPost("add")]
        public IActionResult Add(AccountRecanciliation accountRecanciliation)
        {
            var result = _accountRecanciliationService.Add(accountRecanciliation);
            if (result.Success)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }

        [HttpPost("delete")]
        public IActionResult Delete(AccountRecanciliation accountRecanciliation)
        {
            var result = _accountRecanciliationService.Delete(accountRecanciliation);
            if (result.Success)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }

        [HttpGet("get")]
        public IActionResult Get(int id)
        {
            return Ok(_accountRecanciliationService.Get(id));
        }

        [HttpPut("update")]
        public IActionResult Update(AccountRecanciliation accountRecanciliation)
        {
            var result = _accountRecanciliationService.Update(accountRecanciliation);
            if (result.Success)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);

        }

        [HttpPost("fromAddToExcel")]
        public IActionResult FromAddToExcel(IFormFile file, int companyId)
        {
            if (file.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + ".xlsx";
                var filePath = $"{Directory.GetCurrentDirectory()}/Content/{fileName}";
                using (FileStream stream = System.IO.File.Create(filePath))
                {
                    file.CopyTo(stream);
                    stream.Flush();
                }
                var result = _accountRecanciliationService.AddToExcel(filePath, companyId);
                if (result.Success)
                {
                    return Ok(result.Message);
                }
                return BadRequest(result.Message);
            }
            return BadRequest("Geçerli bir excel yüklemesi gerçekleşmedi.");
        }
    }
}