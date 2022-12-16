using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using eshop_api.Models.DTO;
using eshop_api.Entities;
using eshop_api.Helpers;
using eshop_api.Controllers;
using eshop_pbl6.Helpers.Identities;
using eshop_pbl6.Helpers.Common;
using eshop_pbl6.Models.DTO.Identities;
using eshop_api.Authorization;
using System.IdentityModel.Tokens.Jwt;
using eshop_pbl6.Services.Identities;
using System.Text.Json;
using eshop_pbl6.Authorization;

namespace eshop_pbl6.Controllers.Identities
{
    public class IdentityController : BaseController
    {
        private readonly DataContext _context;
        private readonly IUserService _userService;
        private readonly IJwtUtils _jwtUtils;

        public IdentityController(DataContext context,
            IUserService userService,
            IJwtUtils jwtUtils)
        {
            _context = context;

            _userService = userService;
            _jwtUtils = jwtUtils;
        }
        [HttpPost("register")]
        public IActionResult Register([FromBody] CreateUpdateUserDto create)
        {
            create.Username = create.Username.ToLower();
            Result result = new Result();
            result.Results.Add("User", create.Username.Trim());
            result.Results.Add("Success", false);
            if (_context.AppUsers.Any(u => u.Username == create.Username.Trim()))
            {
                return Ok(CommonReponse.CreateResponse(ResponseCodes.BadRequest, "Tài khoản đã tồn tại", result));

            }
            using var hmac = new HMACSHA512();
            var passwordBytes = Encoding.UTF8.GetBytes(create.Password);
            var newUser = new User
            {
                Username = create.Username.Trim(),
                PasswordSalt = hmac.Key,
                PasswordHash = hmac.ComputeHash(passwordBytes),
                FirstName = create.FirstName,
                LastName = create.LastName,
                Phone = create.Phone,
                BirthDay = create.BirthDay,
                Gender = create.Gender,
                AvatarUrl = create.Avatar != null ? CloudImage.UploadImage(create.Avatar) : "",
                Email = create.Email,
                RoleId = (int)RoleEnum.User
            };

            var userResult = _context.AppUsers.Add(newUser);
            _context.SaveChanges();
            var token = _jwtUtils.GenerateJwtToken(userResult.Entity);
            result.Results.Add("Token", token);
            result.Results["Success"] = true;
            return Ok(CommonReponse.CreateResponse(ResponseCodes.Ok, "Đăng kí thành công", result));
        }
        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLogin userLoin)
        {
            try
            {
                userLoin.Username = userLoin.Username.ToLower();
                Result result = new Result();
                result.Results.Add("User", userLoin.Username);
                result.Results.Add("Success", false);
                var currentUser = _context.AppUsers
                    .FirstOrDefault(u => u.Username == userLoin.Username);
                if (currentUser == null)
                {
                    return Ok(CommonReponse.CreateResponse(ResponseCodes.Unauthorized, "Tài khoản hoặc mật khẩu không trùng khớp", result));
                }
                using var hmac = new HMACSHA512(currentUser.PasswordSalt);
                var passwordBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(userLoin.Password));
                for (int i = 0; i < currentUser.PasswordHash.Length; i++)
                {
                    if (currentUser.PasswordHash[i] != passwordBytes[i])
                    {
                        return Ok(CommonReponse.CreateResponse(ResponseCodes.Unauthorized, "Tài khoản hoặc mật khẩu không trùng khớp", result));
                    }
                }
                var token = _jwtUtils.GenerateJwtToken(currentUser);
                result.Results.Add("Token", token);
                result.Results["Success"] = true;
                return Ok(CommonReponse.CreateResponse(ResponseCodes.Ok, "Đăng nhập thành công", result));
            }
            catch (Exception ex)
            {
                return Ok(CommonReponse.CreateResponse(ResponseCodes.ErrorException, ex.Message, "null"));
            }

        }

        // Change Password
        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword(string username, string passwordOld, string passwordNew)
        {
            try
            {
                if (await _userService.ChangePassworrd(username, passwordOld, passwordNew) == true)
                {
                    return Ok(CommonReponse.CreateResponse(ResponseCodes.Ok, "Thay đổi mật khẩu thành công", true));
                }
                return Ok(CommonReponse.CreateResponse(ResponseCodes.Unauthorized, "Tài khoản hoặc mật khẩu không chính xác", false));

            }
            catch (System.Exception ex)
            {
                return Ok(CommonReponse.CreateResponse(ResponseCodes.ErrorException,ex.Message,false));
            }
        }
        [Authorize(EshopPermissions.ProductPermissions.Get)]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_context.AppUsers.ToList());
        }
        [Authorize(EshopPermissions.UserPermissions.Get)]
        [HttpGet("get-user")]
        public async Task<IActionResult> GetUser(){
            try{
                var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                var handler = new JwtSecurityTokenHandler();
                var jwtSecurityToken = handler.ReadJwtToken(token);
                var username = jwtSecurityToken.Claims.First(claim => claim.Type == "nameid").Value;
                var result = await _userService.GetByUserName(username);
                return Ok(CommonReponse.CreateResponse(ResponseCodes.Ok, "Lấy dữ liệu thành công",result));
            }
            catch(Exception  ex){
                return Ok(CommonReponse.CreateResponse(ResponseCodes.ErrorException,ex.Message,"null"));
            }

        }
        [HttpPut("change-profile")]
        [Authorize(EshopPermissions.UserPermissions.Edit)]
        public async Task<IActionResult> ChangeProfile([FromForm] UpdateUserDto userDto){
            try{
                var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                var handler = new JwtSecurityTokenHandler();
                var jwtSecurityToken = handler.ReadJwtToken(token);
                var username = jwtSecurityToken.Claims.First(claim => claim.Type == "nameid").Value;
                var result = await _userService.UpdateUserById(userDto,username);
                if(result != null){
                    return Ok(CommonReponse.CreateResponse(ResponseCodes.Ok, "Thay đổi thông tin cá nhân thành công",result));
                }
                return Ok(CommonReponse.CreateResponse(ResponseCodes.Ok, "Thay đổi thông tin cá nhân không thành công", "null"));
            }
            catch(Exception ex){
                return Ok(CommonReponse.CreateResponse(ResponseCodes.ErrorException,ex.Message,"null"));
            }
            
        }
    }
}