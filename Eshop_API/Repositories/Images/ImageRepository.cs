using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eshop_api.Entities;
using eshop_api.Helpers;
using Eshop_API.Repositories.Generics;

namespace Eshop_API.Repositories.Images
{
    public class ImageRepository: GenericRepository<Image>, IImageRepository
    {
        private readonly DataContext _context;
        public ImageRepository(DataContext context) : base(context)
        {
            _context = context;
        }
    }
}