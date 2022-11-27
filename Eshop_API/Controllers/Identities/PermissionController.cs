using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eshop_api.Authorization;
using eshop_api.Controllers;
using eshop_pbl6.Helpers.Common;
using eshop_pbl6.Helpers.Identities;
using eshop_pbl6.Services.Identities;
using Microsoft.AspNetCore.Mvc;

namespace eshop_pbl6.Controllers.Identities
{
    public class PermissionController : BaseController
    {
        private readonly IUserService _userService;

        public PermissionController(IUserService userService)
        {
            _userService = userService;
        }
        [Authorize(EshopPermissions.ManagerPermissions.Get)]
        [HttpGet("get-all-permissions")]
        public IActionResult Get()
        {
            try
            {
                var result = _userService.GetAllPermission();
                return Ok(CommonReponse.CreateResponse(ResponseCodes.Ok,"Get dữ liệu thành công",result));
            }
            catch (Exception ex)
            {
                 return Ok(CommonReponse.CreateResponse(ResponseCodes.ErrorException,ex.Message,"null"));
            }
            
        }
        [Authorize(EshopPermissions.ManagerPermissions.Get)]
        [HttpGet("get-all-roles")]
        public IActionResult GetRoles()
        {
            try
            {
                var result = _userService.GetAllRoles();
                return Ok(CommonReponse.CreateResponse(ResponseCodes.Ok,"Get dữ liệu thành công",result));
            }
            catch (Exception ex)
            {
                 return Ok(CommonReponse.CreateResponse(ResponseCodes.ErrorException,ex.Message,"null"));
            }
            
        }
        [Authorize(EshopPermissions.ManagerPermissions.Get)]
        [HttpGet("get-permissions-by-username")]
        public IActionResult GetPermissionByUserName(string username)
        {
            try
            {
                var result = _userService.GetPermissionByUser(username);
                if(result != null){
                    return Ok(CommonReponse.CreateResponse(ResponseCodes.Ok,"Get dữ liệu thành công",result));
                }
                return Ok(CommonReponse.CreateResponse(ResponseCodes.ErrorData,"Get dữ liệu không thành công","null"));
            }
            catch (Exception ex)
            {
                 return Ok(CommonReponse.CreateResponse(ResponseCodes.ErrorException,ex.Message,"null"));
            }
            
        }
    }
}