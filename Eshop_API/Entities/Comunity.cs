using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace eshop_api.Entities
{
    public class Comunity
    {
        [Key]
        public int Id{get;set;}
        [Required]
        public string Name{get;set;}
        [ForeignKey("District")]
        public int DistrictId{get;set;}
        public virtual District District{get;set;}
    }
}