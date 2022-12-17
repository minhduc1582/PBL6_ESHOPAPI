using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eshop_pbl6.Helpers.Identities;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace eshop_pbl6.Models.DTO.Identities
{
    public class UpdateUserDto
    {
        [StringLength(255,ErrorMessage ="Độ dài của email không quá 255 ký tự"),EmailAddress(ErrorMessage ="Email không đúng định dạng")]
        public string Email { get; set; }
        [MaxLength(30,ErrorMessage ="Độ dài của FirstName phải bé hơn 30 ký tự")]
        public string FirstName { get; set; }
        [MaxLength(60,ErrorMessage ="Độ dài của LastName phải bé hơn 30 ký tự")]
        public string LastName { get; set; }
        [RegularExpression(@"(84|0[3|5|7|8|9])+([0-9]{8})\b", ErrorMessage ="Số điện thoại không đúng định dạng")]
        public string Phone { get; set; }
        [DataType(DataType.Upload, ErrorMessage ="Ảnh không đúng định dạng")]
        // [FileExtensions(Extensions ="jpg,png,gif,jpeg,bmp,svg",ErrorMessage = "Ảnh không đúng định dạng")]
        public IFormFile AvatarUrl { get; set; }
        [DataType(DataType.Date,ErrorMessage ="Ngày sinh không đúng định dạng")]
        public DateTime BirthDay { get; set; }
        [Range(minimum:0, maximum:2, ErrorMessage = "Giới tính không hợp lệ")]
        public GenderEnum Gender { get; set; }
    }
}