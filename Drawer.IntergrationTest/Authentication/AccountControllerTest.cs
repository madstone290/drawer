using Drawer.Application.Services.Authentication.Commands;
using Drawer.Contract.Authentication;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Drawer.IntergrationTest.Authentication
{
    public class AccountControllerTest : IClassFixture<ApiInstance>
    {
        private readonly HttpClient _client;

        public AccountControllerTest(ApiInstance apiInstance)
        {
            _client = apiInstance.Client;
        }

        [Fact]
        public async Task Register_Returns_Ok_With_RegisterInfo()
        {
            // Arrange
            string email = $"{Guid.NewGuid()}@test.com";
            string password = Guid.NewGuid().ToString();
            var registerModel = new RegisterModel(email, password, "TestUser");
            
            // Act
            var response = await _client.PostAsJsonAsync("account/register", registerModel);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var registerResult = await response.Content.ReadFromJsonAsync<RegisterResult>();
            registerResult.Should().NotBeNull();
            registerResult!.Email.Should().Be(registerModel.Email);
            registerResult!.DisplayName.Should().Be(registerModel.DisplayName);
        }

        [Fact]
        public async Task Register_With_DuplicateEmail_Returns_BadRequest()
        {
            // Arrange
            string email = $"{Guid.NewGuid()}@test.com";
            string password = Guid.NewGuid().ToString();
            var registerModel = new RegisterModel(email, password, "TestUser");

            // Act
            var firstResponse = await _client.PostAsJsonAsync("account/register", registerModel);
            var secondResponse = await _client.PostAsJsonAsync("account/register", registerModel);

            // Assert
            firstResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var registerResult = await firstResponse.Content.ReadFromJsonAsync<RegisterResult>();
            registerResult.Should().NotBeNull();
            registerResult!.Email.Should().Be(registerModel.Email);
            registerResult!.DisplayName.Should().Be(registerModel.DisplayName);

            secondResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Login_With_UnconfirmedEmail_Returns_BadRequest()
        {
            // Arrange
            string email = $"{Guid.NewGuid()}@test.com";
            string password = Guid.NewGuid().ToString();
            var registerModel = new RegisterModel(email, password, "TestUser");
            var registerResult = await _client.PostAsJsonAsync("account/register", registerModel);
            registerResult.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var loginModel = new LoginModel(email, password);

            // Act
            var response = await _client.PostAsJsonAsync("account/login", loginModel);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Login_With_ConfirmedEmail_Returns_Ok_With_Tokens()
        {
            // Arrange
            string email = "master@master.com";
            string password = "master";
            var loginModel = new LoginModel(email, password);

            // Act
            var response = await _client.PostAsJsonAsync("account/login", loginModel);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var loginResult = await response.Content.ReadFromJsonAsync<LoginResult>();
            loginResult.Should().NotBeNull();
            loginResult!.AccessToken.Should().NotBeNullOrWhiteSpace();
            loginResult!.RefreshToken.Should().NotBeNullOrWhiteSpace();
        }


    }
}
