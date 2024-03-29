﻿using Business.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;
        private readonly IUserReletionShipService _userReletionShipService;
        private readonly IUserThemeOptionService _userThemeOptionService;

        public UserController(IUserService userService, IAuthService authService,IUserReletionShipService userReletionShipService,IUserThemeOptionService userThemeOptionService)
        {
            _userService = userService;
            _authService = authService;
            _userReletionShipService = userReletionShipService;
            _userThemeOptionService = userThemeOptionService;
        }


        [HttpGet("getUserList")]
        public IActionResult GetUserList(int companyId)
        {
            var result = _userService.GetUserList(companyId);
            return Ok(result);
        }

        [HttpGet("getById")]
        public IActionResult GetById(int id)
        {
            var findUser = _userService.GetByIdToResult(id);
            return Ok(findUser);
        }

        [HttpPost("update")]
        public IActionResult Update(UserForRegisterToSecondAccountDto userForRegister)
        {
            var findUser = _userService.GetById(userForRegister.Id);
            if (userForRegister.Email != findUser.Email)
            {
                var userExists = _authService.UserExists(findUser.Email);
                if (!userExists.Success)
                {
                    return BadRequest(userExists.Message);
                }
            }

            var result = _userService.UpdateResult(userForRegister);
            return Ok(result);
        }

        [HttpGet("changeStatus")]
        public IActionResult ChangeStatus(int id)
        {
            var findUser = _userService.GetById(id);
            if (findUser.IsActive)
            {
                findUser.IsActive = false;
            }
            else
            {
                findUser.IsActive = true;
            }

            var result = _authService.Update(findUser);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpGet("getOperationClaimList")]
        public IActionResult GetOperationClaimList(string value,int companyId)
        {
            var result = _userService.GetOperationClaimListForUser(value, companyId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpPost("updateOperationClaim")]
        public IActionResult UpdateOperationClaim(OperationClaimListForUserDto operationClaim)
        {
            var result = _userService.UpdateOperationClaim(operationClaim);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpGet("getAdminUsersList")]
        public IActionResult GetAdminUsersList(int adminUserId)
        {
            var result = _userReletionShipService.GetListDto(adminUserId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpGet("userDelete")]
        public IActionResult UserDelete(int userId)
        {
            var result = _userService.UserDelete(userId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpGet("getUserCompanyList")]
        public IActionResult GetUserCompanyList(int userId)
        {
            var result = _userReletionShipService.GetById(userId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpGet("getListUserCompany")]
        public IActionResult GetListUserCompany(string value)
        {
            var result = _userService.GetUserCompanyList(value);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpGet("userCompanyDelete")]
        public IActionResult UserCompanyDelete(int userId,int companyId)
        {
            var result = _userService.UserCompanyDelete(userId, companyId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpGet("addUserCompany")]
        public IActionResult AddUserCompany(int userId, int companyId)
        {
            var result = _userService.AddUserCompany(userId, companyId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpGet("getAdminCompaniesForUser")]
        public IActionResult GetAdminCompaniesForUser(int adminUserId,int userUserId)
        {
            var result = _userService.GetAdminCompaniesForUser(adminUserId, userUserId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpGet("getUserTheme")]
        public IActionResult GetUserTheme(int userId)
        {
            var result = _userThemeOptionService.GetByUserId(userId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpPost("changeTheme")]
        public IActionResult ChangeTheme(UserTheme userTheme)
        {
            var result = _userThemeOptionService.Update(userTheme);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }
    }
}
