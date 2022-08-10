using Drawer.Shared;
using Drawer.IntergrationTest.Seeds;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using Drawer.Application.Services.UserInformation.QueryModels;
using Drawer.Application.Services.UserInformation.CommandModels;
using Drawer.Application.Services.Authentication.CommandModels;

namespace Drawer.IntergrationTest.UserInformation
{
    [Collection(ApiInstanceCollection.Default)]
    public class UserControllerTest 
    {
        private readonly HttpClient _client;
        private readonly ITestOutputHelper _outputHelper;

        public UserControllerTest(ApiInstance apiInstance, ITestOutputHelper outputHelper)
        {
            _client = apiInstance.Client;
            _outputHelper = outputHelper;
        }

        [Fact]
        public async Task GetUser_WithoutToken_Returns_Unauthorized()
        {
            // Arrange
            
            // Act
            var responseMessage = await _client.GetAsync(ApiRoutes.User.Get);

            // Assert
            responseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
        }

        [Theory]
        [ClassData(typeof(UserSeeds.GetUser))]
        public async Task GetUser_WithToken_Returns_Ok_With_UserInfo(string email, string password, string displayName)
        {
            // Arrange
            var loginRequest = new LoginCommandModel(email, password);
            var loginResponseMessage = await _client.PostAsJsonAsync(ApiRoutes.Account.Login, loginRequest);
            var loginResponse = await loginResponseMessage.Content.ReadFromJsonAsync<LoginResponseCommandModel>();

            var getUserRequestMessage = new HttpRequestMessage(HttpMethod.Get, ApiRoutes.User.Get);
            getUserRequestMessage.SetBearerToken(loginResponse!.AccessToken);

            // Act
            var responseMessage = await _client.SendAsync(getUserRequestMessage);

            // Assert
            responseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var userInfo = await responseMessage.Content.ReadFromJsonAsync<UserInfoQueryModel>() ?? default!;
            userInfo.Should().NotBeNull();
            userInfo.Email.Should().Be(email);
            userInfo.DisplayName.Should().Be(displayName);
        }


        [Theory]
        [ClassData(typeof(UserSeeds.UpdateUser))]
        public async Task UpdateUser_WithToken_Returns_Ok_With_UserInfo(string email, string password)
        {
            // Arrange
            string displayName = Guid.NewGuid().ToString();
            var loginRequest = new LoginCommandModel(email, password);
            var loginResponseMessage = await _client.PostAsJsonAsync(ApiRoutes.Account.Login, loginRequest);
            var loginResponse = await loginResponseMessage.Content.ReadFromJsonAsync<LoginResponseCommandModel>();

            var userInfoDto = new UserInfoCommandModel()
            {
                DisplayName = displayName
            };

            var updateUserRequestMessage = new HttpRequestMessage(HttpMethod.Put, ApiRoutes.User.Update);
            updateUserRequestMessage.Content = JsonContent.Create(userInfoDto);
            updateUserRequestMessage.SetBearerToken(loginResponse!.AccessToken);

            // Act
            var responseMessage = await _client.SendAsync(updateUserRequestMessage);

            // Assert
            responseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Theory]
        [ClassData(typeof(UserSeeds.UpdaterPassword_1))]
        public async Task UpdatePassword_With_InvalidPasswords_Returns_BadRequest(string email, string password)
        {
            // Arrange
            var newPassword = Guid.NewGuid().ToString();
            var loginRequest = new LoginCommandModel(email, password);
            var loginResponseMessage = await _client.PostAsJsonAsync(ApiRoutes.Account.Login, loginRequest);
            var loginResponse = await loginResponseMessage.Content.ReadFromJsonAsync<LoginResponseCommandModel>();

            var userPasswordDto = new UserPasswordCommandModel()
            {
                Password = "salt" + password,
                NewPassword = newPassword
            };
            var updatePasswordRequestMessage = new HttpRequestMessage(HttpMethod.Put, ApiRoutes.User.UpdatePassword);
            updatePasswordRequestMessage.Content = JsonContent.Create(userPasswordDto);
            updatePasswordRequestMessage.SetBearerToken(loginResponse!.AccessToken);

            // Act
            var responseMessage = await _client.SendAsync(updatePasswordRequestMessage);

            // Assert
            responseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);

            _outputHelper.WriteLine(await responseMessage.Content.ReadAsStringAsync());
        }

        [Theory]
        [ClassData(typeof(UserSeeds.UpdaterPassword_2))]
        public async Task UpdatePassword_With_ValidPasswords_Returns_Ok(string email, string password)
        {
            // Arrange
            var newPassword = Guid.NewGuid().ToString();
            var loginRequest = new LoginCommandModel(email, password);
            var loginResponseMessage = await _client.PostAsJsonAsync(ApiRoutes.Account.Login, loginRequest);
            var loginResponse = await loginResponseMessage.Content.ReadFromJsonAsync<LoginResponseCommandModel>();

            var userPasswordDto = new UserPasswordCommandModel()
            {
                Password = password,
                NewPassword = newPassword
            };
            var updatePasswordRequestMessage = new HttpRequestMessage(HttpMethod.Put, ApiRoutes.User.UpdatePassword);
            updatePasswordRequestMessage.Content = JsonContent.Create(userPasswordDto);
            updatePasswordRequestMessage.SetBearerToken(loginResponse!.AccessToken);

            // Act
            var responseMessage = await _client.SendAsync(updatePasswordRequestMessage);
            var newLoginResponseMessage = await _client.PostAsJsonAsync(ApiRoutes.Account.Login, new LoginCommandModel(email, newPassword));

            // Assert
            responseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            newLoginResponseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

    }
}
