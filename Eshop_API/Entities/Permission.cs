using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace eshop_pbl6.Entities
{
    public class Permission
    {
        public Permission(){
            RoleInPermissions = new List<RoleInPermission>();
        }
        public int Id { get; set; }
        public string name { get; set; }
        [JsonIgnore]
        public List<RoleInPermission> RoleInPermissions{get;set;}
    }
}