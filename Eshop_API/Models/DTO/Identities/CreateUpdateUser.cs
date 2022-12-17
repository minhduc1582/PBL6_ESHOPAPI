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
        [StringLength(50, ErrorMessage = "Độ dài của Username phải bé hơn 50 ký tự")]
        public string Username { get; set; }
        [Required]
        [StringLength(255,ErrorMessage ="Độ dài của email không quá 255 ký tự"),EmailAddress(ErrorMessage ="Email không đúng định dạng")]
        public string Email { get; set; }
        [Required]
        [MaxLength(60,ErrorMessage ="Password phải nhỏ hơn 60 kí tự"),MinLength(6,ErrorMessage ="Password phải lớn hơn 6 kí tự")]
        public string Password{get;set;}
        [StringLength(255,ErrorMessage ="Độ dài của FirstName không được quá 255 ký tự")]
        public string FirstName { get; set; }
        [StringLength(255,ErrorMessage ="Độ dài của LastName không được quá 255 ký tự")]
        public string LastName { get; set; }
        [RegularExpression(@"(84|0[3|5|7|8|9])+([0-9]{8})\b",ErrorMessage = "Số điện thoại không đúng định dạng")]
        public string Phone { get; set; }
        [DataType(DataType.Upload,ErrorMessage = "Ảnh không đúng định dạng")]
        public IFormFile Avatar { get; set; }
        public DateTime BirthDay { get; set; }
        public GenderEnum Gender { get; set; }
    }
}