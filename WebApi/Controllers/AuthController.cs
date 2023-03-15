using Business.Abstract;
using Core.Utilities.Hashing;
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
        private readonly IForgotPasswordService _forgotPasswordService;

        public AuthController(IAuthService authService, IForgotPasswordService forgotPasswordService)
        {
            _authService = authService;
            _forgotPasswordService = forgotPasswordService;
        }

        [HttpPost("register")]
        public IActionResult Register(UserAndCompanyRegisterDto userAndCompanyRegister)
        {
            var userExists = _authService.UserExists(userAndCompanyRegister.userForRegister.Email);
            if (!userExists.Success)
            {
                return BadRequest(userExists.Message);
            }
            var companyExists = _authService.CompanyExists(userAndCompanyRegister.company);
            if (!companyExists.Success)
            {
                return BadRequest(companyExists.Message);
            }
            //userAndCompanyRegister.company.AddedAt = DateTime.Now;
            var registerResult = _authService.Register(userAndCompanyRegister.userForRegister, userAndCompanyRegister.userForRegister.Password, userAndCompanyRegister.company);
            var result = _authService.CreateAccessToken(registerResult.Data, registerResult.Data.CompanyId);
            if (result.Success)
            {
                return Ok(registerResult);
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
            if (user.MailConfirm)
            {
                return BadRequest("Hesabınız zaten onaylı, giriş yapabilirsiniz!");
            }
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
        public IActionResult SendConfirmEmail(string email)
        {
            var user = _authService.GetByEmail(email).Data;
            if (user == null)
            {
                return BadRequest("Kullanıcı bulunamadı!");
            }

            if (user.MailConfirm)
            {
                return BadRequest("Bu hesap zaten onaylı!");
            }
            var result = _authService.SendConfirmEmail(user);

            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpGet("forgotPassword")]
        public IActionResult ForgotPassword(string email)
        {
            // emaili gönderilen kullanıcı bulunuyor
            var user = _authService.GetByEmail(email).Data;
            if (user == null)
            {
                return BadRequest("Kullanıcı bulunamadı.");
            }

            // bulunan kullanıcının forgotpassword verileri bulunuyor
            var lists = _forgotPasswordService.GetListByUserId(user.Id).Data;
            foreach (var item in lists)
            {
                item.IsActive = false;
                _forgotPasswordService.Update(item);
            }


            var forgotPassword = _forgotPasswordService.CreateForgotPassword(user).Data;
            var result = _authService.SendForgotPasswordEmail(user, forgotPassword.Value);

            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpGet("forgotPasswordLinkCheck")]
        public IActionResult ForgotPasswordLinkCheck(string value)
        {
            var result = _forgotPasswordService.GetForgotPassword(value);
            if (result == null)
            {
                return BadRequest("Tıkladığınız link geçersiz!");
            }

            if (result.IsActive == true)
            {
                DateTime datetime1 = DateTime.Now.AddHours(-1);
                DateTime dateTime2 = DateTime.Now;
                if (result.SendDate >= datetime1 && result.SendDate <= dateTime2)
                {
                    return Ok(true);
                }
                else
                {
                    return BadRequest("Tıkladığınız link geçersiz!");
                }

            }

            else
            {
                return BadRequest("Tıkladığınız link geçersiz!");
            }
        }

        [HttpPost("changePasswordToForgotPassword")]
        public IActionResult ChangePasswordToForgotPassword(ChangePasswordDto changePassword)
        {
            var forgotPasswordResult = _forgotPasswordService.GetForgotPassword(changePassword.Value);
            forgotPasswordResult.IsActive = false;
            _forgotPasswordService.Update(forgotPasswordResult);

            var userResult = _authService.GetById(forgotPasswordResult.UserId).Data;
            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(changePassword.Password, out passwordHash, out passwordSalt);
            userResult.PasswordHash = passwordHash;
            userResult.PasswordSalt = passwordSalt;

            var result = _authService.ChangePassword(userResult);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }
    }
}
