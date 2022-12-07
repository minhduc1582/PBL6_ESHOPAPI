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
using Nest;
using Sentry;

namespace eshop_api.Controllers.Products
{
    public class ProductController : BaseController
    {
        private IHubContext <MessageHub,IMessageHubClient> _messageHub;
        private readonly DataContext _context;
        private readonly IProductService _productService;
        ILogger<ProductController> _logger;
        private readonly IHub _sentryHub;
        //private readonly IElasticClient _elasticClient;
        public ProductController(DataContext context,
                                IProductService productService,
                                IHubContext <MessageHub,IMessageHubClient> messageHub,
                                ILogger<ProductController> logger,
                                IHub sentryHub)
        {
            _context = context;
            _productService = productService;
            _messageHub = messageHub;
            _logger = logger;
            _sentryHub = sentryHub;
        }
        [HttpGet("get-list-product")]
        public async Task<ActionResult> GetListProduct([FromQuery]PagedAndSortedResultRequestDto input, int sortOrder){
                var childSpan = _sentryHub.GetSpan()?.StartChild("additional-work");

            try{
                 _logger.LogInformation("GetListProduct Get - Begin Get list", DateTime.UtcNow);
                var result = await _productService.GetListProduct(sortOrder);
                result.Where(x => input.Filter == "" || input.Filter == null || x.Name == input.Filter);
                var page_list =  PagedList<ProductDto>.ToPagedList(result,
                        input.PageNumber,
                        input.PageSize);
               childSpan?.Finish(SpanStatus.Ok);
                _logger.LogInformation("GetListProduct Get - Get Success", DateTime.UtcNow);
                return Ok(CommonReponse.CreateResponse(ResponseCodes.Ok,"get dữ liệu thành công",page_list) );
            }
            catch(Exception ex){
                SentrySdk.CaptureException(ex);
                childSpan?.Finish(ex);
                 _logger.LogInformation("GetListProduct Get - Get ERR Exeption: " + ex.Message, DateTime.UtcNow);
                return Ok(CommonReponse.CreateResponse(ResponseCodes.Ok,ex.Message,"null") );
            }
        }
        [HttpGet("get-list-product-by-id-category")]
        public async Task<ActionResult>  GetListProductByIdCategory([FromQuery]PagedAndSortedResultRequestDto input, int idCategory, int sortOrder){
            try{
                var result = await _productService.GetProductsByIdCategory(idCategory, sortOrder);
                result.Where(x => input.Filter == "" || input.Filter == null || x.Name == input.Filter);
                var page_list  = PagedList<ProductDto>.ToPagedList(result.OrderBy(on => on.Name),
                        input.PageNumber,
                        input.PageSize);
                return Ok(CommonReponse.CreateResponse(ResponseCodes.Ok,"get dữ liệu thành công",page_list) );
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
                var page_list  = PagedList<ProductDto>.ToPagedList(result.OrderBy(on => on.Name),
                        input.PageNumber,
                        input.PageSize);
                return Ok(CommonReponse.CreateResponse(ResponseCodes.Ok,"get dữ liệu thành công",page_list) );
            }
            catch(Exception ex){
                return Ok(CommonReponse.CreateResponse(ResponseCodes.Ok,ex.Message,"null") );
            }
        }
        [HttpGet("get-list-product-by-name")]
        public async Task<ActionResult>  GetListProductByName([FromQuery]PagedAndSortedResultRequestDto input, string productName,int sortOrder){
            try{
                var result = await _productService.GetProductsByName(productName);
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
        public async Task<ActionResult>  FindProduct([FromQuery]PagedAndSortedResultRequestDto input, string productName, int stockfirst, int stocklast, int idCategory, int idProduct){
            try{
                var result = await _productService.FindProduct(productName,stockfirst,stocklast,idCategory, idProduct);
                result.Where(x => input.Filter == "" || input.Filter == null || x.Name == input.Filter);
                var page_list = PagedList<ProductDto>.ToPagedList(result.OrderBy(on => on.Name),
                        input.PageNumber,
                        input.PageSize);
                return Ok(CommonReponse.CreateResponse(ResponseCodes.Ok,"get dữ liệu thành công",page_list) );
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
                    if(result != null){
                       // await _elasticClient.IndexDocumentAsync(createProductDto);
                        return Ok(CommonReponse.CreateResponse(ResponseCodes.Ok,"Thêm dữ liệu thành công",result));
                    }
                    
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
        [HttpGet("get-undefined")]
        public async Task<ActionResult> TestSlack([FromQuery]PagedAndSortedResultRequestDto input, int sortOrder){
                var childSpan = _sentryHub.GetSpan()?.StartChild("additional-work");

            try{
                _logger.LogInformation("Get Undefined - Begin Get list", DateTime.UtcNow);
                throw new NullReferenceException();
            }
            catch(Exception ex){
                SentrySdk.CaptureException(ex);
                childSpan?.Finish(ex);
                 _logger.LogInformation("Get Undefined - Get ERR Exeption: " + ex.Message, DateTime.UtcNow);
                return Ok(CommonReponse.CreateResponse(ResponseCodes.Ok,ex.Message,"null") );
            }
        }
    }
}