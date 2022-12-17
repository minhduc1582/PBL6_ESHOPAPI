using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Eshop_API.Entities;
using Eshop_API.Helpers.Orders;
using System.Text.Json.Serialization;

namespace eshop_api.Entities
{
    public class Order
    {
        [Key]
        #nullable disable
        public Guid Id { get; set; } =  Guid.NewGuid();// TNXREF
        public string Status { get; set; }
        public double Total { get; set; }
        public string Note { get; set; }
        public DateTime CreateAt {get;set;}
        public DateTime CheckedAt { get; set; }
        public string CheckedBy { get; set; }
        public string CheckedComment { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        [ForeignKey("Address")]
        public int? AddressId { get; set; }
        public int DeliveryTime { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        [JsonIgnore]
        public virtual Address Address { get; set; }
        [JsonIgnore]
        public virtual User User { get; set; }
        [JsonIgnore]
        public virtual BillPay BillPay{get;set;}
    }
}