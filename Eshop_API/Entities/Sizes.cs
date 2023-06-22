using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eshop_pbl6.Entities
{
    public class Sizes
    {
        [Key]
        public int Id{get;set;}
        public string Size{get;set;}
    }
}