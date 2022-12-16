using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace eshop_api.Models.DTO.Order
{
    public class CreateUpdateOrder
    {
        [Required(ErrorMessage = "Danh sách sản phẩm không được bỏ trống")]
        public List<OrderDetailDTO> listProduct {get; set;}
        [Required(ErrorMessage = "Giá không được bỏ trống")]
        [DisplayName("Price")]
        [Range(0.000001,double.MaxValue, ErrorMessage = "Giá phải lớn hơn 0")]
        public double Total { get; set; }
        public string Note { get; set; }
        public string Check { get; set; }
        [DataType(DataType.Date,ErrorMessage = "CheckedAt không đúng định ngày")]
        public DateTime CheckedAt { get; set; }
        [DataType(DataType.Date,ErrorMessage = "CheckedAt không đúng định ngày")]
        public DateTime CreateAt {get;set;}
        public string CheckedBy { get; set; }
        public string CheckedComment { get; set; }
        public int UserId { get; set; }
    }
}