using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eshop_API.Repositories.Identities;
using eshop_pbl6.Helpers.Identities;

namespace Eshop_API.Services.Identities
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }
        public async Task<RoleEnum> GetRoleEnum(int IdRole)
        {
            var role = (await _roleRepository.FirstOrDefault(x=> x.Id == IdRole)).Name;
            return role;
        }
    }
}