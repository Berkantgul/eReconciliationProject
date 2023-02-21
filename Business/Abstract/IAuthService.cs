using Core.Entities;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Security.JWT;
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
        IDataResult<User> Register(UserForRegister userForRegister, string password);
        // User Login
        IDataResult<User> Login(UserForLogin userForLogin);
        // Kullanıcı var mı yok mu?
        IResult UserExists(string email);
        // Yeni kullanıcı için token oluşturma.
        IDataResult<AccessToken> CreateAccessToken(User user,int companyId);
    }
}
