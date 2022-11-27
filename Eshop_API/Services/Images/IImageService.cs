using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eshop_api.Entities;
using eshop_api.Models.DTO.Images;

namespace eshop_api.Services.Images
{
    public interface IImageService
    {
        Task<List<Image>> GetImagesByIdProduct(int code);
        Task<Image> AddImage(CreateUpdateImageDto image);
        Task<bool> DeleteImageById(int Id);
        Task<Image> UpdateImage(CreateUpdateImageDto image);
        Task<Image> AddImage(IFormFile image,int IdProduct);
    }
}