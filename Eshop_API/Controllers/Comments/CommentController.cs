using eshop_api.Controllers;
using Eshop_API.Models.DTO.Comments;
using Eshop_API.Services.Comments;
using eshop_pbl6.Helpers.Common;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Net.WebSockets;

namespace Eshop_API.Controllers.Comments
{
    public class CommentController : BaseController
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet("get-comment-by-product")]
        public async Task<IActionResult>GetCommentByProduct(int idProduct)
        {
            try
            {
                var result = await _commentService.GetProductComment(idProduct);
                return Ok(CommonReponse.CreateResponse(ResponseCodes.Ok, "lấy dữ liệu thành công", result));
            }
            catch (Exception ex)
            {
                return Ok(CommonReponse.CreateResponse(ResponseCodes.ErrorException, ex.Message, "null"));
            }
        }
        [HttpPost("add-comment")]
        public async Task<IActionResult> AddComment(CreateUpdateComment createUpdateComment)
        {
            try { 
                var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                var handler = new JwtSecurityTokenHandler();
                var jwtSecurityToken = handler.ReadJwtToken(token);
                string idUser = jwtSecurityToken.Claims.First(claim => claim.Type == "Id").Value;

                var result = await _commentService.AddComment(createUpdateComment, int.Parse(idUser));
                return Ok(CommonReponse.CreateResponse(ResponseCodes.Ok, "lấy dữ liệu thành công", result));
            }
            catch (Exception ex)
            {
                return Ok(CommonReponse.CreateResponse(ResponseCodes.ErrorException, ex.Message, "null"));
            }
        }
        [HttpPut("Edit Comment")]
        public async Task<IActionResult> EditComment(CreateUpdateComment createUpdateComment, int idComment)
        {
            try
            {
                var result = await _commentService.EditComment(createUpdateComment, idComment);
                return Ok(CommonReponse.CreateResponse(ResponseCodes.Ok, "lấy dữ liệu thành công", result));
            }
            catch (Exception ex)
            {
                return Ok(CommonReponse.CreateResponse(ResponseCodes.ErrorException, ex.Message, "null"));
            }
        }
        [HttpDelete("delete-comment")]
        public async Task<IActionResult> DelComment(int idComment)
        {
            try
            {
                var result = await _commentService.DeleteComment(idComment);
                return Ok(CommonReponse.CreateResponse(ResponseCodes.Ok, "lấy dữ liệu thành công", result));
            }
            catch (Exception ex)
            {
                return Ok(CommonReponse.CreateResponse(ResponseCodes.ErrorException, ex.Message, "null"));
            }
        }
    }
}
