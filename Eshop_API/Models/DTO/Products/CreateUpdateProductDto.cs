using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eshop_api.Models.DTO.Products
{
    public class CreateUpdateProductDto
    {
        [Required(ErrorMessage = "Tên sản phẩm không được bỏ trống")]
        [MaxLength(512,ErrorMessage = "Độ dài của tên phải bé hơn 512 ký tự")]
        public string Name{get;set;}
        public string Keyword{get;set;}
        [Required(ErrorMessage = "Ảnh product đại diện không được bỏ trống")]
        [DataType(DataType.Upload,ErrorMessage = "Ảnh không đúng định dạng")]
        public IFormFile AvtImage{get;set;}
        [Required]
        [Range(1,double.MaxValue, ErrorMessage = "giá không được thấp hơn hăọc bằng 0 VND")]
        public double Price{get;set;}
        [Range(0,100, ErrorMessage = "Discount phải từ 0 đến 100%")]
        public double Discount{get;set;}
        [Range(1,int.MaxValue)]
        public int ImportQuantity{get;set;} // Số lượng nhập kho
        [Range(0,10, ErrorMessage = "Cân nặng phải từ 0 đến 10kg")]
        public float Weight{get;set;}
        public string Description{get;set;}
        [MaxLength(20)]
        public string Color{get;set;}
        public string Detail{get;set;}
        [Required]
        [DataType(DataType.Upload,ErrorMessage = "Ảnh không đúng định dạng")]
        public IFormFileCollection ProductImages{get;set;}
        [Required]
        public int IdCategory{get;set;}
    }
}