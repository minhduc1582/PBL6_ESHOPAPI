using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eshop_pbl6.Helpers.Identities
{
    public class CreateUpdateUserDto
    {
        [Required]
        [StringLength(50)]
        public string Username { get; set; }
        [Required]
        [StringLength(255),EmailAddress]
        public string Email { get; set; }
        [Required]
        [MaxLength(60),MinLength(6)]
        public string Password{get;set;}
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [RegularExpression(@"^(\+[0-9]{9})$",ErrorMessage = "Not Right Format Phone Number")]
        public string Phone { get; set; }
        [DataType(DataType.Upload)]
        [FileExtensions(Extensions ="jpg,png,gif,jpeg,bmp,svg")]
        public IFormFile Avatar { get; set; }
        public DateTime BirthDay { get; set; }
        public GenderEnum Gender { get; set; }
    }
}