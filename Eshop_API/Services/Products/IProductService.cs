using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eshop_api.Models.DTO.Products;
using eshop_api.Entities;

namespace eshop_api.Service.Products
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetListProduct(int sortOrder);
        Task<List<ProductDto>> GetProductsByIdCategory(int idCategory, int sortOrder);
        Task<List<ProductDto>> GetProductsByName(string productName);
        Task<List<ProductDto>> FindProduct(string productName, int stockfirst, int stocklast, int idCategory, int idProduct);
        Task<ProductDto> AddProduct(CreateUpdateProductDto createProductDto);
        Task<Product> UpdateProduct(CreateUpdateProductDto createProductDto, int IdProduct);
        Task<bool> DeleteProductById(int id);
        Task<ProductDto> GetProductById(int IdProduct);
        Task<List<List<Product>>> GetBestSeller();
        Task<List<ProductDto>> GetListProductsByType(int type);
    }
}