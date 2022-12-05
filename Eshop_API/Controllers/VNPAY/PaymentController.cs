using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eshop_api.Controllers;
using Eshop_API.Services.VNPAY;
using Microsoft.AspNetCore.Mvc;

namespace Eshop_API.Controllers.VNPAY
{
    public class PaymentController : BaseController
    {
        private readonly IVnPayService _paymentService;
        public PaymentController(IVnPayService paymentService){
            _paymentService = paymentService;
        }
        [HttpGet("IPN")]
        public async Task<IActionResult> ResolvePayMent(){
            var queryString = System.Web.HttpUtility.ParseQueryString(HttpContext.Request.QueryString.Value);
            if(queryString.Count > 0){
                var result =  _paymentService.ChecksumReponse(queryString);
                return Ok(result);
            }
            else{
                return Ok(new {RspCode = "99",Message = "Input data required"});
            }
        }
    }
}