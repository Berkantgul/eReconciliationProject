using Business.Abstract;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class UserReletionShipManager : IUserReletionShipService
    {
        private readonly IUserReletionShipDal _userReletionShipDal;

        public UserReletionShipManager(IUserReletionShipDal userReletionShipDal)
        {
            _userReletionShipDal = userReletionShipDal;
        }

        public void Add(UserReletionShip userReletionShip)
        {
            _userReletionShipDal.Add(userReletionShip);
        }

        public void Delete(UserReletionShip userReletionShip)
        {
            _userReletionShipDal.Delete(userReletionShip);
        }

        public IDataResult<UserReletionShipDto> GetById(int userUserId)
        {
            return new SuccessDataResult<UserReletionShipDto>(_userReletionShipDal.GetById(userUserId));
        }

        public List<UserReletionShip> GetList(int userId)
        {
            return _userReletionShipDal.GetAll(i => i.UserUserId == userId);
        }

        public IDataResult<List<UserReletionShipDto>> GetListDto(int adminUserId)
        {
            return new SuccessDataResult<List<UserReletionShipDto>>(_userReletionShipDal.GetListDto(adminUserId));
        }

        public void Update(UserReletionShip userReletionShip)
        {
            _userReletionShipDal.Update(userReletionShip);
        }
    }
}
