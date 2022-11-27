using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eshop_pbl6.Helpers.Identities
{
    public class CreateUpdateUser
    {
        [Required]
        [StringLength(50)]
        public string Username { get; set; }
        [Required]
        [StringLength(255),EmailAddress]
        public string Email { get; set; }
        public string Code { get; set; }
        public string Password{get;set;}
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone1 { get; set; }
        public string Address1 { get; set; }
        public IFormFile Avatar { get; set; }
        public DateTime BirthDay { get; set; }
        public GenderEnum Gender { get; set; }
    }
}