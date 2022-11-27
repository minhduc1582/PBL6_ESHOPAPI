using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using eshop_api.Entities;
using Microsoft.IdentityModel.Tokens;

namespace eshop_api.Services.Identities
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string CreateToken(User user)
        {
            var claims = new List<Claim>(){
                new Claim(JwtRegisteredClaimNames.NameId, user.Username),
                new Claim("FirstName", user.FirstName ?? ""),
                new Claim("LastName", user.LastName ?? ""),
                new Claim("Code", user.Code ?? ""),
                new Claim("Phone1", user.Phone1 ?? ""),
                new Claim("Address1", user.Address1 ?? ""),
                new Claim("Phone2", user.Phone2 ?? ""),
                new Claim("Address2", user.Address2 ?? ""),
                new Claim("AvatarUrl", user.AvatarUrl ?? ""),
                new Claim(JwtRegisteredClaimNames.Gender, user.Gender.ToString() ?? ""),
                new Claim(JwtRegisteredClaimNames.Birthdate, user.BirthDay.ToString() ?? ""),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            };
            var symetrickey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["TokenKey"])
            );
            var tokenDescriptor = new SecurityTokenDescriptor{
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = new SigningCredentials(
                        symetrickey,SecurityAlgorithms.HmacSha512Signature
                    )
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}