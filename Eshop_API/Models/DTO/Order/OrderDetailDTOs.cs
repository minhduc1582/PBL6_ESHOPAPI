using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace eshop_api.Models.DTO.Order
{
    public class OrderDetailDTOs
    {
         public int Id{get;set;}
        public int OrderId{get;set;}
        public int ProductId{get;set;}
        public string AvtImageUrl { get; set; }
        public string ProductName{get; set;}
        public double Price{get; set;}
        public int Quantity{get;set;}
        public string Note{get;set;}

    }
}