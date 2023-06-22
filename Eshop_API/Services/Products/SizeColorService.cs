using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eshop_api.Helpers;
using eshop_pbl6.Entities;

namespace eshop_api.Service.Products
{
    public class SizeColorService : ISizeColorService
    {
        private readonly DataContext _context;
        public SizeColorService(DataContext context){
            _context = context;
        }
        public async Task<List<Colors>> GetAllColors()
        {
            return _context.Colors.ToList();
        }

        public async Task<List<Sizes>> GetAllSizes()
        {
            return _context.Sizes.ToList();
        }
    }
}