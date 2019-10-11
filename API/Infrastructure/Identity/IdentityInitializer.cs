using API.Models;
using DAO.Contexts;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace API.Infrastructure.Identity
{
    public class IdentityInitializer
    {
        public static async Task Initialize(IServiceProvider serviceProvider, CreateUserModel model)
        {
            var context = serviceProvider.GetRequiredService<DbContextIdentity>();
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            context.Database.EnsureCreated();

            // add roles
            if (!context.Roles.Any())
            {
                var roles = new IdentityRole[]
                {
                    new IdentityRole(RoleConstants.Admin),
                    new IdentityRole(RoleConstants.User)
                };

                foreach (var role in roles)
                {
                    await roleManager.CreateAsync(role);
                }
            }

            // add user
            if (!context.Users.Any())
            {
                var user = new User()
                {
                    Email = model.Email,
                    UserName = model.UserName
                };

                var createUser = await userManager.CreateAsync(user, model.Password);

                // add user role
                if (
                    createUser.Succeeded
                    && (await roleManager.RoleExistsAsync(RoleConstants.Admin))
                    && (!(await userManager.GetRolesAsync(user)).Contains(RoleConstants.Admin)))
                {
                    await userManager.AddToRoleAsync(user, RoleConstants.Admin);
                }
            }
        }
    }
}
