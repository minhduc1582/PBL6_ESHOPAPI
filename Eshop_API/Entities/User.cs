using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eshop_pbl6.Helpers.Identities;
using System.Text.Json.Serialization;

namespace eshop_api.Entities
{
    public class User
    {
        public User()
        {
            Comments = new List<Comment>();
            Orders = new List<Order>();
            Histories = new List<History>();
        }
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(32)]
        public string Username { get; set; }
        [Required]
        [StringLength(255),EmailAddress]
        public string Email { get; set; }
        public string Code { get; set; }
        [JsonIgnore]
        public byte[] PasswordHash { get; set; }
        [JsonIgnore]
        public byte[] PasswordSalt { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string AvatarUrl { get; set; }
        public DateTime BirthDay { get; set; }
        public GenderEnum Gender { get; set; }
        [ForeignKey("Role")]
        public int RoleId{get;set;}
        public List<Comment> Comments { get; set; }
        public List<Order> Orders { get; set; }
        public List<History> Histories { get; set; }
    }
}