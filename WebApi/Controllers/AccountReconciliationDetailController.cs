using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountReconciliationDetailController : ControllerBase
    {
        private readonly IAccountRecanciliationDetailService _accountRecanciliationDetailService;

        public AccountReconciliationDetailController(IAccountRecanciliationDetailService accountRecanciliationDetailService)
        {
            _accountRecanciliationDetailService = accountRecanciliationDetailService;
        }

        [HttpGet("getList")]
        public IActionResult GetList(int accountReconciliationId)
        {
            var result = _accountRecanciliationDetailService.GetList(accountReconciliationId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpPost("add")]
        public IActionResult Add(AccountRecanciliationDetail accountRecanciliationDetail)
        {
            var result = _accountRecanciliationDetailService.Add(accountRecanciliationDetail);
            if (result.Success)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }

        [HttpPost("delete")]
        public IActionResult Delete(AccountRecanciliationDetail accountRecanciliationDetail)
        {
            var result = _accountRecanciliationDetailService.Delete(accountRecanciliationDetail);
            if (result.Success)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }

        [HttpGet("get")]
        public IActionResult Get(int id)
        {
            return Ok(_accountRecanciliationDetailService.Get(id));
        }

        [HttpPut("update")]
        public IActionResult Update(AccountRecanciliationDetail accountRecanciliationDetail)
        {
            var result = _accountRecanciliationDetailService.Update(accountRecanciliationDetail);
            if (result.Success)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);

        }

        [HttpPost("fromAddToExcel")]
        public IActionResult FromAddToExcel(IFormFile file, int accountReconciliationId)
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
                var result = _accountRecanciliationDetailService.AddToExcel(filePath, accountReconciliationId);
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
