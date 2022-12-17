using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eshop_pbl6.Helpers.Identities;

namespace Eshop_API.Services.Identities
{
    public interface IRoleService
    {
        Task<RoleEnum> GetRoleEnum(int IdRole);
    }
}