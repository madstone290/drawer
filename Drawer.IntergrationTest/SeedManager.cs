using Drawer.Domain.Models.Authentication;
using Drawer.Infrastructure.Data;
using Drawer.IntergrationTest.Seeds;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.IntergrationTest
{
    public static class SeedManager
    {
        public static void Initialize(IServiceScope scope)
        {
            var dbContext = scope.ServiceProvider.GetService<DrawerIdentityDbContext>()
                          ?? throw new Exception("DrawerIdentityDbContext is null");
            dbContext.Database.Migrate();

            dbContext.Users.RemoveRange(dbContext.Users.ToArray());
            dbContext.UserRoles.RemoveRange(dbContext.UserRoles.ToArray());
            dbContext.UserClaims.RemoveRange(dbContext.UserClaims.ToArray());
            dbContext.UserLogins.RemoveRange(dbContext.UserLogins.ToArray());
            dbContext.UserTokens.RemoveRange(dbContext.UserTokens.ToArray());
            dbContext.Roles.RemoveRange(dbContext.Roles.ToArray());
            dbContext.RoleClaims.RemoveRange(dbContext.RoleClaims.ToArray());
            dbContext.SaveChanges();

            var userManager = scope.ServiceProvider.GetService<UserManager<User>>();

            foreach(var userRequest in new UserSeeds().Users)
            {
                var user = new User(userRequest.Email, userRequest.DisplayName)
                {
                    EmailConfirmed = true
                };

                var creatResult = userManager!.CreateAsync(user, userRequest.Password).GetAwaiter().GetResult();
                if (!creatResult.Succeeded)
                    throw new Exception(string.Join(", ", creatResult.Errors.Select(x => x.Description)));
            }
          

        }
    }
}
