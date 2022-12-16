using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eshop_API.Models.DTO.Adress
{
    public class CreateUpdateAddress
    {
        [RegularExpression(@"(84|0[3|5|7|8|9])+([0-9]{8})\b",ErrorMessage = "Số điện thoại không đúng định dạng")]
        public string Phone { get; set; }
        [MaxLength(24, ErrorMessage = "Độ dài của tỉnh phải bé hơn 24 kí tự")]
        public string ProvinceName { get; set; }
        [MaxLength(52, ErrorMessage = "Độ dài của huyện phải bé hơn 52 kí tự")]
        public string DistrictName { get; set; }
        [MaxLength(52, ErrorMessage = "Độ dài của xã phải bé hơn 52 kí tự")]
        public string CommunityName { get; set; }
        [MaxLength(216, ErrorMessage = "Độ dài của xã phải bé hơn 216 kí tự")]
        public string address { get; set; }
        public bool IsDefault { get; set; }
    }
}
