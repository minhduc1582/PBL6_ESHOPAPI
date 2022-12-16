using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eshop_API.Models.DTO.Adress
{
    public class CreateUpdateAddress
    {
        [RegularExpression(@"^(\+[0-9]{9})$",ErrorMessage = "Not Right Format Phone Number")]
        public string Phone { get; set; }
        public string ProvinceName { get; set; }
        public string DistrictName { get; set; }
        public string CommunityName { get; set; }
        public string address { get; set; }
        public bool IsDefault { get; set; }
    }
}
