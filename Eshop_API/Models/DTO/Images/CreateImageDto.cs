using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eshop_api.Models.DTO.Images
{
    public class CreateImageDto
    {
        public string Name{get;set;}
        [Required]
        [DataType(DataType.Upload)]
        [FileExtensions(Extensions ="jpg,png,gif,jpeg,bmp,svg")]
        public IFormFile Image{get;set;}
        public string Description{get;set;}
        [Required]
        public int ProductID{get;set;}
    }
}