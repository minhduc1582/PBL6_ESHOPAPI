using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eshop_pbl6.Helpers.Products;

namespace eshop_api.Entities
{
    public class Product
    {
        public Product(){
            Images = new List<Image>();
            OrderDetails = new List<OrderDetail>();
            Comments = new List<Comment>();
            Detail = new Dictionary<string, object>();
        }

        [Key]
        public int Id{get;set;}
        #nullable enable annotations
        public string? Code{get;set;}
        public Status Status{get;set;} = Status.Pending;// trạng thái sản phẩm
        public string Name{get;set;}
        public string? Keyword{get;set;}
        public string? AvtImageUrl{get;set;}
        public double Price{get;set;}
        public double? Discount{get;set;}
        public int? ExportQuantity{get;set;}
        public int? ImportQuantity{get;set;}
        public float Weight{get;set;}
        public string? Description{get;set;}
        public string Color{get;set;}
        public bool IsDelete{get;set;}
        public Dictionary<string, object> Detail{get;set;} = new Dictionary<string, object>();
        [ForeignKey("Category")]
        public int CategoryId{get;set;}
        public virtual Category Category{get;set;}
        public virtual List<Image> Images{get;set;}
        public virtual List<OrderDetail> OrderDetails{get;set;}
        public virtual List<Comment> Comments{get;set;}
    }
}