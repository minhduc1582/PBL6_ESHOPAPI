using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eshop_api.Entities
{
    public class Category
    {
        public Category(){
            Products = new List<Product>();
        }
        [Key]
        public int Id{get;set;}
        #nullable enable
        public string? Code{get;set;}
        public string Name{get;set;}
        public int? ParentId{get;set;}
        public virtual List<Product> Products{get;set;}
    }
}