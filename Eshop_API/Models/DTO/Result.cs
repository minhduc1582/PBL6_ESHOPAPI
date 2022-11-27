using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eshop_api.Models.DTO
{
    public class Result
    {
        public Result(){
            Results = new Dictionary<string, object>();
        }
        public Dictionary<string,object> Results{get;set;}
    }
}