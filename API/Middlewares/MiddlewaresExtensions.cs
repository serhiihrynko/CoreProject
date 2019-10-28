using Microsoft.AspNetCore.Builder;

namespace API.Middlewares
{
    public static class MiddlewaresExtensions
    {
        public static IApplicationBuilder UseRoleProvider(this IApplicationBuilder builder) =>
            builder.UseMiddleware<RoleProviderMiddleware>();
    }
}
