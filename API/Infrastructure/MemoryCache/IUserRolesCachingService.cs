using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Infrastructure.MemoryCache
{
    public interface IUserRolesCachingService
    {
        Task<IEnumerable<string>> GetUserRolesAsync(string userId);
    }
}
