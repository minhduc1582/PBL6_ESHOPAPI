using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using eshop_api.Entities;

namespace eshop_pbl6.Entities
{
    public class RoleInPermission
    {
        [ForeignKey("Role")]
        public int RoleId { get; set; }
        [ForeignKey("Permission")]
        public int PermissionId { get; set; }
        public Role Role { get; set; }
        public Permission Permission { get; set; }
    }
}