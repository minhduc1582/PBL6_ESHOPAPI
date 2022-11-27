using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace eshop_api.Entities
{
    public class OrderDetail
    {
        [Key]
        public int Id{get;set;}
        [ForeignKey("Order")]
        public int OrderId{get;set;}
        [ForeignKey("Product")]
        public int ProductId{get;set;}
        public int Quantity{get;set;}
        public string Note{get;set;}
        public Order Order{get;set;}
        public Product Product{get;set;}

    }
}