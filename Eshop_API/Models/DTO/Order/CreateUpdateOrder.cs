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
        [Required]
        public List<OrderDetailDTO> listProduct {get; set;}
        [Required]
        [DisplayName("Price")]
        [RegularExpression(@"^\$?\d+(\.(\d{2}))?$",ErrorMessage = "Price is not valid")]
        [Range(0.000001,double.MaxValue, ErrorMessage = "Price greater than 0")]
        public double Total { get; set; }
        public string Note { get; set; }
        public string Check { get; set; }
        public DateTime CheckedAt { get; set; }
        public DateTime CreateAt {get;set;}
        public string CheckedBy { get; set; }
        public string CheckedComment { get; set; }
        [Required]
        public int UserId { get; set; }
    }
}