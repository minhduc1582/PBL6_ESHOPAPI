using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eshop_API.Models.DTO.VNPAY
{
    public class ModelPayDto
    {
        private double _amount;
        public double Amount{ 
            get => _amount; 
            set{_amount = value*100;}
        }
        public string Content{get;set;}
        public string Email{get;set;}
    }
}