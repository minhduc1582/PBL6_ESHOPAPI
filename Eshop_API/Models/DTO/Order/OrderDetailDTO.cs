using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eshop_api.Models.DTO.Order
{
    public class OrderDetailDTO
    {
        [Required(ErrorMessage = "ProductId không được bỏ trống")]
        public int ProductId{get;set;}
        [Required(ErrorMessage = "Quantity không được bỏ trống")]
        [Range(1,1000000,ErrorMessage = "Số lượng sản phẩm từ 1 đến 1000000")]
        public int Quantity{get;set;}
        public string Note{get;set;}
    }
}