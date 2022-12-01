using System.ComponentModel.DataAnnotations.Schema;

namespace Eshop_API.Models.DTO.Adress
{
    public class AddressDTO
    {
        public int UserId { get; set; }
        public string Phone { get; set; }
        public int ProvinceId { get; set; }
        public int DistrictId { get; set; }
        public int CommunityId { get; set; }
        public string address { get; set; }
        public bool IsDefault { get; set; }
    }
}
