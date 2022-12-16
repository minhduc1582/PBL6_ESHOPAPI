using eshop_api.Entities;
using Eshop_API.Entities;
using Eshop_API.Models.DTO.Adress;
using eshop_pbl6.Entities;

namespace eshop_pbl6.Services.Addresses
{
    public interface IAddressService
    {
        List<AddressView> GetListAddressByUser(int idUser);
        Task<List<AddressView>> GetAddressById(int idAddress);
        Task<Address> AddAddress(CreateUpdateAddress createUpdateAddress, int idUser);
        Task<Address> UpdateAddress(CreateUpdateAddress createUpdateAddress, int idAddress);
        Task<bool> DelAddress(int idAddress);
        List<Province> GetProvince();
        List<District> GetDistrict(int idProvince);
        List<Comunity> GetComunity(int idDistrict);
    }
}