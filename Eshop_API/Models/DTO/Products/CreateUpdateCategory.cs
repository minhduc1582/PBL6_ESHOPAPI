using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eshop_api.Models.DTO.Products
{
    public class CreateUpdateCategory
    {
        [Required]
        [MaxLength(30)]
        public string Name{get;set;}
        public int ParentId{get;set;}
    }
}