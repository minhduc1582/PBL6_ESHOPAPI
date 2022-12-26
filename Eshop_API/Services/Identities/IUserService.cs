using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eshop_api.Entities;
using eshop_pbl6.Entities;
using eshop_pbl6.Helpers.Identities;
using eshop_pbl6.Models.DTO.Identities;

namespace eshop_pbl6.Services.Identities
{
    public interface IUserService
    {
        Task<UserDto> GetUsersDto();
        Task<User> Register(CreateUpdateUserDto create);
        Task<bool> Login(string username, string password);
        Task<User> GetById(int idUser);
        Task<UserDto> UpdateUserById(UpdateUserDto userDto,int idUser);
        Task<List<string>> GetPermissionByUser(int idUser);
        Task<List<Permission>> GetAllPermission();
        Task<List<RoleDto>> GetAllRoles();
        Task<bool> ChangePassword(string username,string passwordOld, string passwordNew);
    }
}