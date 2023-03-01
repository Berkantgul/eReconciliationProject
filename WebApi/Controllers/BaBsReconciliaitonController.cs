using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaBsReconciliaitonController : ControllerBase
    {
        private readonly IBaBsRecanciliationService _baBsRecanciliationService;

        public BaBsReconciliaitonController(IBaBsRecanciliationService baBsRecanciliationService)
        {
            _baBsRecanciliationService = baBsRecanciliationService;
        }

        [HttpGet("getList")]
        public IActionResult GetList(int companyId)
        {
            var result = _baBsRecanciliationService.GetList(companyId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpPost("add")]
        public IActionResult Add(BaBsRecanciliation baBsRecanciliation)
        {
            var result = _baBsRecanciliationService.Add(baBsRecanciliation);
            if (result.Success)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }

        [HttpPost("delete")]
        public IActionResult Delete(BaBsRecanciliation baBsRecanciliation)
        {
            var result = _baBsRecanciliationService.Delete(baBsRecanciliation);
            if (result.Success)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }

        [HttpGet("get")]
        public IActionResult Get(int id)
        {
            return Ok(_baBsRecanciliationService.Get(id));
        }

        [HttpPut("update")]
        public IActionResult Update(BaBsRecanciliation baBsRecanciliation)
        {
            var result = _baBsRecanciliationService.Update(baBsRecanciliation);
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
                var result = _baBsRecanciliationService.AddToExcel(filePath, companyId);
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
