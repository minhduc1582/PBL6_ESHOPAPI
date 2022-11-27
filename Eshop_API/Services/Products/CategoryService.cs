using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eshop_api.Entities;
using eshop_api.Helpers;
using eshop_api.Models.DTO.Products;

namespace eshop_api.Services.Products
{
    public class CategoryService : ICategoryService
    {
        private readonly DataContext _context;

        public CategoryService(DataContext context)
        {
            _context = context;
        }
        public async Task<Category> AddCategory(CreateUpdateCategory createCategory)
        {
            Category category = new Category();
            category.Name = createCategory.Name;
            category.ParentId = createCategory.ParentId;
            var result = await _context.AddAsync(category);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<bool> DeleteCateoryById(int Id)
        {
            var category = _context.Categories.FirstOrDefault(x => x.Id == Id);
            if (category != null)
            {
                var result = _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public List<Category> GetListCategory()
        {
            return _context.Categories.ToList();
        }

        public async Task<Category> UpdateCategory(CreateUpdateCategory updateCategory, int IdCategory)
        {
            var category = _context.Categories.FirstOrDefault(x => x.Id == IdCategory);
            if (category != null)
            {
                category.Name = updateCategory.Name;
                category.ParentId = updateCategory.ParentId;
                var result = _context.Update(category);
                await _context.SaveChangesAsync();
                return result.Entity;
            }
            else
            {
                category = new Category();
                category.Name = updateCategory.Name;
                category.ParentId = updateCategory.ParentId;
                var result = await _context.AddAsync(category);
                await _context.SaveChangesAsync();
                return result.Entity;
            }
        }
    }
}