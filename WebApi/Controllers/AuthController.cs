using Business.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public IActionResult Register(UserAndCompanyRegisterDto userAndCompanyRegister)
        {
            var userExists = _authService.UserExists(userAndCompanyRegister.userForRegister.Email);
            if (userExists == null)
            {
                return BadRequest(userExists.Message);
            }
            var companyExists = _authService.CompanyExists(userAndCompanyRegister.company);
            if (!companyExists.Success)
            {
                return BadRequest(companyExists.Message);
            }
            var registerResult = _authService.Register(userAndCompanyRegister.userForRegister, userAndCompanyRegister.userForRegister.Password, userAndCompanyRegister.company);
            var result = _authService.CreateAccessToken(registerResult.Data, registerResult.Data.CompanyId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(registerResult.Message);
        }

        [HttpPost("registerSecondAccount")]
        public IActionResult RegisterSecondAccount(UserForRegisterToSecondAccountDto userForRegister)
        {
            var userExists = _authService.UserExists(userForRegister.Email);
            if (userExists == null)
            {
                return BadRequest(userExists.Message);
            }
            var registerResult = _authService.RegisterSecondAccount(userForRegister, userForRegister.Password, userForRegister.CompanyId);
            var result = _authService.CreateAccessToken(registerResult.Data, userForRegister.CompanyId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(registerResult.Message);
        }

        [HttpPost("login")]
        public IActionResult Login(UserForLogin userForLogin)
        {
            var userToLogin = _authService.Login(userForLogin);
            if (!userToLogin.Success)
            {
                return BadRequest(userToLogin.Message);
            }
            if (userToLogin.Data.IsActive)
            {
                if (userToLogin.Data.MailConfirm)
                {
                    var userCompany = _authService.GetCompany(userToLogin.Data.Id).Data;
                    var result = _authService.CreateAccessToken(userToLogin.Data, userCompany.CompanyId);
                    if (result.Success)
                    {
                        return Ok(result);
                    }
                    return BadRequest(result);
                }
                return BadRequest("Gelen onay mailini cevaplamalısınız. Mail adresinizi onaylamadan sisteme giriş yapamazsınız!");
            }
            return BadRequest("Kullanıcınız pasif durumda, aktif etmek için yöneticinizle danışın.");
        }

        [HttpGet("confirmuser")]
        public IActionResult ConfirmUser(string value)
        {
            var user = _authService.GetByMailConfirmValue(value).Data;
            user.MailConfirm = true;
            user.MailConfirmDate = DateTime.Now;
            var result = _authService.Update(user);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpGet("sendconfirmemail")]
        public IActionResult SendConfirmEmail(int id)
        {
            var user = _authService.GetById(id).Data;
            var result = _authService.SendConfirmEmail(user);

            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

    }
}
