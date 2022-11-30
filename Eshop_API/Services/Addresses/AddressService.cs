using eshop_api.Entities;
using eshop_api.Helpers;
using Eshop_API.Entities;
using Eshop_API.Helpers.Mapper;
using Eshop_API.Models.DTO.Adress;
using eshop_pbl6.Entities;
using System.ComponentModel.DataAnnotations;

namespace eshop_pbl6.Services.Addresses
{
    public class AddressService : IAddressService
    {
        private readonly DataContext _context;

        public AddressService(DataContext context)
        {
            _context = context;
        }

        public List<Province> GetProvince()
        {
            return _context.Provinces.ToList();
        }
        public List<District> GetDistrict(int idProvince)
        {
            return _context.Dictricts.Where(x => x.ProvinceId == idProvince).ToList();
        }
        public List<Comunity> GetComunity(int idDistrict)
        {
            return _context.Comunities.Where(x => x.DistrictId == idDistrict).ToList();
        }

        public List<CreateUpdateAddress> GetListAddressByUser(string username)
        {
            int userId = _context.AppUsers.FirstOrDefault(x => x.Username == username).Id;
            List<CreateUpdateAddress> addresses = new List<CreateUpdateAddress>();
            List<Address> list = _context.Addresses.Where(x => x.UserId == userId).ToList();
            foreach(Address i in list)
            {
                addresses.Add(new CreateUpdateAddress
                {
                    Phone = i.Phone,
                    ProvinceName = _context.Provinces.FirstOrDefault(x => x.Id == i.ProvinceId).Name,
                    DistrictName = _context.Dictricts.FirstOrDefault(x => x.Id == i.DistrictId).Name,
                    CommunityName = _context.Comunities.FirstOrDefault(x => x.Id == i.CommunityId).Name,
                    address = i.address,
                    IsDefault = i.IsDefault
                });
            }
            return addresses;
        }

        public async Task<List<CreateUpdateAddress>> GetAddressById(int idAddress)
        {
            var address = _context.Addresses.FirstOrDefault(x => x.Id == idAddress);
            List<CreateUpdateAddress> addresses = new List<CreateUpdateAddress>();
            addresses.Add(new CreateUpdateAddress
            {
                Phone = address.Phone,
                ProvinceName = _context.Provinces.FirstOrDefault(x => x.Id == address.ProvinceId).Name,
                DistrictName = _context.Dictricts.FirstOrDefault(x => x.Id == address.DistrictId).Name,
                CommunityName = _context.Comunities.FirstOrDefault(x => x.Id == address.CommunityId).Name,
                address = address.address,
                IsDefault = address.IsDefault
            });
            return await Task.FromResult(addresses);
        }

        public async Task<Address> AddAddress(CreateUpdateAddress createUpdateAddress, string username)
        {
            int userId = _context.AppUsers.FirstOrDefault(x => x.Username == username).Id;
            int provinceId = _context.Provinces.FirstOrDefault(x => x.Name == createUpdateAddress.ProvinceName).Id;
            int districtId = _context.Dictricts.FirstOrDefault(x => x.Name == createUpdateAddress.DistrictName).Id;
            int communityId = _context.Comunities.FirstOrDefault(x => x.Name == createUpdateAddress.CommunityName).Id;
            Address address = new Address();
            address.UserId = userId;
            address.Phone = createUpdateAddress.Phone;
            address.ProvinceId = provinceId;
            address.DistrictId = districtId;
            address.CommunityId = communityId;
            address.address = createUpdateAddress.address;
            address.IsDefault = createUpdateAddress.IsDefault;
            var result = _context.Addresses.AddAsync(address);
            await _context.SaveChangesAsync();
            return address;
            
        }

        public async Task<Address> UpdateAddress(CreateUpdateAddress createUpdateAddress, int idAddress)
        {
            int provinceId = _context.Provinces.FirstOrDefault(x => x.Name == createUpdateAddress.ProvinceName).Id;
            int districtId = _context.Dictricts.FirstOrDefault(x => x.Name == createUpdateAddress.DistrictName).Id;
            int communityId = _context.Comunities.FirstOrDefault(x => x.Name == createUpdateAddress.CommunityName).Id;
            var address = _context.Addresses.FirstOrDefault(x => x.Id == idAddress);
            if(address!=null)
            {
                address.Phone = createUpdateAddress.Phone;
                address.ProvinceId = provinceId;
                address.DistrictId = districtId;
                address.CommunityId = communityId;
                address.address = createUpdateAddress.address;
                address.IsDefault = createUpdateAddress.IsDefault;
                var result = _context.Addresses.Update(address);
                await _context.SaveChangesAsync();
                return result.Entity;
            }
            else
            {
                throw null;
            }    
        }

        public async Task<bool> DelAddress(int idAddress)
        {
            var address = _context.Addresses.FirstOrDefault(x => x.Id == idAddress);
            if(address != null)
            {
                var result = _context.Addresses.Remove(address);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}