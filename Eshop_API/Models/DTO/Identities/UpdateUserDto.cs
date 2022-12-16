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
        [EmailAddress]
        public string Email { get; set; }
        [MaxLength(30)]
        public string FirstName { get; set; }
        [MaxLength(60)]
        public string LastName { get; set; }
        [RegularExpression(@"^(\+[0-9]{9})$")]
        public string Phone { get; set; }
        [DataType(DataType.Upload)]
        [FileExtensions(Extensions ="jpg,png,gif,jpeg,bmp,svg",ErrorMessage = "Not Right Format Image")]
        public IFormFile AvatarUrl { get; set; }
        [DataType(DataType.Date)]
        public DateTime BirthDay { get; set; }
        public GenderEnum Gender { get; set; }
    }
}