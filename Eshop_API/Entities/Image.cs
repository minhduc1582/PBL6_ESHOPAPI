using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eshop_api.Entities
{
    public class Image
    {
        [Key]
        public int Id{get;set;}
        #nullable enable annotations
        public string? Code{get;set;}
        public string Name{get;set;}
        public string Url{get;set;}
        #nullable enable annotations
        public string? Description{get;set;}
        public int ProductID{get;set;}
        public Product Product{get;set;}
    }
}