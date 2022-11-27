using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using eshop_api.Entities;
using eshop_api.Models.DTO.Products;

namespace eshop_api.Helpers.Mapper
{
    public static class ProductMapper
    {
        public static ProductDto toProductDto(Product product){
            ProductDto productDto = new ProductDto();
            productDto.Id = product.Id;
            productDto.Code = product.Code;
            productDto.Keyword = product.Keyword;
            productDto.Name = product.Name;
            productDto.AvtImageUrl = product.AvtImageUrl;
            productDto.Price = product.Price;
            productDto.Discount = (double)product.Discount;
            productDto.Weight = product.Weight;
            productDto.Description = product.Description;
            productDto.Color = product.Color;
            return productDto;
        }
    }
}