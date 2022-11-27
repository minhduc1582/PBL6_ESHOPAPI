using eshop_api.Entities;
using eshop_api.Helpers;
using eshop_pbl6.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace eshop_api.Authorization
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string _roles;
        public AuthorizeAttribute(string roles)
        {
            _roles = roles ?? "";
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
            if (allowAnonymous)
                return;

            // authorization
            //var user = (User)context.HttpContext.Items["User"];
            var role = (List<string>)context.HttpContext.Items["Permissions"];
            if (role == null || (_roles.Any() && !role.Contains(_roles)))
            //if (user == null || (_roles.Any() && !_roles.Any(x => user.UserInRoles)))
            {
                // not logged in or role not authorized
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }
    }
}