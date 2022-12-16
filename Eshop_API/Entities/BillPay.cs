using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Eshop_API.Helpers.Orders;

namespace eshop_api.Entities
{
    public class BillPay
    {
        [Key]
        [ForeignKey("Order")]
        public Guid TnxRef{get;set;}
        public string TransactionNo{get;set;}
        public string Amount{get;set;}
        public string PayDate{get;set;}
        public string OrderInfo{get;set;}
        public string BankCode{get;set;}
        [Required]
        public PaymentStatus Status{get;set;} // watting,
        [JsonIgnore]
        public virtual Order Order {get;set;}
    }
}