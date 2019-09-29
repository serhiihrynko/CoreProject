using API.Models;
using DAO.Contexts;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace API.Infrastructure.Identity
{
    public class IdentityInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider, CreateUserModel model)
        {
            var context = serviceProvider.GetRequiredService<DbContextIdentity>();
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

            context.Database.EnsureCreated();

            if (context.Users.Any())
            {
                return;
            }

            var user = new User()
            {
                SecurityStamp = Guid.NewGuid().ToString(),
                Email = model.Email,
                UserName = model.UserName
            };

            userManager.CreateAsync(user, model.Password);
        }
    }
}
