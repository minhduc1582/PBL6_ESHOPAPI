using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eshop_API.Models.DTO.VNPAY
{
    public class ModelPayDto
    {
        private double _amount;
        [Required]
        [Range(0.000001,double.MaxValue, ErrorMessage = "Price greater than 0 VND")]
        public double Amount{ 
            get => _amount; 
            set{_amount = value*100;}
        }
        [Required]
        [MaxLength(512)]
        public string Content{get;set;}
        [EmailAddress]
        [Required]
        public string Email{get;set;}
        [Required]
        [MaxLength(40)]
        public string Name{get;set;}
        [Required]
        public Guid Tnx_Ref{get;set;}
        public string UrlOrigin{get;set;}
    }
}