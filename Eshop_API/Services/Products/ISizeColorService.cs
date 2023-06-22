using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eshop_pbl6.Entities;

namespace eshop_api.Service.Products
{
    public interface ISizeColorService
    {
        Task<List<Sizes>> GetAllSizes(); 
        Task<List<Colors>> GetAllColors(); 
    }
}