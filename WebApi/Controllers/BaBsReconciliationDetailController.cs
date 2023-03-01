using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaBsReconciliationDetailController : ControllerBase
    {
        private readonly IBaBsRecanciliationDetailService _baBsRecanciliationDetailService;

        public BaBsReconciliationDetailController(IBaBsRecanciliationDetailService baBsRecanciliationDetailService)
        {
            _baBsRecanciliationDetailService = baBsRecanciliationDetailService;
        }

        [HttpGet("getList")]
        public IActionResult GetList(int BaBsRecanciliationId)
        {
            var result = _baBsRecanciliationDetailService.GetList(BaBsRecanciliationId);
            return Ok(result);
        }

        [HttpPost("add")]
        public IActionResult Add(BaBsRecanciliationDetail baBsRecanciliationDetail)
        {
            var result = _baBsRecanciliationDetailService.Add(baBsRecanciliationDetail);
            if (result.Success)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }

        [HttpGet("get")]
        public IActionResult Get(int id)
        {
            return Ok(_baBsRecanciliationDetailService.Get(id));
        }

        [HttpPut("update")]
        public IActionResult Update(BaBsRecanciliationDetail baBsRecanciliationDetail)
        {
            var result = _baBsRecanciliationDetailService.Update(baBsRecanciliationDetail);
            if (result.Success)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }

        [HttpPost("delete")]
        public IActionResult Delete(BaBsRecanciliationDetail baBsRecanciliationDetail)
        {
            var result = _baBsRecanciliationDetailService.Delete(baBsRecanciliationDetail);
            if (result.Success)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }

        [HttpPost("fromAddToExcel")]
        public IActionResult FromAddToExcel(IFormFile file, int baBsReconciliationId)
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
                var result = _baBsRecanciliationDetailService.AddToExcel(filePath, baBsReconciliationId);
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
