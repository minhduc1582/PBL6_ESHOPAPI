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
        public int Id{get;set;}
        public string TnxRef{get;set;}
        public string TransactionNo{get;set;}
        public string Amount{get;set;}
        public string PayDate{get;set;}
        public string OrderInfo{get;set;}
        [ForeignKey("Order")]
        public int IdOrder{get;set;}
        public string BankCode{get;set;}
        public int Status{get;set;} // watting,
    }
}