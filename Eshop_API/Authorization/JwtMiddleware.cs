using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eshop_pbl6.Helpers.Identities;
using eshop_pbl6.Services.Identities;
using Microsoft.Extensions.Options;

namespace eshop_pbl6.Authorization
{
    public class JwtMiddleware
    {
        private readonly IConfiguration _configuration;
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task Invoke(HttpContext context, IUserService userService, IJwtUtils jwtUtils)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            string idUser = jwtUtils.ValidateJwtToken(token);
            if (!string.IsNullOrEmpty(idUser))
            {
                // attach user to context on successful jwt validation
                //context.Items["User"] = await userService.GetByUserName(username);
                context.Items["Permissions"] = await userService.GetPermissionByUser(int.Parse(idUser));
            }

            await _next(context);
        }
    }
}