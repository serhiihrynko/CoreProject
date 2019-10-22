using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Extensions;
using API.Infrastructure.MemoryCache;
using Domain.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace API.Middlewares
{
    public class RoleProviderMiddleware
    {
        private readonly RequestDelegate _requestDelegate;
        private readonly IUserRolesCachingService _userRolesCache;

        public RoleProviderMiddleware(
            RequestDelegate requestDelegate, 
            IUserRolesCachingService userRolesCache)
        {
            _requestDelegate = requestDelegate;
            _userRolesCache = userRolesCache;
        }


        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext.User.Identity.IsAuthenticated)
            {
                var userId = httpContext.User.GetUserId();

                IEnumerable<string> userRoles = await _userRolesCache.GetUserRolesAsync(userId);

                if (userRoles != null)
                {
                    IEnumerable<Claim> claims = userRoles.Select(role => new Claim(ClaimTypes.Role, role));
                    (httpContext.User.Identity as ClaimsIdentity).AddClaims(claims);
                } 
            }

            await _requestDelegate.Invoke(httpContext);
        }
    }
}
