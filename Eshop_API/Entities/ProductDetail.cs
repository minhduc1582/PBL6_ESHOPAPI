using System;

namespace eshop_api.Entities
{
    public class ProductDetail
    {
        public ProductDetail()
        {
            
        }
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int SizeId { get; set; }
        public int ColorId { get; set; }
        public string Object3D { get; set; }
    }
}

