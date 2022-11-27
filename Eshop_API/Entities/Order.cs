using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace eshop_api.Entities
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public string Status { get; set; }
        public double Total { get; set; }
        public string Note { get; set; }
        public string Check { get; set; }
        public string CheckedAt { get; set; }
        public string CheckedBy { get; set; }
        public string CheckedComment { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }
    }
}