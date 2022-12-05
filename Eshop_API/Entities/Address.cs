using eshop_api.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eshop_API.Entities
{
    public class Address
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        public string Phone { get; set; }
        [ForeignKey("Province")]
        public int ProvinceId { get; set; }
        [ForeignKey("District")]
        public int DistrictId { get; set; }
        [ForeignKey("Comunity")]
        public int CommunityId { get; set; }
        public string address { get; set; }
        public bool IsDefault { get; set; }
        public virtual User user { get; set; }
        public virtual Province province { get; set; }
        public virtual District district { get; set; }
        public virtual Comunity comunity { get; set; }
    }
}
