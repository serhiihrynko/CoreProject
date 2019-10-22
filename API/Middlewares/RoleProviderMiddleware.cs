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
        private readonly UserRolesCachingService _userRolesCache;

        public RoleProviderMiddleware(RequestDelegate requestDelegate, UserRolesCachingService userRolesCache)
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
                    (httpContext.User.Identity as ClaimsIdentity).AddClaims(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));
                } 
            }

            await _requestDelegate.Invoke(httpContext);
        }
    }
}
