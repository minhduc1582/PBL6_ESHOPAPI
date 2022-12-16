using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eshop_api.Entities;
using eshop_api.Helpers;
using eshop_api.Models.DTO.Images;
using Eshop_API.Models.DTO.Images;

namespace eshop_api.Services.Images
{
    public class ImageService : IImageService
    {
        private readonly DataContext _context;

        public ImageService(DataContext context){
            _context = context;
        }
        public async Task<Image> AddImage(CreateImageDto createImage)
        {
            Image image = new Image();
            if(createImage.Name == null || createImage.Name == "")
                image.Name = createImage.Image.FileName;
            else
                image.Name = createImage.Image.FileName;
            image.Description = createImage.Description;
            image.ProductID = createImage.ProductID;
            image.Url = CloudImage.UploadImage(createImage.Image);
            _context.Images.Add(image);
            await _context.SaveChangesAsync();
            return image;
        }

        public async Task<Image> AddImage(IFormFile image, int IdProduct)
        {
            Image img = new Image();
            img.Name = image.FileName;
            img.Url = CloudImage.UploadImage(image);
            img.ProductID = IdProduct;
            var result =await _context.Images.AddAsync(img);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<bool> DeleteImageById(int Id)
        {
            Image image = _context.Images.FirstOrDefault(x => x.Id == Id);
            if(image != null){
                _context.Images.Remove(image);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }


        public Task<List<Image>> GetImagesByIdProduct(int Id)
        {
            List<Image> image = _context.Images.Where(x => x.Id == Id).ToList();
            return Task.FromResult(image);
        }

        public async Task<Image> UpdateImage(UpdateImageDto updateImage)
        {
            Image image = _context.Images.FirstOrDefault(x => x.Id == updateImage.Id);
            if(updateImage.Name == null || updateImage.Name == "")
                image.Name = updateImage.Image.FileName;
            else
                image.Name = updateImage.Image.FileName;
            image.Description = updateImage.Description;
            image.ProductID = updateImage.ProductID;
            image.Url = CloudImage.UploadImage(updateImage.Image);
            _context.Images.Add(image);
            await _context.SaveChangesAsync();
            return image;
        }
    }
}