using Eshop_API.Entities;
using Eshop_API.Models.DTO.Adress;

namespace Eshop_API.Helpers.Mapper
{
    public class AddressMapper
    {
        public static AddressDTO toAddressDTO(Address address)
        {
            AddressDTO addressDTO = new AddressDTO();
            addressDTO.UserId = address.UserId;
            addressDTO.ProvinceId = address.ProvinceId;
            addressDTO.DistrictId = address.DistrictId;
            addressDTO.CommunityId = address.CommunityId;
            addressDTO.address = address.address;
            addressDTO.Phone = address.Phone;
            addressDTO.IsDefault = address.IsDefault;
            return addressDTO;
        }
    }
}
