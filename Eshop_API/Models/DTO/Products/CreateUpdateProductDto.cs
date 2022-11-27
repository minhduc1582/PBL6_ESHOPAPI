using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eshop_api.Models.DTO.Products
{
    public class CreateUpdateProductDto
    {
        public string Name{get;set;}
        public string Keyword{get;set;}
        public IFormFile AvtImage{get;set;}
        public double Price{get;set;}
        public double Discount{get;set;}
        public int ImportQuantity{get;set;} // Số lượng nhập kho
        public float Weight{get;set;}
        public string Description{get;set;}
        public string Color{get;set;}
        public Dictionary<string, object> Detail{get;set;}
        public IFormFileCollection ProductImages{get;set;}
        public int IdCategory{get;set;}
    }
}