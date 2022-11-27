using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eshop_api.Entities
{
    public class Comment
    {

        [Key]
        public int Id{get;set;}
        public string CommentUser{get;set;}
        public int ParentId{get;set;}
        [ForeignKey("User")]
        public int UserId{get;set;}
        [ForeignKey("Product")]
        public int ProductId{get;set;}
        public User User{get;set;}
        public Product Product{get;set;}
    }
}