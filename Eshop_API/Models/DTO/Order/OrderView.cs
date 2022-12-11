using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eshop_api.Entities;
using Eshop_API.Entities;
using Eshop_API.Models.DTO.Adress;

namespace eshop_api.Models.DTO.Order
{
    public class OrderView
    {
        public Guid Id { get; set; }
        public string Status { get; set; }
        public double Total { get; set; }
        public string Note { get; set; }
        public string Check { get; set; }
        public string CheckedAt { get; set; }
        public string CheckedBy { get; set; }
        public string CheckedComment { get; set; }
        public int UserId { get; set; }
        public List<OrderDetailDTOs> list {get; set;}
        public List<AddressView> address { get; set; }
        public string Payment { get; set; }
        public string Time { get; set; }
    }
}