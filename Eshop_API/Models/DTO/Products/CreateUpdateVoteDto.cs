using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eshop_API.Models.DTO.Products
{
    public class CreateUpdateVoteDto
    {
        [Required]
        [Range(0,5,ErrorMessage = "Số sao phải từ 0 - 5")]
        public float Star {get;set;}
        [Required]
        public int ProductId{get;set;}
    }
}