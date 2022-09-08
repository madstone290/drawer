using Drawer.Shared;
using Drawer.IntergrationTest.Seeds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Drawer.Application.Services.Authentication.CommandModels;
using Drawer.Application.Services.UserInformation.Repos;
using Drawer.Application.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Drawer.Domain.Models.UserInformation;

namespace Drawer.IntergrationTest
{
    public static class Extensions
    {
        /// <summary>
        /// 마스터 계정으로 로그인한다
        /// </summary>
        public static async Task<LoginResponseCommandModel> LoginAsync(this HttpClient client, string email, string password)
        {
            var loginRequest = new LoginCommandModel()
            {
                Email = email,
                Password = password,
            };
            var loginResponseMessage = await client.PostAsJsonAsync(ApiRoutes.Account.Login, loginRequest);
            var loginResponse = await loginResponseMessage.Content.ReadFromJsonAsync<LoginResponseCommandModel>();
            return loginResponse!;
        }

        public static async Task<HttpResponseMessage> SendWithToken(this HttpClient client, string email, string password, HttpRequestMessage request)
        {
            var loginResponse = await client.LoginAsync(email, password);
            request.SetBearerToken(loginResponse.AccessToken);
            return await client.SendAsync(request);
        }

        /// <summary>
        /// 마스터 계정으로 로그인한다
        /// </summary>
        public static async Task<LoginResponseCommandModel> MasterLoginAsync(this HttpClient client)
        {
            var loginRequest = new LoginCommandModel()
            {
                Email = UserSeeds.Master.Email,
                Password = UserSeeds.Master.Password
            };
            var loginResponseMessage = await client.PostAsJsonAsync(ApiRoutes.Account.Login, loginRequest);
            var loginResponse = await loginResponseMessage.Content.ReadFromJsonAsync<LoginResponseCommandModel>();
            return loginResponse!;
        }

        public static async Task<HttpResponseMessage> SendWithMasterAuthentication(this HttpClient client, HttpRequestMessage requestMessage)
        {
            var loginResponse = await client.MasterLoginAsync();
            requestMessage.SetBearerToken(loginResponse.AccessToken);

            return await client.SendAsync(requestMessage);
        }

        public static void SetBearerToken(this HttpRequestMessage request, string token)
        {
            request.Headers.Remove("Authorization");
            request.Headers.Add("Authorization", $"bearer {token}");
        }

        /// <summary>
        /// 두 시간이 주어진 정확도만큼 근접한지 확인한다.
        /// </summary>
        /// <param name="time1"></param>
        /// <param name="time2"></param>
        /// <param name="precision"></param>
        /// <returns></returns>
        public static bool IsCloseTo(this DateTime time1, DateTime time2, TimeSpan precision)
        {
            return time1.Subtract(time2).Duration() <= precision;
        }

        public static string ToDateFormat(this DateTime time)
        {
            return time.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// 스코프를 이용하여 사용자를 추가한다
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <exception cref="Exception"></exception>
        public static async Task AddUserAsync(this IServiceScope scope, string email, string userName, string password)
        {
            var unitOfWork = scope.ServiceProvider.GetService<IUnitOfWork>();
            var userManager = scope.ServiceProvider.GetService<UserManager<IdentityUser>>();
            var userRepository = scope.ServiceProvider.GetService<IUserRepository>();
            if (unitOfWork == null || userManager == null || userRepository == null)
                throw new Exception("서비스를 로드하지 못했습니다");

            await AddUserAsync(userManager, userRepository, new RegisterCommandModel()
            {
                Email = email,
                Password = password,
                UserName = userName
            });

            await unitOfWork.CommitAsync();
        }

        public static async Task AddUserAsync(this IServiceScope scope, IEnumerable<RegisterCommandModel> commandModels)
        {
            var unitOfWork = scope.ServiceProvider.GetService<IUnitOfWork>();
            var userManager = scope.ServiceProvider.GetService<UserManager<IdentityUser>>();
            var userRepository = scope.ServiceProvider.GetService<IUserRepository>();
            if (unitOfWork == null || userManager == null || userRepository == null)
                throw new Exception("서비스를 로드하지 못했습니다");

            foreach (var userDto in commandModels ?? Enumerable.Empty<RegisterCommandModel>())
            {
                await AddUserAsync(userManager, userRepository, userDto);
            }

            await unitOfWork.CommitAsync();
        }

        private static async Task AddUserAsync(UserManager<IdentityUser> userManager, IUserRepository userRepository, RegisterCommandModel userDto)
        {
            if (userManager == null || userRepository == null)
                throw new ArgumentNullException();

            // Email인증을 우회하기 위해 API대신 직접 사용자를 등록한다
            var identityUser = new IdentityUser()
            {
                Email = userDto.Email,
                UserName = userDto.Email,
                EmailConfirmed = true
            };
            var creatResult = userManager.CreateAsync(identityUser, userDto.Password).GetAwaiter().GetResult();

            if (!creatResult.Succeeded)
                throw new Exception(string.Join(", ", creatResult.Errors.Select(x => x.Description)));

            var user = new User(identityUser, identityUser.Email, userDto.UserName);
            await userRepository.AddAsync(user);
        }

    }
}
