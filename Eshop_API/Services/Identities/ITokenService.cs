using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eshop_api.Entities;

namespace eshop_api.Services.Identities
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}