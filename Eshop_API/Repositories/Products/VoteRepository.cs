using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eshop_api.Helpers;
using Eshop_API.Entities;
using Eshop_API.Repositories.Generics;

namespace Eshop_API.Repositories.Products
{
    public class VoteRepository : GenericRepository<Vote>, IVoteRepository
    {
        private readonly DataContext _context;
        public VoteRepository(DataContext context) : base(context)
        {
            _context = context;
        }
    }
}