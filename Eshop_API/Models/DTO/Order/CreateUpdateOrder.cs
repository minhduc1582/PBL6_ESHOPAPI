using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eshop_api.Models.DTO.Order
{
    public class CreateUpdateOrder
    {
        public List<OrderDetailDTO> listpro {get; set;}
        
        public double Total { get; set; }
        public string Note { get; set; }
        public string Check { get; set; }
        public string CheckedAt { get; set; }
        public string CheckedBy { get; set; }
        public string CheckedComment { get; set; }
        public int UserId { get; set; }
    }
}