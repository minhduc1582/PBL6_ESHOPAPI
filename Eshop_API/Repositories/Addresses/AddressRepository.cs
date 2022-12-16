using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eshop_api.Helpers;
using Eshop_API.Entities;
using Eshop_API.Repositories.Generics;

namespace Eshop_API.Repositories.Addresses
{
    public class AddressRepository : GenericRepository<Address>, IAddressRepository
    {
        private readonly DataContext _context;
        public AddressRepository(DataContext context) : base(context)
        {
            _context = context;
        }
    }
}