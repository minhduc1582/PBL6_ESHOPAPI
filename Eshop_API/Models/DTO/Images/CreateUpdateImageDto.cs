using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eshop_api.Models.DTO.Images
{
    public class CreateUpdateImageDto
    {
        public int Id{get;set;}
        public string Name{get;set;}
        public IFormFile Image{get;set;}
        public string Description{get;set;}
        public int ProductID{get;set;}
    }
}