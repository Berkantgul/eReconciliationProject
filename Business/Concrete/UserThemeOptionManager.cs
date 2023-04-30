using Business.Abstract;
using Business.Contans;
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
    public class UserThemeOptionManager : IUserThemeOptionService
    {
        private readonly IUserThemeOptionDal _userThemeOptionDal;

        public UserThemeOptionManager(IUserThemeOptionDal userThemeOptionDal)
        {
            _userThemeOptionDal = userThemeOptionDal;
        }

        public void Delete(UserTheme userTheme)
        {
            _userThemeOptionDal.Delete(userTheme);
        }

        public IDataResult<UserTheme> GetByUserId(int userId)
        {
            return new SuccessDataResult<UserTheme>(_userThemeOptionDal.Get(p => p.UserId == userId));
        }

        public IResult Update(UserTheme userTheme)
        {
            var result = _userThemeOptionDal.Get(p => p.UserId == userTheme.UserId);
            if (result == null)
            {
                userTheme.Id = 0;
                _userThemeOptionDal.Add(userTheme);
            }
            else
            {
                _userThemeOptionDal.Update(userTheme);
            }
            return new SuccessResult(Messages.UpdatedUserTheme);
        }
    }
}
