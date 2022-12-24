using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using eshop_api.Authorization;
using eshop_api.Controllers;
using Eshop_API.Models.DTO.Products;
using Eshop_API.Services.Products;
using eshop_pbl6.Helpers.Common;
using eshop_pbl6.Helpers.Identities;
using Microsoft.AspNetCore.Mvc;

namespace Eshop_API.Controllers.Products
{
    public class VoteController : BaseController
    {
        private readonly IVoteService _voteService;
        public VoteController(IVoteService voteService){
            _voteService = voteService;
        }

        [HttpPost("vote-product")]
        [Authorize(EshopPermissions.Votes.Add)]
        public async Task<IActionResult> VoteProduct(CreateUpdateVoteDto createUpdateVoteDto){
            try
            {
                var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                var handler = new JwtSecurityTokenHandler();
                var jwtSecurityToken = handler.ReadJwtToken(token);
                string idUser = jwtSecurityToken.Claims.First(claim => claim.Type == "Id").Value;
                var vote = await _voteService.VoteProduct(createUpdateVoteDto,int.Parse(idUser));
                return Ok(CommonReponse.CreateResponse(ResponseCodes.Ok,"get dữ liệu thành công",vote));
            }
            catch (Exception ex)
            {
                return Ok(CommonReponse.CreateResponse(ResponseCodes.ErrorException,ex.Message,"null"));
            }
        }

        [HttpPost("get-votes-by-idproduct")]
        [Authorize(EshopPermissions.Votes.Get)]
        public async Task<IActionResult> GetVoteProduct(int IdProduct){
            try
            {
                var vote = await _voteService.GetVoteByProductID(IdProduct);
                return Ok(CommonReponse.CreateResponse(ResponseCodes.Ok,"get dữ liệu thành công",vote));
            }
            catch (Exception ex)
            {
                return Ok(CommonReponse.CreateResponse(ResponseCodes.ErrorException,ex.Message,"null"));
            }
        }
    }
}