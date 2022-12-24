using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Eshop_API.Entities;
using Eshop_API.Models.DTO.Products;
using Eshop_API.Repositories.Products;

namespace Eshop_API.Services.Products
{
    public class VoteService : IVoteService
    {
        private readonly IMapper _mapper;
        private readonly IVoteRepository _voteRepository;
        public VoteService(IVoteRepository voteRepository,
                            IMapper mapper){
            _voteRepository = voteRepository;
            _mapper = mapper;
        }
        public async Task<float> GetVoteByProductID(int productId)
        {
            var votes = await _voteRepository.Find(x => x.ProductId == productId);
            if(votes != null && votes.Count >= 0){
                return votes.Sum(x => x.Star)/votes.Count;
            }
            return 0;

        }

        public async Task<VoteDto> VoteProduct(CreateUpdateVoteDto createUpdateVoteDto,int userId)
        {
            var vote = await _voteRepository.FirstOrDefault(x => x.ProductId == createUpdateVoteDto.ProductId && x.UserId == userId);
            Vote createVote = new Vote();
            if(vote == null){
                createVote.ProductId= createUpdateVoteDto.ProductId;
                createVote.UserId = userId;
                createVote.Star = createUpdateVoteDto.Star;
                await _voteRepository.Add(createVote);
            }
            else{
                vote.Star = createUpdateVoteDto.Star;
                createVote = await _voteRepository.Update(vote);
            }
            await _voteRepository.SaveChangesAsync();
            return _mapper.Map<Vote,VoteDto>(createVote);
        }
    }
}