using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eshop_API.Models.DTO.Images
{
    public class UpdateImageDto
    {
        [Required(ErrorMessage = "Id không được bỏ trống")]
        public int Id{get;set;}
        public string Name{get;set;}
        [Required(ErrorMessage = "Ảnh không được bỏ trống")]
        [DataType(DataType.Upload,ErrorMessage = "Ảnh không đúng định dạng")]
        // [FileExtensions(Extensions ="jpg,png,gif,jpeg,bmp,svg",ErrorMessage = "Ảnh không đúng định dạng")]
        public IFormFile Image{get;set;}
        public string Description{get;set;}
        [Required(ErrorMessage = "ProductId không được bỏ trống")]
        public int ProductID{get;set;}
    }
}