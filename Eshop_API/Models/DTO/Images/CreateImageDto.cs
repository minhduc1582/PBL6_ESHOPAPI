using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eshop_api.Models.DTO.Images
{
    public class CreateImageDto
    {
        [MaxLength(64,ErrorMessage = "Độ dài của tên phải bé hơn 64 ký tự")]
        public string Name{get;set;}
        [Required]
        [DataType(DataType.Upload,ErrorMessage = "Ảnh không đúng định dạng")]
        [FileExtensions(Extensions ="jpg,png,gif,jpeg,bmp,svg",ErrorMessage = "Ảnh không đúng định dạng")]
        public IFormFile Image{get;set;}
        public string Description{get;set;}
        [Required]
        public int ProductID{get;set;}
    }
}