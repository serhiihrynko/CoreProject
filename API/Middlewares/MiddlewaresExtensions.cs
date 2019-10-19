using API.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace API.Extensions
{
    public static class MiddlewaresExtensions
    {
        public static IApplicationBuilder UseRoleProvider(this IApplicationBuilder builder) =>
            builder.UseMiddleware<RoleProviderMiddleware>();
    }
}
