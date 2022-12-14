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
using Eshop_API.Helpers.Order;

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
        public async Task<IActionResult> GetListOrders([FromQuery] PagedAndSortedResultRequestDto input, bool getDetails)
        {
            try{
                var result = await _orderService.GetListOrders(getDetails);
                result.Where(x => input.Filter == "" || input.Filter == null || x.Id.ToString() == input.Filter);
                var page_list = PagedList<OrderView>.ToPagedList(result,
                        input.PageNumber,
                        input.PageSize);

                return Ok(CommonReponse.CreateResponse(ResponseCodes.Ok, "lấy dữ liệu thành công", page_list));
            }
            catch (Exception ex)
            {
                return Ok(CommonReponse.CreateResponse(ResponseCodes.ErrorException, ex.Message, "null"));
            }
        }
        [HttpGet("get-order-by-userid")]
        public async Task<IActionResult> GetOrdersByUserId([FromQuery] PagedAndSortedResultRequestDto input, int userId, bool getDetails)
        {
            try
            {
                List<OrderView> result = new List<OrderView>();
                if (userId == 0) result = await _orderService.GetListOrders(getDetails);
                else result = await _orderService.GetOrdersByUserId(userId, getDetails);
                result.Where(x => input.Filter == "" || input.Filter == null || x.Id.ToString() == input.Filter);
                var page_list = PagedList<OrderView>.ToPagedList(result,
                        input.PageNumber,
                        input.PageSize);
                return Ok(CommonReponse.CreateResponse(ResponseCodes.Ok, "lấy dữ liệu thành công", page_list));
            }
            catch (Exception ex)
            {
                return Ok(CommonReponse.CreateResponse(ResponseCodes.ErrorException, ex.Message, "null"));
            }
        }
        [HttpGet("get-order-by-id")]
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
        public async Task<IActionResult> GetOrdersByStatus([FromQuery] PagedAndSortedResultRequestDto input, int status, bool getDetails)
        {
            try
            {
                List<OrderView> result = new List<OrderView>();
                if (status == 0) result = await _orderService.GetListOrders(getDetails);
                else result = await _orderService.GetOrdersByStatus(status, getDetails);
                result.Where(x => input.Filter == "" || input.Filter == null || x.Id.ToString() == input.Filter);
                var page_list = PagedList<OrderView>.ToPagedList(result,
                        input.PageNumber,
                        input.PageSize);
                return Ok(CommonReponse.CreateResponse(ResponseCodes.Ok, "lấy dữ liệu thành công", page_list));
            }
            catch (Exception ex)
            {
                return Ok(CommonReponse.CreateResponse(ResponseCodes.ErrorException, ex.Message, "null"));
            }
        }
        [HttpGet("get-order-by-status-of-each-user")]
        [Authorize(EshopPermissions.OrderPermissions.Get)]
        public async Task<IActionResult> GetOrderByStatusOfEachUser([FromQuery] PagedAndSortedResultRequestDto input, int status, bool getDetails)
        {
            try
            {
                string remoteIpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
                //var serId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                var handler = new JwtSecurityTokenHandler();
                var jwtSecurityToken = handler.ReadJwtToken(token);
                var username = jwtSecurityToken.Claims.First(claim => claim.Type == "nameid").Value;
                var result = await _orderService.GetOrderByStatusOfEachUser(username, status, getDetails);
                result.Where(x => input.Filter == "" || input.Filter == null || x.Id.ToString() == input.Filter);
                var page_list = PagedList<OrderView>.ToPagedList(result,
                        input.PageNumber,
                        input.PageSize);
                return Ok(CommonReponse.CreateResponse(ResponseCodes.Ok, "lấy dữ liệu thành công", page_list));
            }
            catch (Exception ex)
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
                //var serId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                var handler = new JwtSecurityTokenHandler();
                var jwtSecurityToken = handler.ReadJwtToken(token);
                var username = jwtSecurityToken.Claims.First(claim => claim.Type == "nameid").Value;
                // var claimsIdentity = (ClaimsIdentity)User.Identity;
                // var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                // int userId = Convert.ToInt32(claim.Value);
                var result = await _orderService.AddOrder(orderDetailDTO, username, idAddress, payment, time,remoteIpAddress);
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
        public async Task<IActionResult> ChangeStatus(Guid idOrder, int status)
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