﻿using Core.Entities;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Security.JWT;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IAuthService
    {
        // User Kayıt
        IDataResult<UserCompanyDto> Register(UserForRegister userForRegister, string password, Company company);
        IDataResult<List<UserReletionShipDto>> RegisterSecondAccount(UserForRegister userForRegister, string password, int companyId,int adminUserId);
        // User Login
        IDataResult<User> Login(UserForLogin userForLogin);
        // Kullanıcı var mı yok mu?
        IResult UserExists(string email);
        // Yeni kullanıcı için token oluşturma.
        IDataResult<AccessToken> CreateAccessToken(User user, int companyId);
        IResult CompanyExists(Company company);
        IDataResult<User> GetByMailConfirmValue(string value);
        IResult Update(User user);
        IResult ChangePassword(User user);
        IDataResult<User> GetById(int id);
        IDataResult<User> GetByEmail(string email);
        IResult SendConfirmEmail(User user);
        IDataResult<UserCompany> GetCompany(int userId);
        IResult SendForgotPasswordEmail(User user, string value);
    }
}
