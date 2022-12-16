using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eshop_api.Models.DTO.Order
{
    public class CreateUpdateOrderDetail
    {
        public Guid idOrder{get; set;}
        [Required]
        public int ProductId{get;set;}
        [Required]
        public int Quantity{get;set;}
        public string Note{get;set;}
    }
}