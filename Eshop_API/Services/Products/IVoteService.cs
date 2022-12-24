using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eshop_API.Models.DTO.Products;

namespace Eshop_API.Services.Products
{
    public interface IVoteService
    {
        Task<VoteDto> VoteProduct(CreateUpdateVoteDto createUpdateVoteDto,int userId);
        Task<float> GetVoteByProductID(int ProductId);
    }
}