using API.Infrastructure.Jwt;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace API.Extensions
{
    public static class AuthorizationExtensions
    {
        public static string GetUserId(this HttpContext httpContext) => httpContext.User?.Claims.FindUserId();

        public static string GetUserId(this ClaimsPrincipal claimsPrincipal) => claimsPrincipal.Claims.FindUserId();


        private static string FindUserId(this IEnumerable<Claim> claims) => claims.SingleOrDefault(x => x.Type == JwtRegisteredClaimNamesCustom.UserId)?.Value;
    }
}
