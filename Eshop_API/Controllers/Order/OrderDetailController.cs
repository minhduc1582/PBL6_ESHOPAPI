using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eshop_api.Services.Orders;
using Microsoft.AspNetCore.Mvc;
using eshop_api.Models.DTO.Order;
using eshop_pbl6.Helpers.Common;
using eshop_api.Helpers;

namespace eshop_api.Controllers.Order
{
    public class OrderDetailController : BaseController
    {
        private readonly IOderDetailService _orderDetailService;
        public OrderDetailController(IOderDetailService oderDetailService)
        {
            _orderDetailService = oderDetailService;
        }
        [HttpGet("get-order-detail-by-id")]
        public IActionResult GetOrderDetailsById(int idOrderDetail)
        {
            try{
                var result = _orderDetailService.GetOrderDetailsById(idOrderDetail);
                return Ok(CommonReponse.CreateResponse(ResponseCodes.Ok, "lấy dữ liệu thành công", result));
            }
            catch(Exception ex)
            {
                return Ok(CommonReponse.CreateResponse(ResponseCodes.ErrorException, ex.Message, "null"));
            }
        }
        [HttpGet("get-order-detail-by-order-id")]
        public IActionResult GetOrderDetailsByOrderId(int idOrder)
        {
            try{
                var result = _orderDetailService.GetOrderDetailsByOrderId(idOrder);
                return Ok(CommonReponse.CreateResponse(ResponseCodes.Ok, "lấy dữ liệu thành công", result));
            }
            catch(Exception ex)
            {
                return Ok(CommonReponse.CreateResponse(ResponseCodes.ErrorException, ex.Message, "null"));
            }
        }
        [HttpPost("add-order-detail")]
        public async Task<IActionResult> AddOrderDetail(CreateUpdateOrderDetail createUpdateOrderDetail)
        {
            try{
                var result = await _orderDetailService.AddOrderDetail(createUpdateOrderDetail);
                return Ok(CommonReponse.CreateResponse(ResponseCodes.Ok, "thêm dữ liệu thành công", result));
            }
            catch(Exception ex)
            {
                return Ok(CommonReponse.CreateResponse(ResponseCodes.ErrorException, ex.Message, "null"));
            }
        }
        [HttpPut("update-order-detail")]
        public async Task<IActionResult> UpdateOrderDetail(CreateUpdateOrderDetail createUpdateOrderDetail, int idOrderDetail)
        {
            try{
                var result = await _orderDetailService.UpdateOrderDetail(createUpdateOrderDetail, idOrderDetail);
                return Ok(CommonReponse.CreateResponse(ResponseCodes.Ok, "cập nhật dữ liệu thành công", result));
            }
            catch(Exception ex)
            {
                return Ok(CommonReponse.CreateResponse(ResponseCodes.ErrorException, ex.Message, "null"));
            }
        }
        [HttpDelete("delete-order-detail")]
        public async Task<IActionResult> DeleteOrderDetail(int idOrderDetail)
        {
            try{
                var result = await _orderDetailService.DeleteOrderDetail(idOrderDetail);
                return Ok(CommonReponse.CreateResponse(ResponseCodes.Ok, "xóa dữ liệu thành công", result));
            }
            catch(Exception ex)
            {
                return Ok(CommonReponse.CreateResponse(ResponseCodes.ErrorException, ex.Message, "null"));
            }
        }
    }
}