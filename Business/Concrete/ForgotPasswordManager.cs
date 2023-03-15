using Business.Abstract;
using Core.Entities;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class ForgotPasswordManager : IForgotPasswordService
    {
        private readonly IForgotPasswordDal _forgotPasswordDal;

        public ForgotPasswordManager(IForgotPasswordDal forgotPasswordDal)
        {
            _forgotPasswordDal = forgotPasswordDal;
        }

        public IDataResult<ForgotPassword> CreateForgotPassword(User user)
        {
            ForgotPassword forgotPassword = new ForgotPassword
            {
                UserId = user.Id,
                IsActive = true,
                SendDate = DateTime.Now,
                Value = Guid.NewGuid().ToString(),
            };
            _forgotPasswordDal.Add(forgotPassword);
            return new SuccessDataResult<ForgotPassword>(forgotPassword);
        }

        public ForgotPassword GetForgotPassword(string value)
        {
            return _forgotPasswordDal.Get(i => i.Value == value);
        }

        public IDataResult<List<ForgotPassword>> GetListByUserId(int userId)
        {
            return new SuccessDataResult<List<ForgotPassword>>(_forgotPasswordDal.GetAll(i => i.UserId == userId && i.IsActive == true));
        }

        public void Update(ForgotPassword forgotPassword)
        {
            _forgotPasswordDal.Update(forgotPassword);
        }
    }
}
