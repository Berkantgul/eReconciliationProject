﻿using Business.Abstract;
using Business.Contans;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Transaction;
using Core.CrossCuttingConcerns.Validation;
using Core.Entities;
using Core.Utilities.Hashing;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete;
using Core.Utilities.Security.JWT;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Business.Concrete
{
    public class AuthManager : IAuthService
    {
        private readonly IUserService _userService;
        private readonly ITokenHelper _tokenHelper;
        private readonly ICompanyService _companyService;
        private readonly IMailService _mailService;
        private readonly IMailParameterService _mailParameterService;
        private readonly IMailTemplateService _mailTemplateService;
        private readonly IUserOperationClaimService _userOperationClaimService;
        private readonly IOperationClaimService _operationClaimService;
        private readonly IUserReletionShipService _userReletionShipService;
        private readonly IUserThemeOptionService _userThemeOptionService;


        public AuthManager(IUserService userService, ITokenHelper tokenHelper, ICompanyService companyService, IMailService mailService, IMailParameterService mailParameterService, IMailTemplateService mailTemplateService, IUserOperationClaimService userOperationClaimService, IOperationClaimService operationClaimService, IUserReletionShipService userReletionShipService, IUserThemeOptionService userThemeOptionService)
        {
            _userService = userService;
            _tokenHelper = tokenHelper;
            _companyService = companyService;
            _mailService = mailService;
            _mailParameterService = mailParameterService;
            _mailTemplateService = mailTemplateService;
            _userOperationClaimService = userOperationClaimService;
            _operationClaimService = operationClaimService;
            _userReletionShipService = userReletionShipService;
            _userThemeOptionService = userThemeOptionService;
        }

        public IResult CompanyExists(Company company)
        {
            var result = _companyService.CompanyExists(company);
            if (!result.Success)
            {
                return new ErrorResult(Messages.CompanyAlreadyExists);
            }
            return new SuccessResult();
        }

        public IDataResult<AccessToken> CreateAccessToken(User user, int companyId)
        {
            var claims = _userService.GetClaims(user, companyId);
            var company = _companyService.GetById(companyId).Data;
            var accessToken = _tokenHelper.CreateToken(user, claims, companyId, company.Name);
            return new SuccessDataResult<AccessToken>(accessToken, Messages.SuccessfulLogin);
        }

        public IDataResult<User> GetById(int id)
        {
            return new SuccessDataResult<User>(_userService.GetById(id));
        }

        public IDataResult<User> GetByMailConfirmValue(string value)
        {
            return new SuccessDataResult<User>(_userService.GetByMailConfirmValue(value));
        }

        public IDataResult<User> Login(UserForLogin userForLogin)
        {
            var userToCheck = _userService.GetByMail(userForLogin.Email);
            if (userToCheck == null)
            {
                return new ErrorDataResult<User>(Messages.UserNotFound);
            }
            if (!HashingHelper.VerifyPasswordHash(userForLogin.Password, userToCheck.PasswordHash, userToCheck.PasswordSalt))
            {
                return new ErrorDataResult<User>(Messages.PasswordError);
            }
            return new SuccessDataResult<User>(userToCheck, Messages.SuccessfulLogin);

        }

        [TransactionScopeAspect]
        public IDataResult<UserCompanyDto> Register(UserForRegister userForRegister, string password, Company company)
        {
            byte[] passworhHash, passwordSalt;
            HashingHelper.CreatePasswordHash(password, out passworhHash, out passwordSalt);
            var user = new User()
            {
                Email = userForRegister.Email,
                Name = userForRegister.Name,
                AddedAt = DateTime.Now,
                MailConfirmDate = DateTime.Now,
                MailConfirmValue = Guid.NewGuid().ToString(),
                MailConfirm = false,
                IsActive = true,
                PasswordHash = passworhHash,
                PasswordSalt = passwordSalt
            };

            //ValidationTools.Validate(new UserValidator(), user);
            //ValidationTools.Validate(new CompanyValidator(), company);
            _userService.Add(user);
            _companyService.Add(company);
            _companyService.UserCompanyAdd(user.Id, company.Id);

            UserCompanyDto userCompanyDto = new UserCompanyDto()
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                AddedAt = user.AddedAt,
                CompanyId = company.Id,
                IsActive = true,
                MailConfirmDate = user.MailConfirmDate,
                MailConfirmValue = user.MailConfirmValue,
                PasswordHash = user.PasswordHash,
                PasswordSalt = user.PasswordSalt
            };


            var operationClaims = _operationClaimService.GetList().Data;
            foreach (var operationClaim in operationClaims)
            {
                if (operationClaim.Name != "Admin" && operationClaim.Name != "MailParameter" && operationClaim.Name != "MailTemplate" && operationClaim.Name.Contains("UserOperationClaim"))
                {
                    UserOperationClaim addUserOperationClaim = new UserOperationClaim()
                    {
                        CompanyId = company.Id,
                        AddedAt = DateTime.Now,
                        IsActive = true,
                        OperationClaimId = operationClaim.Id,
                        UserId = user.Id
                    };
                    _userOperationClaimService.Add(addUserOperationClaim);
                }

            }

            UserTheme userTheme = new UserTheme()
            {
                UserId = user.Id,
                SidenavColor = "primary",
                SidenavType = "dark"
            };
            _userThemeOptionService.Update(userTheme);


            // Onay maili 
            SendMail(user);

            return new SuccessDataResult<UserCompanyDto>(userCompanyDto, Messages.UserRegistered);
        }

        void SendMail(User user)
        {
            string subject = "Kullanıcı kayıt onay maili.";
            string body = "Kullanıcınız sisteme kayıt oldu, kaydınızı tamamlamak için aşağıdaki linke tıklayın";
            string link = "https://localhost:4200/registerConfirm/" + user.MailConfirmValue;
            string linkDescription = "Kaydı onaylamak için tıklayın";

            var mailTemplate = _mailTemplateService.GetByTemplateName(13, "Kayıt");
            string templateBody = mailTemplate.Data.Value;
            templateBody = templateBody.Replace("{{title}}", subject);
            templateBody = templateBody.Replace("{{message}}", body);
            templateBody = templateBody.Replace("{{link}}", link);
            templateBody = templateBody.Replace("{{linkDescription}}", linkDescription);

            var mailParameter = _mailParameterService.Get(13);
            SendMailDto sendMailDto = new SendMailDto
            {
                email = user.Email,
                subject = "Kullanıcı onay maili",
                body = templateBody,
                mailParameter = mailParameter.Data
            };

            _mailService.SendMail(sendMailDto);

            user.MailConfirmDate = DateTime.Now;
            _userService.Update(user);
        }

        [TransactionScopeAspect]
        public IDataResult<List<UserReletionShipDto>> RegisterSecondAccount(UserForRegister userForRegister, string password, int companyId, int adminUserId)
        {
            byte[] passworhHash, passwordSalt;
            HashingHelper.CreatePasswordHash(password, out passworhHash, out passwordSalt);
            var user = new User()
            {
                Email = userForRegister.Email,
                Name = userForRegister.Name,
                AddedAt = DateTime.Now,
                MailConfirmDate = DateTime.Now,
                MailConfirmValue = Guid.NewGuid().ToString(),
                MailConfirm = false,
                IsActive = true,
                PasswordHash = passworhHash,
                PasswordSalt = passwordSalt
            };
            _userService.Add(user);
            _companyService.UserCompanyAdd(user.Id, companyId);

            var operationClaims = _operationClaimService.GetList().Data;
            foreach (var operationClaim in operationClaims)
            {
                if (operationClaim.Name != "Admin" && operationClaim.Name != "MailParameter" && operationClaim.Name != "MailTemplate" && operationClaim.Name.Contains("UserOperationClaim"))
                {
                    UserOperationClaim addUserOperationClaim = new UserOperationClaim()
                    {
                        CompanyId = companyId,
                        AddedAt = DateTime.Now,
                        IsActive = true,
                        OperationClaimId = operationClaim.Id,
                        UserId = user.Id
                    };
                    _userOperationClaimService.Add(addUserOperationClaim);
                }

            }

            UserReletionShip userReletionShip = new UserReletionShip
            {
                AdminUserId = adminUserId,
                UserUserId = user.Id
            };


            UserTheme userTheme = new UserTheme()
            {
                UserId = user.Id,
                SidenavColor = "primary",
                SidenavType = "dark"
            };
            _userThemeOptionService.Update(userTheme);

            _userReletionShipService.Add(userReletionShip);

            var result = _userReletionShipService.GetListDto(adminUserId).Data;

            SendMail(user);
            return new SuccessDataResult<List<UserReletionShipDto>>(result, Messages.UserRegistered);
        }

        public IResult Update(User user)
        {
            _userService.Update(user);
            return new SuccessResult(Messages.ChangeStatus);
        }

        public IResult UserExists(string email)
        {
            var user = _userService.GetByMail(email);
            if (user != null)
            {
                return new ErrorResult(Messages.ExistsAlreadyUser);
            }
            return new SuccessResult();
        }

        public IResult SendConfirmEmail(User user)
        {
            if (user.MailConfirm == true)
            {
                return new ErrorResult(Messages.MailAlreadyConfirm);
            }
            if (user.MailConfirmDate.ToShortDateString() == DateTime.Now.ToShortDateString())
            {
                if (user.MailConfirmDate.Hour == DateTime.Now.Hour && user.MailConfirmDate.AddMinutes(5).Minute < DateTime.Now.Minute)
                {
                    SendMail(user);
                    return new SuccessResult(Messages.MailConfirmSendSuccessfull);
                }
                else
                {
                    return new ErrorResult(Messages.MailConfirmationHasNotExpirated);
                }
            }
            SendMail(user);
            return new SuccessResult(Messages.MailConfirmSendSuccessfull);
        }

        public IDataResult<UserCompany> GetCompany(int userId)
        {
            return new SuccessDataResult<UserCompany>(_companyService.GetCompany(userId).Data);
        }

        public IDataResult<User> GetByEmail(string email)
        {
            return new SuccessDataResult<User>(_userService.GetByMail(email));
        }

        public IResult SendForgotPasswordEmail(User user, string value)
        {
            string subject = "Şifremi Unuttum";
            string body = "e-Mutabıkat sistesi tarafaından sizin isteğinizle şifre yenileme isteği aldık. Aşağıda belirtilen linke tıklayarak şifrenizi yenileyebilirsiniz. Unutmayın! bu link sadece 1 saat süreliğine geçerlidir.";
            string link = "https://localhost:7127/api/Auth/forgotPasswordLinkCheck?value=" + value;
            string linkDescription = "Kaydı onaylamak için tıklayın";

            var mailTemplate = _mailTemplateService.GetByTemplateName(13, "Kayıt");
            string templateBody = mailTemplate.Data.Value;
            templateBody = templateBody.Replace("{{title}}", subject);
            templateBody = templateBody.Replace("{{message}}", body);
            templateBody = templateBody.Replace("{{link}}", link);
            templateBody = templateBody.Replace("{{linkDescription}}", linkDescription);

            var mailParameter = _mailParameterService.Get(13);
            SendMailDto sendMailDto = new SendMailDto
            {
                email = user.Email,
                subject = "Şifremi Unuttum",
                body = templateBody,
                mailParameter = mailParameter.Data
            };

            _mailService.SendMail(sendMailDto);

            return new SuccessResult(Messages.MailSendSuccess);
        }

        public IResult ChangePassword(User user)
        {
            _userService.Update(user);
            return new SuccessResult(Messages.ChangedPassword);
        }
    }
}
