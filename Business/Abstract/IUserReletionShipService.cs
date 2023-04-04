using Core.Utilities.Results.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IUserReletionShipService
    {
        IDataResult<List<UserReletionShipDto>> GetListDto(int adminUserId);
        IDataResult<UserReletionShipDto> GetById(int userUserId);
        void Update(UserReletionShip userReletionShip);
        void Add(UserReletionShip userReletionShip);
        void Delete(UserReletionShip userReletionShip);
        List<UserReletionShip> GetList(int userId);
    }
}
