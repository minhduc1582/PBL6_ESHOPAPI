using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eshop_pbl6.Helpers.Identities;

namespace eshop_pbl6.Models.DTO.Identities
{
    public class UpdateUserDto
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string AvatarUrl { get; set; }
        public DateTime BirthDay { get; set; }
        public GenderEnum Gender { get; set; }
    }
}