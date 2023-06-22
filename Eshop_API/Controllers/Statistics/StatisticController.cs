using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eshop_api.Controllers;
using eshop_api.Helpers;
using eshop_pbl6.Helpers.Common;
using Microsoft.AspNetCore.Mvc;

namespace Eshop_API.Controllers.Statistics
{
    public class StatisticController : BaseController
    {
        private readonly DataContext _context;
        public StatisticController(DataContext context){
            _context = context;
        }
        [HttpGet("statistics")]
        public async Task<IActionResult> Statistics(){
            try
            {
                var user = _context.AppUsers.ToList();
                var product = _context.Products.OrderByDescending(x => x.ExportQuantity).ToList();
                var order = _context.Orders.OrderByDescending(y => y.Total).ToList();   
                return Ok(CommonReponse.CreateResponse(ResponseCodes.Ok,"Lấy dữ liệu thành công",new List<object>(){user,product,order}));
            }
            catch (Exception ex)
            {
                return Ok(CommonReponse.CreateResponse(ResponseCodes.ErrorException,ex.Message,"null"));
            }
        }
    }
}