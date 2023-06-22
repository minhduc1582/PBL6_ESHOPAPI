using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eshop_pbl6.Entities
{
    public class Colors
    {
        [Key]
        public int Id{get;set;}
        public string Color {get;set;}
    }
}