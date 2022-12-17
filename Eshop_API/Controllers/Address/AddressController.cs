using eshop_api.Authorization;
using eshop_api.Controllers;
using eshop_api.Models.DTO.Order;
using Eshop_API.Models.DTO.Adress;
using eshop_pbl6.Helpers.Common;
using eshop_pbl6.Helpers.Identities;
using eshop_pbl6.Services.Addresses;
using eshop_pbl6.Services.Identities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json.Serialization;

namespace Eshop_API.Controllers.Address
{
    public class AddressController : BaseController
    {
        private readonly IAddressService _addressService;

        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }
        
        /// <summary>
        /// Lấy danh sách các tỉnh
        /// <para>Created by: MinhDuc</para>
        /// </summary>
        /// <response code="500">Lỗi khi có exception</response>
        [HttpGet("get-province")]
        public IActionResult GetProvince()
        {
            try
            {
                var result = _addressService.GetProvince();
                return Ok(CommonReponse.CreateResponse(ResponseCodes.Ok, "lấy dữ liệu thành công", result));
            }
            catch (Exception ex)
            {
                return Ok(CommonReponse.CreateResponse(ResponseCodes.ErrorException, ex.Message, "null"));
            }
        }
        /// <summary>
        /// Lấy danh sách các huyện
        /// <para>Created by: MinhDuc</para>
        /// </summary>
        /// <response code="500">Lỗi khi có exception</response>
        [HttpGet("get-district")]
        public IActionResult GetDistrict(int idProvince)
        {
            try
            {
                var result = _addressService.GetDistrict(idProvince);
                return Ok(CommonReponse.CreateResponse(ResponseCodes.Ok, "lấy dữ liệu thành công", result));
            }
            catch (Exception ex)
            {
                return Ok(CommonReponse.CreateResponse(ResponseCodes.ErrorException, ex.Message, "null"));
            }
        }
        /// <summary>
        /// Lấy danh sách các xã
        /// <para>Created by: MinhDuc</para>
        /// </summary>
        /// <response code="500">Lỗi khi có exception</response>
        [HttpGet("get-community")]
        public IActionResult GetCommunity(int idDistrict)
        {
            try
            {
                var result = _addressService.GetComunity(idDistrict);
                return Ok(CommonReponse.CreateResponse(ResponseCodes.Ok, "lấy dữ liệu thành công", result));
            }
            catch (Exception ex)
            {
                return Ok(CommonReponse.CreateResponse(ResponseCodes.ErrorException, ex.Message, "null"));
            }
        }
        /// <summary>
        /// lấy danh sách địa chỉ của người dùng
        /// <para>Created by: MinhDuc</para>
        /// </summary>
        /// <response code="401">Lỗi không đăng nhập</response>
        /// <response code="500">Lỗi khi có exception</response>
        [HttpGet("get-list-address-by-user")]
        [Authorize(EshopPermissions.AddressPermissions.Get)]
        public IActionResult GetListAddressByUser()
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                var handler = new JwtSecurityTokenHandler();
                var jwtSecurityToken = handler.ReadJwtToken(token);
                var idUser = jwtSecurityToken.Claims.First(claim => claim.Type == "Id").Value;
                var result = _addressService.GetListAddressByUser(int.Parse(idUser));
                return Ok(CommonReponse.CreateResponse(ResponseCodes.Ok, "lấy dữ liệu thành công", result));
            }
            catch (Exception ex)
            {
                return Ok(CommonReponse.CreateResponse(ResponseCodes.ErrorException, ex.Message, "null"));
            }
        }
        /// <summary>
        /// lấy địa chỉ theo id
        /// <para>Created by: MinhDuc</para>
        /// </summary>
        /// <response code="401">Lỗi không đăng nhập</response>
        /// <response code="500">Lỗi khi có exception</response>
        [HttpGet("get-address-by-id")]
        [Authorize(EshopPermissions.AddressPermissions.Get)]
        public IActionResult GetAddressById(int idAddress)
        {
            try
            {
                var result = _addressService.GetAddressById(idAddress);
                return Ok(CommonReponse.CreateResponse(ResponseCodes.Ok, "lấy dữ liệu thành công", result));
            }
            catch (Exception ex)
            {
                return Ok(CommonReponse.CreateResponse(ResponseCodes.ErrorException, ex.Message, "null"));
            }
        }
        /// <summary>
        /// thêm địa chỉ của người dùng
        /// <para>Created by: MinhDuc</para>
        /// </summary>
        /// <response code="401">Lỗi không đăng nhập</response>
        /// <response code="500">Lỗi khi có exception</response>
        [HttpPost("add-address")]
        [Authorize(EshopPermissions.AddressPermissions.Add)]
        public async Task<IActionResult> AddAddress(CreateUpdateAddress createUpdateAddress)
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                var handler = new JwtSecurityTokenHandler();
                var jwtSecurityToken = handler.ReadJwtToken(token);
                var idUser = jwtSecurityToken.Claims.First(claim => claim.Type == "Id").Value;
                var result = await _addressService.AddAddress(createUpdateAddress, int.Parse(idUser));
                return Ok(CommonReponse.CreateResponse(ResponseCodes.Ok, "lấy dữ liệu thành công", result));
            }

            catch (Exception ex)
            {
                return Ok(CommonReponse.CreateResponse(ResponseCodes.ErrorException, ex.Message, "null"));
            }
        }
        /// <summary>
        /// chỉnh sửa địa chỉ của người dùng
        /// <para>Created by: MinhDuc</para>
        /// </summary>
        /// <response code="401">Lỗi không đăng nhập</response>
        /// <response code="500">Lỗi khi có exception</response>
        [HttpPut("update-address")]
        [Authorize(EshopPermissions.AddressPermissions.Edit)]
        public async Task<IActionResult> UpdateAddress(CreateUpdateAddress createUpdateAddress, int idAddress)
        {
            try
            {
               var result = await _addressService.UpdateAddress(createUpdateAddress, idAddress);
                return Ok(CommonReponse.CreateResponse(ResponseCodes.Ok, "lấy dữ liệu thành công", result));
            }
            catch (Exception ex)
            {
                return Ok(CommonReponse.CreateResponse(ResponseCodes.ErrorException, ex.Message, "null"));
            }
        }
        /// <summary>
        /// Xóa địa chỉ của người dùng
        /// <para>Created by: MinhDuc</para>
        /// </summary>
        /// <response code="401">Lỗi không đăng nhập</response>
        /// <response code="500">Lỗi khi có exception</response>
        [HttpDelete("del-address")]
        [Authorize(EshopPermissions.AddressPermissions.Delete)]
        public async Task<IActionResult> DelAddress(int idAddress)
        {
            try
            {
                var result = await _addressService.DelAddress(idAddress);
                return Ok(CommonReponse.CreateResponse(ResponseCodes.Ok, "lấy dữ liệu thành công", result));
            }
            catch (Exception ex)
            {
                return Ok(CommonReponse.CreateResponse(ResponseCodes.ErrorException, ex.Message, "null"));
            }
        }
    }
}
