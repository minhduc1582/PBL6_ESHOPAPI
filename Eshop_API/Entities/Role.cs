using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eshop_pbl6.Helpers.Identities;
using eshop_pbl6.Entities;
using System.Text.Json.Serialization;

namespace eshop_api.Entities
{
    public class Role
    {
        public Role(){
            RoleInPermissions = new List<RoleInPermission>();
            Users = new List<User>();
        }
        [Key]
        public int Id{get;set;}
        public RoleEnum Name{get;set;}
        [JsonIgnore]
        public virtual List<RoleInPermission> RoleInPermissions{get;set;}
        [JsonIgnore]
        public virtual List<User> Users{get;set;}
    }
}