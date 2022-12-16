using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eshop_api.Models.DTO.Products
{
    public class CreateUpdateCategory
    {
        [Required(ErrorMessage ="Tên danh mục không được bỏ trống")]
        [MaxLength(30,ErrorMessage = "Độ dài của tên phải bé hơn 30 ký tự")]
        public string Name{get;set;}
        public int ParentId{get;set;}
    }
}