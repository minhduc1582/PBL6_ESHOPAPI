using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eshop_pbl6.Helpers.Identities;

namespace eshop_pbl6.Models.DTO.Identities
{
    public class RoleDto
    {
        private RoleEnum _role{get;set;}
        public int Id{get;set;}
        public string Name{
        get{ return _role.ToString();}
        set{ _role = (RoleEnum)Enum.Parse(typeof(RoleEnum), value);}
        }
    }
}