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
        Task<bool> Login(UserLogin userLogin);
        Task<User> GetById(int idUser);
        Task<UserDto> UpdateUserById(UpdateUserDto userDto,int idUser);
        Task<bool> ChangePassword(string passwordOld,string passwordNew);
        Task<List<string>> GetPermissionByUser(int idUser);
        Task<List<Permission>> GetAllPermission();
        Task<List<RoleDto>> GetAllRoles();
        Task<bool> ChangePassworrd(string username,string passwordOld, string passwordNew);
    }
}