using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eshop_api.Entities;
using eshop_api.Helpers;
using Newtonsoft.Json;

namespace eshop_api.Models.DTO.Products
{
    public class CategoryTreeDto
    {
        [JsonProperty("data")]
        public Category data{get;set;}
        public List<CategoryTreeDto> child{get;set;} = new List<CategoryTreeDto>();
    }
}