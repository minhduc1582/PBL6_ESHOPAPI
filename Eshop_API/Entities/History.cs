using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eshop_api.Entities
{
    public class History
    {
        public int Id{get;set;}
        [ForeignKey("User")]
        public int UserId{get;set;}
        public DateTime LoginAt{get;set;}
        public DateTime LogoutAt{get;set;}
        public DateTime ActionAt{get;set;}
        public string Action{get;set;}
        public User User{get;set;}
    }
}