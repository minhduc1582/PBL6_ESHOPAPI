using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using eshop_api.Entities;
using Microsoft.IdentityModel.Tokens;

namespace eshop_pbl6.Authorization
{
    public interface IJwtUtils
    {
        public string GenerateJwtToken(User user);
        public string ValidateJwtToken(string token);
    }

    public class JwtUtils : IJwtUtils
    {
        private readonly IConfiguration _configuration;
        public JwtUtils(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateJwtToken(User user)
        {
            var claims = new List<Claim>(){
                new Claim("Id", user?.Id.ToString() ?? ""),
                new Claim(JwtRegisteredClaimNames.NameId, user.Username),
                new Claim("FirstName", user?.FirstName ?? ""),
                new Claim("LastName", user?.LastName ?? ""),
                new Claim("AvatarUrl", user?.AvatarUrl ?? ""),
                new Claim(JwtRegisteredClaimNames.Gender, user?.Gender.ToString() ?? ""),
                new Claim(JwtRegisteredClaimNames.Birthdate, user?.BirthDay.ToString() ?? ""),
                new Claim(JwtRegisteredClaimNames.Email, user?.Email)
            };
            var symetrickey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["TokenKey"])
            );
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = new SigningCredentials(
                        symetrickey, SecurityAlgorithms.HmacSha512Signature
                    )
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public string ValidateJwtToken(string token)
        {
            if (token == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["TokenKey"]);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var idUser = jwtToken.Claims.First(x => x.Type == "Id").Value;

                // return user id from JWT token if validation successful
                return idUser;
            }
            catch
            {
                // return null if validation fails
                return null;
            }
        }
    }
}