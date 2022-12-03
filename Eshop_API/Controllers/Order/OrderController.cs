using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eshop_api.Services.Orders;
using Microsoft.AspNetCore.Mvc;
using eshop_api.Models.DTO.Order;
using eshop_pbl6.Helpers.Common;
using eshop_api.Helpers;
using System.Security.Claims;
using eshop_api.Authorization;
using eshop_pbl6.Helpers.Identities;
using System.IdentityModel.Tokens.Jwt;

namespace eshop_api.Controllers.Products
{
    public class OrderController : BaseController
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [HttpGet("get-list-order")]
        public IActionResult GetListOrders()
        {
            try{
                var result = _orderService.GetListOrders();
                return Ok(CommonReponse.CreateResponse(ResponseCodes.Ok, "lấy dữ liệu thành công", result));
            }
            catch(Exception ex)
            {
                return Ok(CommonReponse.CreateResponse(ResponseCodes.ErrorException, ex.Message, "null"));
            }
        }
        [HttpGet("get-order-by-userid")]
        public IActionResult GetOrdersByUserId(int userId)
        {
            try{
                var result = _orderService.GetOrdersByUserId(userId);
                return Ok(CommonReponse.CreateResponse(ResponseCodes.Ok, "lấy dữ liệu thành công", result));
            }
            catch(Exception ex)
            {
                return Ok(CommonReponse.CreateResponse(ResponseCodes.ErrorException, ex.Message, "null"));
            }
        }
        [HttpGet("get-order-by-id")]
        public IActionResult GetOrderById(string idOrder)
        {
            try{
                var result = _orderService.GetOrderById(idOrder);
                return Ok(CommonReponse.CreateResponse(ResponseCodes.Ok, "lấy dữ liệu thành công", result));
            }
            catch(Exception ex)
            {
                return Ok(CommonReponse.CreateResponse(ResponseCodes.ErrorException, ex.Message, "null"));
            }
        }
        [HttpGet("get-order-by-status")]
        public IActionResult GetOrdersByStatus(int status)
        {
            try{
                var result = _orderService.GetOrdersByStatus(status);
                return Ok(CommonReponse.CreateResponse(ResponseCodes.Ok, "lấy dữ liệu thành công", result));
            }
            catch(Exception ex)
            {
                return Ok(CommonReponse.CreateResponse(ResponseCodes.ErrorException, ex.Message, "null"));
            }
        }
        [HttpGet("get-order-by-status-of-each-user")]
        public IActionResult GetOrderByStatusOfEachUser(int userId, int status)
        {
            try{
                var result = _orderService.GetOrderByStatusOfEachUser(userId, status);
                return Ok(CommonReponse.CreateResponse(ResponseCodes.Ok, "lấy dữ liệu thành công", result));
            }
            catch(Exception ex)
            {
                return Ok(CommonReponse.CreateResponse(ResponseCodes.ErrorException, ex.Message, "null"));
            }
        }
        [HttpPost("add-order")]
        [Authorize(EshopPermissions.OrderPermissions.Add)]
        public async Task<IActionResult> AddOrder(List<OrderDetailDTO> orderDetailDTO, int idAddress, int payment, int time)
        {
            try{
                //var serId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                var handler = new JwtSecurityTokenHandler();
                var jwtSecurityToken = handler.ReadJwtToken(token);
                var username = jwtSecurityToken.Claims.First(claim => claim.Type == "nameid").Value;
                // var claimsIdentity = (ClaimsIdentity)User.Identity;
                // var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                // int userId = Convert.ToInt32(claim.Value);
                var result = await _orderService.AddOrder(orderDetailDTO, username, idAddress, payment, time);
                int i = 0;
                return Ok(CommonReponse.CreateResponse(ResponseCodes.Ok, "thêm dữ liệu thành công", result));
            }
            catch(Exception ex)
            {
                return Ok(CommonReponse.CreateResponse(ResponseCodes.ErrorException, ex.Message, "null"));
            }
        }
        [HttpPost("add-to-cart")]
        [Authorize(EshopPermissions.OrderPermissions.Add)]
        public async Task<IActionResult> AddToCart(OrderDetailDTO orderDetailDTO)
        {
            try{
                //var serId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                var handler = new JwtSecurityTokenHandler();
                var jwtSecurityToken = handler.ReadJwtToken(token);
                var username = jwtSecurityToken.Claims.First(claim => claim.Type == "nameid").Value;
                // var claimsIdentity = (ClaimsIdentity)User.Identity;
                // var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                // int userId = Convert.ToInt32(claim.Value);
                var result = await _orderService.AddToCart(orderDetailDTO, username);
                return Ok(CommonReponse.CreateResponse(ResponseCodes.Ok, "thêm dữ liệu thành công", result));
            }
            catch(Exception ex)
            {
                return Ok(CommonReponse.CreateResponse(ResponseCodes.ErrorException, ex.Message, "null"));
            }
        }
        [HttpPost("get-cart")]
        [Authorize(EshopPermissions.OrderPermissions.Get)]
        public async Task<IActionResult> GetCart()
        {
            try{
                //var serId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                var handler = new JwtSecurityTokenHandler();
                var jwtSecurityToken = handler.ReadJwtToken(token);
                var username = jwtSecurityToken.Claims.First(claim => claim.Type == "nameid").Value;
                // var claimsIdentity = (ClaimsIdentity)User.Identity;
                // var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                // int userId = Convert.ToInt32(claim.Value);
                var result = await _orderService.GetCart(username);
                return Ok(CommonReponse.CreateResponse(ResponseCodes.Ok, "thêm dữ liệu thành công", result));
            }
            catch(Exception ex)
            {
                return Ok(CommonReponse.CreateResponse(ResponseCodes.ErrorException, ex.Message, "null"));
            }
        }
        [HttpPut("del-from-cart")]
        [Authorize(EshopPermissions.OrderPermissions.Edit)]
        public async Task<IActionResult> DelFromCart(int idProduct, int quantity)
        {
            try{
                //var serId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                var handler = new JwtSecurityTokenHandler();
                var jwtSecurityToken = handler.ReadJwtToken(token);
                var username = jwtSecurityToken.Claims.First(claim => claim.Type == "nameid").Value;
                // var claimsIdentity = (ClaimsIdentity)User.Identity;
                // var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                // int userId = Convert.ToInt32(claim.Value);
                var result = await _orderService.DelFromCart(idProduct, username, quantity);
                return Ok(CommonReponse.CreateResponse(ResponseCodes.Ok, "xóa dữ liệu thành công", result));
            }
            catch(Exception ex)
            {
                return Ok(CommonReponse.CreateResponse(ResponseCodes.ErrorException, ex.Message, "null"));
            }
        }
        [HttpPut("change-status")]
        public async Task<IActionResult> ChangeStatus(string idOrder, int status)
        {
            try{
                var result = await _orderService.ChangeStatus(idOrder, status);
                return Ok(CommonReponse.CreateResponse(ResponseCodes.Ok, "cập nhật dữ liệu thành công", result));
            }
            catch(Exception ex)
            {
                return Ok(CommonReponse.CreateResponse(ResponseCodes.ErrorException, ex.Message, "null"));
            }
        }
        [HttpPut("update-order")]
        public async Task<IActionResult> UpdateOrder(CreateUpdateOrder createUpdateOrder, string idOrder)
        {
            try{
                var result = await _orderService.UpdateOrder(createUpdateOrder, idOrder);
                return Ok(CommonReponse.CreateResponse(ResponseCodes.Ok, "cập nhật dữ liệu thành công", result));
            }
            catch(Exception ex)
            {
                return Ok(CommonReponse.CreateResponse(ResponseCodes.ErrorException, ex.Message, "null"));
            }
        }
        [HttpDelete("delete-order")]
        public async Task<IActionResult> DeleteOrder(string idOrder)
        {
            try{
                var result = await _orderService.DeleteOrderById(idOrder);
                return Ok(CommonReponse.CreateResponse(ResponseCodes.Ok, "xóa dữ liệu thành công", result));
            }
            catch(Exception ex)
            {
                return Ok(CommonReponse.CreateResponse(ResponseCodes.ErrorException, ex.Message, "null"));
            }
        }
    }
}