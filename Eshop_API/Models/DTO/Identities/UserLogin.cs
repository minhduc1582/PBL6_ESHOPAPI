using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace eshop_pbl6.Models.DTO.Identities
{
    public class UserLogin
    {
        /// <summary>
        /// Tên người dùng
        /// </summary>
        [Required]
        [MaxLength(30)]
        public string Username {get; set;}

        /// <summary>
        /// Mật khẩu
        /// </summary>
        [Required]
        [MaxLength(60)]
        [DataType(DataType.Password)]
        public string Password {get; set;}
    }
}