using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eshop_api.Authorization;
using eshop_api.Entities;
using eshop_api.Helpers;
using eshop_api.Models.DTO.Products;
using eshop_api.Service.Products;
using eshop_pbl6.Helpers.Common;
using eshop_pbl6.Helpers.Identities;
using eshop_pbl6.Services.Hub;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace eshop_api.Controllers.Products
{
    public class ProductController : BaseController
    {
        private IHubContext <MessageHub,IMessageHubClient> _messageHub;
        private readonly DataContext _context;
        private readonly IProductService _productService;

        public ProductController(DataContext context,
                                IProductService productService,
                                IHubContext <MessageHub,IMessageHubClient> messageHub)
        {
            _context = context;
            _productService = productService;
            _messageHub = messageHub;
        }
        [HttpGet("get-list-product")]
        public async Task<ActionResult> GetListProduct([FromQuery]PagedAndSortedResultRequestDto input, int sortOrder){
            try{
                var result = await _productService.GetListProduct(sortOrder);
                result.Where(x => input.Filter == "" || input.Filter == null || x.Name == input.Filter);
                result = PagedList<ProductDto>.ToPagedList(result,
                        input.PageNumber,
                        input.PageSize);
                return Ok(CommonReponse.CreateResponse(ResponseCodes.Ok,"get dữ liệu thành công",result) );
            }
            catch(Exception ex){
                return Ok(CommonReponse.CreateResponse(ResponseCodes.Ok,ex.Message,"null") );
            }
        }
        [HttpGet("get-list-product-by-id-category")]
        public async Task<ActionResult>  GetListProductByIdCategory([FromQuery]PagedAndSortedResultRequestDto input, int idCategory, int sortOrder){
            try{
                var result = await _productService.GetProductsByIdCategory(idCategory, sortOrder);
                result.Where(x => input.Filter == "" || input.Filter == null || x.Name == input.Filter);
                result = PagedList<ProductDto>.ToPagedList(result.OrderBy(on => on.Name),
                        input.PageNumber,
                        input.PageSize);
                return Ok(CommonReponse.CreateResponse(ResponseCodes.Ok,"get dữ liệu thành công",result) );
            }
            catch(Exception ex){
                return Ok(CommonReponse.CreateResponse(ResponseCodes.Ok,ex.Message,"null") );
            }
        }
        [HttpGet("get-list-product-by-id")]
        public async Task<ActionResult>  GetListProductById([FromQuery]PagedAndSortedResultRequestDto input, int idProduct){
            try{
                var result = await _productService.GetProductsById(idProduct);
                result.Where(x => input.Filter == "" || input.Filter == null || x.Name == input.Filter);
                result = PagedList<ProductDto>.ToPagedList(result.OrderBy(on => on.Name),
                        input.PageNumber,
                        input.PageSize);
                return Ok(CommonReponse.CreateResponse(ResponseCodes.Ok,"get dữ liệu thành công",result) );
            }
            catch(Exception ex){
                return Ok(CommonReponse.CreateResponse(ResponseCodes.Ok,ex.Message,"null") );
            }
        }
        [HttpGet("get-list-product-by-name")]
        public async Task<ActionResult>  GetListProductByName([FromQuery]PagedAndSortedResultRequestDto input, string productName){
            try{
                var result = await _productService.GetListProduct(1);
                // result = result.Where(x => input.Filter == "" || input.Filter == null || x.Name.Contains(input.Filter)).ToList();
                // var paging_result = PagedList<ProductDto>.ToPagedList(result.OrderBy(on => on.Name),
                //         input.PageNumber,
                //         input.PageSize);
                return Ok(CommonReponse.CreateResponse(ResponseCodes.Ok,"get dữ liệu thành công",result) );
            }
            catch(Exception ex){
                return Ok(CommonReponse.CreateResponse(ResponseCodes.Ok,ex.Message,"null") );
            }
        }
        [HttpGet("find-product")]
        public async Task<ActionResult>  FindProduct([FromQuery]PagedAndSortedResultRequestDto input, string productName, int stockfirst, int stocklast, int idCategory){
            try{
                var result = await _productService.FindProduct(productName,stockfirst,stocklast,idCategory);
                result.Where(x => input.Filter == "" || input.Filter == null || x.Name == input.Filter);
                result = PagedList<ProductDto>.ToPagedList(result.OrderBy(on => on.Name),
                        input.PageNumber,
                        input.PageSize);
                return Ok(CommonReponse.CreateResponse(ResponseCodes.Ok,"get dữ liệu thành công",result) );
            }
            catch(Exception ex){
                return Ok(CommonReponse.CreateResponse(ResponseCodes.Ok,ex.Message,"null") );
            }
        }
        [HttpPost("add-product")]
        [Authorize(EshopPermissions.ProductPermissions.Add)]
        public async Task<IActionResult> AddProduct([FromForm]CreateUpdateProductDto createProductDto){
            try{
                if(createProductDto != null){
                    
                    var result  = await _productService.AddProduct(createProductDto);
                    if(result != null)
                    return Ok(CommonReponse.CreateResponse(ResponseCodes.Ok,"Thêm dữ liệu thành công",result));
                }
                return Ok(CommonReponse.CreateResponse(ResponseCodes.BadRequest,"Thêm dữ liệu thất bại","null"));
            }
            catch(Exception ex){
                return Ok(CommonReponse.CreateResponse(ResponseCodes.ErrorException,ex.Message,"null"));
            }
        }
        [HttpGet("get-product-by-id")]
        public async Task<IActionResult> GetProductById(int IdProduct){
            try{
                    var result  = await _productService.GetProductById(IdProduct);
                    if(result != null)
                        return Ok(CommonReponse.CreateResponse(ResponseCodes.Ok,"Lấy dữ liệu thành công",result));
                    return Ok(CommonReponse.CreateResponse(ResponseCodes.ErrorData,"Thêm dữ liệu thất bại","null"));
            }
            catch(Exception ex){
                return Ok(CommonReponse.CreateResponse(ResponseCodes.ErrorException,ex.Message,"null"));
            }
        }
        [HttpDelete("delete-product-by-id")]
        [Authorize(EshopPermissions.ProductPermissions.Delete)]
        public async Task<IActionResult> DelProductById(int idProduct){
            try{
                var result = await _productService.DeleteProductById(idProduct);
                if(result == true)
                        return Ok(CommonReponse.CreateResponse(ResponseCodes.Ok,"Lấy dữ liệu thành công",result));
                return Ok(CommonReponse.CreateResponse(ResponseCodes.ErrorData,"Thêm dữ liệu thất bại","null"));
            }
            catch(Exception ex){
                return Ok(CommonReponse.CreateResponse(ResponseCodes.ErrorException,ex.Message,"null"));
            }
        }
    }
}