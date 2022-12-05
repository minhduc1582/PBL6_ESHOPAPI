using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Eshop_API.Entities
{
    public class BillPay
    {
        [Key]
        [ForeignKey("Order")]
        public string TnxRef{get;set;}
        public string TransactionNo{get;set;}
        public string Amount{get;set;}
        public string PayDate{get;set;}
        public string OrderInfo{get;set;}
        public int IdOrder{get;set;}
        public string BankCode{get;set;}
        public int Status{get;set;} // watting,
    }
}