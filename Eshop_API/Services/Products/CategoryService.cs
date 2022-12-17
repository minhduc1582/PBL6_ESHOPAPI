using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eshop_api.Entities;
using eshop_api.Helpers;
using eshop_api.Models.DTO.Products;
using Eshop_API.Repositories.Products;

namespace eshop_api.Services.Products
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public async Task<Category> AddCategory(CreateUpdateCategory createCategory)
        {
            Category category = new Category();
            category.Name = createCategory.Name;
            category.ParentId = createCategory.ParentId;
            var result = await _categoryRepository.Add(category);
            await _categoryRepository.SaveChangesAsync();
            return result;
        }

        public async Task<bool> DeleteCateoryById(int Id)
        {
            var category = await _categoryRepository.FirstOrDefault(x => x.Id == Id);
            if (category != null)
            {
                var result = _categoryRepository.Remove(category);
                await _categoryRepository.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<List<Category>> GetListCategory()
        {
            return await _categoryRepository.GetAll();
        }

        public async Task<Category> UpdateCategory(CreateUpdateCategory updateCategory, int IdCategory)
        {
            var category = await _categoryRepository.FirstOrDefault(x => x.Id == IdCategory);
            if (category != null)
            {
                category.Name = updateCategory.Name;
                category.ParentId = updateCategory.ParentId;
                var result = await _categoryRepository.Update(category);
                await _categoryRepository.SaveChangesAsync();
                return result;
            }
            else
            {
                category = new Category();
                category.Name = updateCategory.Name;
                category.ParentId = updateCategory.ParentId;
                var result = await _categoryRepository.Add(category);
                await _categoryRepository.SaveChangesAsync();
                return result;
            }
        }
    }
}