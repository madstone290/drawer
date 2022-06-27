using Drawer.Application.Services.Authentication.Repos;
using Drawer.Domain.Models.Authentication;
using Drawer.Domain.Models.UserInformation;
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
        public static async Task InitializeAsync(IServiceScope scope)
        {
            var dbContext = scope.ServiceProvider.GetService<DrawerDbContext>()
                          ?? throw new Exception("DrawerIdentityDbContext is null");
            dbContext.Database.Migrate();

            dbContext.Users.Truncate();
            dbContext.UserRoles.Truncate();
            dbContext.UserClaims.Truncate();
            dbContext.UserLogins.Truncate();
            dbContext.UserTokens.Truncate();
            dbContext.Roles.Truncate();
            dbContext.RoleClaims.Truncate();
            dbContext.RefreshTokens.Truncate();

            dbContext.CompanyMembers.Truncate();
            dbContext.Companies.Truncate();

            dbContext.SaveChanges();

            var authenticationUnitOfWork = scope.ServiceProvider.GetService<IAuthenticationUnitOfWork>()
                ?? default!;

            foreach (var userRequest in new UserSeeds().Users)
            {
                var user = new IdentityUser()
                {
                    Email = userRequest.Email,
                    UserName = userRequest.Email,
                    EmailConfirmed = true
                };

                var creatResult = authenticationUnitOfWork.UserManager
                    .CreateAsync(user, userRequest.Password).GetAwaiter().GetResult();

                if (!creatResult.Succeeded)
                    throw new Exception(string.Join(", ", creatResult.Errors.Select(x => x.Description)));

                var userInfo = new UserInfo(user.Id, user.Email, userRequest.DisplayName);
                await authenticationUnitOfWork.UserInfoRepository.AddAsync(userInfo);
            }

            await authenticationUnitOfWork.SaveChangesAsync();


        }
      
    }
}
