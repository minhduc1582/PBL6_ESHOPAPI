using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Eshop_API.Entities
{
    public class Vote
    {
        [Required]
        [Range(0,5,ErrorMessage = "Số sao phải từ 0 - 5")]
        public float Star {get;set;}
        [Required]
        [ForeignKey("User")]
        public int UserId {get;set;}
        [ForeignKey("Product")]
        [Required]
        public int ProductId{get;set;}
    }
}