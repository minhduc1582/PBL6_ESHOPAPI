using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using eshop_api.Helpers;

namespace eshop_api.Models.DTO.Products
{
    public class ProductDto
    {
        public int Id{get;set;}
        public string Code{get;set;}
        public string Name{get;set;}
        public string Keyword{get;set;}
        public string AvtImageUrl{get;set;}
        public double Price{get;set;}
        public double Discount{get;set;}
        public int? ImportQuantity{get;set;}
        public float Weight{get;set;}
        public string DetailProduct{get;set;}
        public string Description{get;set;}
        public string Color{get;set;}
        public List<string> ImageUrl{get;set;} = new List<string>();
        //public Dictionary<string, object> Detail{get;set;} = new Dictionary<string, object>();
    }
}