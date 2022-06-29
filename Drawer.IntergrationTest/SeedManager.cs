﻿using Drawer.Application.Services.Authentication.Repos;
using Drawer.Contract;
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
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.IntergrationTest
{
    public static class SeedManager
    {
        public static async Task ClearDatabase(IServiceScope scope)
        {
            var dbContext = scope.ServiceProvider.GetService<DrawerDbContext>()
                          ?? throw new Exception("DrawerIdentityDbContext is null");
            //dbContext.Database.Migrate();

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

            dbContext.Positions.Truncate();
            dbContext.Zones.Truncate();
            dbContext.ZoneTypes.Truncate();
            dbContext.WorkPlaces.Truncate();

            await dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// 서비스 스코프를 이용하여 시드를 생성한다.
        /// </summary>
        /// <param name="scope"></param>
        /// <exception cref="Exception"></exception>
        public static async Task UsingScopeAsync(IServiceScope scope)
        {
            var authenticationUnitOfWork = scope.ServiceProvider.GetService<IAuthenticationUnitOfWork>()
               ?? default!;

            // 사용자 Seed
            foreach (var userRequest in new UserSeeds().Users)
            {
                // Email인증을 우회하기 위해 API대신 직접 사용자를 등록한다
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

        /// <summary>
        /// API를 이용해서 시드를 생성한다.
        /// </summary>
        /// <param name="client"></param>
        /// <exception cref="NotImplementedException"></exception>
        public static async Task UsingApiAsync(HttpClient client)
        {
            // 회사 시드 등록
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Company.Create);
            requestMessage.Content = JsonContent.Create(CompanySeeds.MasterCompany);
            await client.SendAsyncWithMasterAuthentication(requestMessage);
        }
    }
}
