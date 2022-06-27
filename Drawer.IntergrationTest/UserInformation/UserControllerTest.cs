using Drawer.Contract;
using Drawer.Contract.Authentication;
using Drawer.Contract.UserInformation;
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
            var responseMessage = await _client.GetAsync(ApiRoutes.User.GetUser);

            // Assert
            responseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
        }

        [Theory]
        [ClassData(typeof(UserSeeds.GetUser))]
        public async Task GetUser_WithToken_Returns_Ok_With_UserInfo(string email, string password, string displayName)
        {
            // Arrange
            var loginRequest = new LoginRequest(email, password);
            var loginResponseMessage = await _client.PostAsJsonAsync(ApiRoutes.Account.Login, loginRequest);
            var loginResponse = await loginResponseMessage.Content.ReadFromJsonAsync<LoginResponse>();

            var getUserRequestMessage = new HttpRequestMessage(HttpMethod.Get, ApiRoutes.User.GetUser);
            getUserRequestMessage.SetBearerToken(loginResponse!.AccessToken);

            // Act
            var responseMessage = await _client.SendAsync(getUserRequestMessage);

            // Assert
            responseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var getUserResponse = await responseMessage.Content.ReadFromJsonAsync<GetUserResponse>();
            getUserResponse.Should().NotBeNull();
            getUserResponse!.Email.Should().Be(email);
            getUserResponse!.DisplayName.Should().Be(displayName);
        }


        [Theory]
        [ClassData(typeof(UserSeeds.UpdateUser))]
        public async Task UpdateUser_WithToken_Returns_Ok_With_UserInfo(string email, string password)
        {
            // Arrange
            string displayName = Guid.NewGuid().ToString();
            var loginRequest = new LoginRequest(email, password);
            var loginResponseMessage = await _client.PostAsJsonAsync(ApiRoutes.Account.Login, loginRequest);
            var loginResponse = await loginResponseMessage.Content.ReadFromJsonAsync<LoginResponse>();

            var updateUserRequestMessage = new HttpRequestMessage(HttpMethod.Put, ApiRoutes.User.UpdateUser);
            updateUserRequestMessage.Content = JsonContent.Create(new UpdateUserRequest(displayName));
            updateUserRequestMessage.SetBearerToken(loginResponse!.AccessToken);

            // Act
            var responseMessage = await _client.SendAsync(updateUserRequestMessage);

            // Assert
            responseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var getUserResponse = await responseMessage.Content.ReadFromJsonAsync<UpdateUserResponse>();
            getUserResponse.Should().NotBeNull();
            getUserResponse!.DisplayName.Should().Be(displayName);

            _outputHelper.WriteLine(displayName);
        }

        [Theory]
        [ClassData(typeof(UserSeeds.UpdaterPassword_1))]
        public async Task UpdatePassword_With_InvalidPasswords_Returns_BadRequest(string email, string password)
        {
            // Arrange
            var newPassword = Guid.NewGuid().ToString();
            var loginRequest = new LoginRequest(email, password);
            var loginResponseMessage = await _client.PostAsJsonAsync(ApiRoutes.Account.Login, loginRequest);
            var loginResponse = await loginResponseMessage.Content.ReadFromJsonAsync<LoginResponse>();

            var updatePasswordRequestMessage = new HttpRequestMessage(HttpMethod.Put, ApiRoutes.User.UpdatePassword);
            updatePasswordRequestMessage.Content = JsonContent.Create(new UpdatePasswordRequest("salt" + password, newPassword));
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
            var loginRequest = new LoginRequest(email, password);
            var loginResponseMessage = await _client.PostAsJsonAsync(ApiRoutes.Account.Login, loginRequest);
            var loginResponse = await loginResponseMessage.Content.ReadFromJsonAsync<LoginResponse>();

            var updatePasswordRequestMessage = new HttpRequestMessage(HttpMethod.Put, ApiRoutes.User.UpdatePassword);
            updatePasswordRequestMessage.Content = JsonContent.Create(new UpdatePasswordRequest(password, newPassword));
            updatePasswordRequestMessage.SetBearerToken(loginResponse!.AccessToken);

            // Act
            var responseMessage = await _client.SendAsync(updatePasswordRequestMessage);
            var newLoginResponseMessage = await _client.PostAsJsonAsync(ApiRoutes.Account.Login, new LoginRequest(email, newPassword));

            // Assert
            responseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            newLoginResponseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

    }
}
