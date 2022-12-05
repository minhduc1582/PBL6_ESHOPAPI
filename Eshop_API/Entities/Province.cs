using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eshop_api.Entities
{
    public class Province
    {
        public Province(){
            Districts = new List<District>();
        }
        [Key]
        public int Id{get;set;}
        [Required]
        public string Name{get;set;}
        public virtual List<District> Districts{get;set;}
    }
}