using API.Infrastructure.Jwt;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Extensions
{
    public static class AuthorizationExtensions
    {
        public static string GetUserId(this HttpContext httpContext) => (httpContext.User == null)
            ? string.Empty
            : httpContext.User.Claims.Single(x => x.Type == JwtRegisteredClaimNamesCustom.Id).Value;
    }
}
