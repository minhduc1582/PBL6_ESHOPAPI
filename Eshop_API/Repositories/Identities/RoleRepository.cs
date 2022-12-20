using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eshop_api.Entities;
using eshop_api.Helpers;
using Eshop_API.Repositories.Generics;

namespace Eshop_API.Repositories.Identities
{
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        private readonly DataContext _context;
        public RoleRepository(DataContext context) : base(context)
        {
            _context = context;
        }
    }
}