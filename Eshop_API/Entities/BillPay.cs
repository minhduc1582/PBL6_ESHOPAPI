using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Eshop_API.Helpers.Order;

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
        public PaymentStatus Status{get;set;} // watting,
        public virtual Order Order {get;set;}
    }
}