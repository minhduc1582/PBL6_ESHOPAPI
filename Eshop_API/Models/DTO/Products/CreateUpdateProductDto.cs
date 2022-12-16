using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eshop_api.Models.DTO.Products
{
    public class CreateUpdateProductDto
    {
        [Required]
        public string Name{get;set;}
        public string Keyword{get;set;}
        [Required]
        [DataType(DataType.Upload)]
        [FileExtensions(Extensions ="jpg,png,gif,jpeg,bmp,svg")]
        public IFormFile AvtImage{get;set;}
        [Required]
        [Range(0.000001,double.MaxValue, ErrorMessage = "Price greater than 0 VND")]
        public double Price{get;set;}
        [Range(0,100, ErrorMessage = "Discount is from 0 to 100 percent")]
        public double Discount{get;set;}
        [Range(0.000001,int.MaxValue)]
        public int ImportQuantity{get;set;} // Số lượng nhập kho
        [Range(0,10, ErrorMessage = "Weight is from 0 to 10kg")]
        public float Weight{get;set;}
        public string Description{get;set;}
        [MaxLength(20)]
        public string Color{get;set;}
        [RegularExpression("{('\\w+':(\\d+|'\\w+'|true|false|null))+}\\]",ErrorMessage = "Not Right Format Json")]
        public Dictionary<string, object> Detail{get;set;}
        [Required]
        [DataType(DataType.Upload)]
        [FileExtensions(Extensions ="jpg,png,gif,jpeg,bmp,svg")]
        public IFormFileCollection ProductImages{get;set;}
        [Required]
        public int IdCategory{get;set;}
    }
}