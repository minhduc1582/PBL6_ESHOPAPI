using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using eshop_api.Authorization;
using eshop_api.Entities;
using eshop_api.Models.DTO.Products;
using eshop_api.Services.Products;
using eshop_pbl6.Helpers.Common;
using eshop_pbl6.Helpers.Identities;
using eshop_pbl6.Helpers.Products;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace eshop_api.Controllers.Products
{
    public class CategoryController : BaseController
    {
        delegate void TreeVisitor<T>(T nodeData,T data);
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [HttpGet("get-list-category")]
        [SwaggerOperation(Summary = "Get list category")]
        public IActionResult GetListCategory(){
            try{
            var result = _categoryService.GetListCategory();
            return Ok(CommonReponse.CreateResponse(ResponseCodes.Ok,"get dữ liệu thành công",result) );
            }
            catch(Exception ex){
                return Ok(CommonReponse.CreateResponse(ResponseCodes.ErrorException,ex.Message,"null") );
            }
            
        }
        /// <summary>
        /// Lấy danh sách category theo cây
        /// <para>Created by: MinhDuc</para>
        /// </summary>
        /// <response code="401">Lỗi chưa đăng nhập</response>
        /// <response code="500">Lỗi khi có exception</response>
        [HttpGet("get-list-tree-category")]
        public async Task<IActionResult> GetListTreeCategory(){
            try{
            Category category = new Category(){
                Id = -100,
                Name = "EShopCategory",
                ParentId = 0
            };
            var result = await _categoryService.GetListCategory();
            Node root = new Node{data = category};
            foreach(var item in result){
                TreeCategory.AddNode(root,item);
            }
            return Ok(CommonReponse.CreateResponse(ResponseCodes.Ok,"get dữ liệu thành công",root) );
            }
            catch(Exception ex){
                return Ok(CommonReponse.CreateResponse(ResponseCodes.ErrorException,ex.Message,"null") );
            }
            
        }
        /// <summary>
        /// Thêm danh mục sản phẩm
        /// <para>Created by: MinhDuc</para>
        /// </summary>
        /// <response code="500">Lỗi khi có exception</response>
        [HttpPost("create-category")]
        [Authorize(EshopPermissions.CategoryPermissions.Add)]
        public async Task<IActionResult> AddCategory(CreateUpdateCategory createCategory)
        {
            try
            {
                var result = await _categoryService.AddCategory(createCategory);
                return Ok(CommonReponse.CreateResponse(ResponseCodes.Ok,"thêm dữ liệu thành công",result) );
            }
            catch (Exception ex)
            {
                return Ok(CommonReponse.CreateResponse(ResponseCodes.ErrorException,ex.Message,"null"));
            }

        }
        [HttpPut("update-category")]
        public async Task<IActionResult> UpdateCategory(CreateUpdateCategory updateCategory,int IdCategory)
        {
            try
            {
                var result = await _categoryService.UpdateCategory(updateCategory,IdCategory);
                return Ok(CommonReponse.CreateResponse(ResponseCodes.Ok,"cập nhật dữ liệu thành công",result) );
            }
            catch (Exception ex)
            {
                return Ok(CommonReponse.CreateResponse(ResponseCodes.ErrorException,ex.Message,"null"));
            }

        }
        [HttpDelete("delete-category")]
        public async Task<IActionResult> DeleteCategory(int IdCategory){
            try
            {
                var check = await _categoryService.DeleteCateoryById(IdCategory);
                if(check){
                    return Ok(CommonReponse.CreateResponse(ResponseCodes.Ok,string.Empty,check));
                }
                return Ok(CommonReponse.CreateResponse(ResponseCodes.ErrorData,"Không tìm thấy dữ liệu",false));
            }
            catch (Exception ex)
            {
                return Ok(CommonReponse.CreateResponse(ResponseCodes.ErrorException,ex.Message,false));
            }
        }
    }
}