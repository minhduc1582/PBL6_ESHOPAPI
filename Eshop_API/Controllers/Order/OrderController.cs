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
using Eshop_API.Services.VNPAY;
using Eshop_API.Models.DTO.VNPAY;
using Eshop_API.Helpers.Orders;

namespace eshop_api.Controllers.Products
{
    public class OrderController : BaseController
    {
        private readonly IOrderService _orderService;
        //private readonly IVnPayService _vnPayService;
        public OrderController(IOrderService orderService)
                              //  IVnPayService vnPayService)
        {
            _orderService = orderService;
         //   _vnPayService = vnPayService;
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
        [Authorize(EshopPermissions.OrderPermissions.Get)]
        public IActionResult GetOrderById(Guid idOrder)
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
        [Authorize(EshopPermissions.OrderPermissions.GetList)]
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
        [Authorize(EshopPermissions.OrderPermissions.Get)]
        public IActionResult GetOrderByStatusOfEachUser(int status)
        {
            try{
                var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                var handler = new JwtSecurityTokenHandler();
                var jwtSecurityToken = handler.ReadJwtToken(token);
                string idUser = jwtSecurityToken.Claims.First(claim => claim.Type == "Id").Value;
                var result = _orderService.GetOrderByStatusOfEachUser(int.Parse(idUser), status);
                return Ok(CommonReponse.CreateResponse(ResponseCodes.Ok, "lấy dữ liệu thành công", result));
            }
            catch(Exception ex)
            {
                return Ok(CommonReponse.CreateResponse(ResponseCodes.ErrorException, ex.Message, "null"));
            }
        }
        [HttpPost("add-order")]
        [Authorize(EshopPermissions.OrderPermissions.Add)]
        public async Task<IActionResult> AddOrder(List<OrderDetailDTO> orderDetailDTO, int idAddress, PaymentMethod payment, int time)
        {
            try{
                string remoteIpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
                string urlOrigin = "";
                if (HttpContext.Request.Headers.ContainsKey("Origin"))
                {
                    urlOrigin = Request.Headers["Origin"];
                    // Do stuff with the values... probably .FirstOrDefault()
                }
                //var serId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                var handler = new JwtSecurityTokenHandler();
                var jwtSecurityToken = handler.ReadJwtToken(token);
                string idUser = jwtSecurityToken.Claims.First(claim => claim.Type == "Id").Value;
                // var claimsIdentity = (ClaimsIdentity)User.Identity;
                // var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                // int userId = Convert.ToInt32(claim.Value);
                var result = await _orderService.AddOrder(orderDetailDTO, int.Parse(idUser), idAddress, payment, time,remoteIpAddress,urlOrigin);
                //result.Id
                
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
                return Ok(CommonReponse.CreateResponse(ResponseCodes.Ok, "lấy dữ liệu thành công", result));
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
                var idUser = jwtSecurityToken.Claims.First(claim => claim.Type == "Id").Value;
                // var claimsIdentity = (ClaimsIdentity)User.Identity;
                // var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                // int userId = Convert.ToInt32(claim.Value);
                var result = await _orderService.DelFromCart(idProduct, int.Parse(idUser), quantity);
                return Ok(CommonReponse.CreateResponse(ResponseCodes.Ok, "xóa dữ liệu thành công", result));
            }
            catch(Exception ex)
            {
                return Ok(CommonReponse.CreateResponse(ResponseCodes.ErrorException, ex.Message, "null"));
            }
        }
        [HttpPut("change-status")]
        public async Task<IActionResult> ChangeStatus(List<Guid> idOrder, int status, string note)
        {
            try{
                var result = await _orderService.ChangeStatus(idOrder, status, note);
                return Ok(CommonReponse.CreateResponse(ResponseCodes.Ok, "cập nhật dữ liệu thành công", result));
            }
            catch(Exception ex)
            {
                return Ok(CommonReponse.CreateResponse(ResponseCodes.ErrorException, ex.Message, "null"));
            }
        }
        [HttpPut("update-order")]
        public async Task<IActionResult> UpdateOrder(CreateUpdateOrder createUpdateOrder, Guid idOrder)
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
        public async Task<IActionResult> DeleteOrder(Guid idOrder)
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