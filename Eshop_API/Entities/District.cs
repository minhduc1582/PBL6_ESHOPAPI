using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eshop_api.Entities
{
    public class District
    {
        public District(){
            Comunities  =  new List<Comunity>();
        }
        [Key]
        public int Id{get;set;}
        [Required]
        public string Name{get;set;}
        [ForeignKey("Province")]
        public int ProvinceId{get; set;}
        public virtual Province Province{get;set;}
        public virtual List<Comunity> Comunities{get;set;}
    }
}