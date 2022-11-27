using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eshop_api.Models.DTO.Products
{
    public class CreateUpdateCategory
    {
        public string Name{get;set;}
        public int ParentId{get;set;}
    }
}