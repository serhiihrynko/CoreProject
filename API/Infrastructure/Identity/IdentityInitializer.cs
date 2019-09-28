using API.Models;
using DAO.Contexts;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Infrastructure.Identity
{
    public class IdentityInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider, CreateUserModel model)
        {
            DbContextIdentity context = serviceProvider.GetRequiredService<DbContextIdentity>();
            UserManager<User> userManager = serviceProvider.GetRequiredService<UserManager<User>>();

            context.Database.EnsureCreated();

            if (!context.Users.Any())
            {
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
}
