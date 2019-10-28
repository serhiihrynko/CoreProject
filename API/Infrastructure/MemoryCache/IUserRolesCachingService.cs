using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Infrastructure.MemoryCache
{
    public interface IUserRolesCachingService
    {
        Task<IList<string>> GetUserRolesAsync(string userId);
    }
}
