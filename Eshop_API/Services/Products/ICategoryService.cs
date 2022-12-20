using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eshop_api.Entities;
using eshop_api.Models.DTO.Products;

namespace eshop_api.Services.Products
{
    public interface ICategoryService
    {
        Task<List<Category>> GetListCategory();
        Task<Category> AddCategory(CreateUpdateCategory createCategory);
        Task<Category> UpdateCategory(CreateUpdateCategory updateCategory,int IdCategory);
        Task<bool> DeleteCateoryById(int Id);
    }
}