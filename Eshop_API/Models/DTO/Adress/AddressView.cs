namespace Eshop_API.Models.DTO.Adress
{
    public class AddressView
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Phone { get; set; }
        public string ProvinceName { get; set; }
        public string DistrictName { get; set; }
        public string CommunityName { get; set; }
        public string address { get; set; }
        public bool IsDefault { get; set; }
    }

}
