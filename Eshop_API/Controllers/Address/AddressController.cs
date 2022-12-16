﻿using eshop_api.Controllers;
using eshop_api.Models.DTO.Order;
using Eshop_API.Models.DTO.Adress;
using eshop_pbl6.Helpers.Common;
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
        [HttpGet("get-list-address-by-user")]
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
        [HttpGet("get-address-by-id")]
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
        [HttpPost("add-address")]
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
        [HttpPut("update-address")]
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
        [HttpDelete("del-address")]
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
