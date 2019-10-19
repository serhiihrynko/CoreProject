using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Extensions;
using Domain.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace API.Middlewares
{
    public class RoleProviderMiddleware
    {
        private readonly RequestDelegate _requestDelegate;

        public RoleProviderMiddleware(RequestDelegate requestDelegate)
        {
            _requestDelegate = requestDelegate;
        }


        public async Task Invoke(HttpContext httpContext, UserManager<User> userManager)
        {
            var authenticateResult = await httpContext.AuthenticateAsync("Bearer");

            if (authenticateResult.Succeeded)
            {
                var principal = new ClaimsPrincipal(new ClaimsIdentity(authenticateResult.Principal.Identity));
               
                var userId = principal.GetUserId();

                // TODO: need refactor, too many calls DB

                var user = await userManager.FindByIdAsync(userId);
                IEnumerable<string> userRoles = await userManager.GetRolesAsync(user);
                (principal.Identity as ClaimsIdentity).AddClaims(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

                //

                httpContext.User = principal;
            }

            await _requestDelegate.Invoke(httpContext);
        }
    }
}
