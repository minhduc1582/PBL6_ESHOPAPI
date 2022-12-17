using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eshop_api.Entities;
using eshop_api.Helpers;
using eshop_api.Models.DTO.Images;
using Eshop_API.Models.DTO.Images;
using Eshop_API.Repositories.Images;

namespace eshop_api.Services.Images
{
    public class ImageService : IImageService
    {
        private readonly IImageRepository _imageRepository;

        public ImageService(IImageRepository imageRepository){
            _imageRepository = imageRepository;
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
            await _imageRepository.Add(image);
            await _imageRepository.SaveChangesAsync();
            return image;
        }

        public async Task<Image> AddImage(IFormFile image, int IdProduct)
        {
            Image img = new Image();
            img.Name = image.FileName;
            img.Url = CloudImage.UploadImage(image);
            img.ProductID = IdProduct;
            var result =await _imageRepository.Add(img);
            await _imageRepository.SaveChangesAsync();
            return result;
        }

        public async Task<bool> DeleteImageById(int Id)
        {
            Image image = await _imageRepository.FirstOrDefault(x => x.Id == Id);
            if(image != null){
                await _imageRepository.Remove(image);
                await _imageRepository.SaveChangesAsync();
                return true;
            }
            return false;
        }


        public async Task<List<Image>> GetImagesByIdProduct(int Id)
        {
            List<Image> image = await _imageRepository.Find(x => x.Id == Id);
            return image;
        }

        public async Task<Image> UpdateImage(UpdateImageDto updateImage)
        {
            Image image = await _imageRepository.FirstOrDefault(x => x.Id == updateImage.Id);
            if(updateImage.Name == null || updateImage.Name == "")
                image.Name = updateImage.Image.FileName;
            else
                image.Name = updateImage.Image.FileName;
            image.Description = updateImage.Description;
            image.ProductID = updateImage.ProductID;
            image.Url = CloudImage.UploadImage(updateImage.Image);
            await _imageRepository.Add(image);
            await _imageRepository.SaveChangesAsync();
            return image;
        }
    }
}