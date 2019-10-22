using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Infrastructure.MemoryCache
{
    public class UserRolesCachingService
    {
        private readonly IMemoryCache _cache;
        private readonly IServiceProvider _serviceProvider;

        public UserRolesCachingService(IServiceProvider serviceProvider, IMemoryCache cache)
        {
            _cache = cache;
            _serviceProvider = serviceProvider;
        }


        public async Task<IEnumerable<string>> GetUserRolesAsync(string userId)
        {
            IEnumerable<string> userRoles = null;

            if (!_cache.TryGetValue(userId, out userRoles))
            {
                using (var serviceScope = _serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    var userManager = serviceScope.ServiceProvider.GetService<UserManager<User>>();

                    var user = await userManager.FindByIdAsync(userId);
                    userRoles = await userManager.GetRolesAsync(user);
                }

                if (userRoles != null)
                {
                    _cache.Set(userId, userRoles);
                }
            }

            return userRoles;
        }
    }
}
