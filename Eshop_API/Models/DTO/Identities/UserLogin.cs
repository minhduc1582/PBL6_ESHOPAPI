using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace eshop_pbl6.Models.DTO.Identities
{
    public class UserLogin
    {
        [Required]
        [MaxLength(256)]
        public string Username {get; set;}

        [Required]
        [MaxLength(256)]
        public string Password {get; set;}
    }
}