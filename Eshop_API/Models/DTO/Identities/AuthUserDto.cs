using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace eshop_api.Models.DTO
{
    public class AuthUserDto
    {
        [Required]
        [MaxLength(256)]
        public string Username {get; set;}

        [Required]
        [MaxLength(256)]
        public string Password {get; set;}

        [EmailAddress]
        public string Email{get; set;}
    }
}