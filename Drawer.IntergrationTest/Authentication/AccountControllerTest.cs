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
using Xunit.Abstractions;

namespace Drawer.IntergrationTest.Authentication
{
    public class AccountControllerTest : IClassFixture<ApiInstance>
    {
        private readonly HttpClient _client;
        private readonly ITestOutputHelper _outputHelper;

        public AccountControllerTest(ApiInstance apiInstance, ITestOutputHelper outputHelper)
        {
            _client = apiInstance.Client;
            _outputHelper = outputHelper;
        }

        [Theory]
        [InlineData("register_ok_email1@email.com", "password1", "user1")]
        [InlineData("register_ok_email2@email.com", "password2", "user2")]
        [InlineData("register_ok_email3@email.com", "password3", "user3")]
        public async Task Register_Returns_Ok_With_RegisterInfo(string email, string password, string displayName)
        {
            // Arrange
            var registerModel = new RegisterModel(email, password, displayName);
            
            // Act
            var response = await _client.PostAsJsonAsync("account/register", registerModel);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var registerResult = await response.Content.ReadFromJsonAsync<RegisterResult>();
            registerResult.Should().NotBeNull();
            registerResult!.Email.Should().Be(registerModel.Email);
            registerResult!.DisplayName.Should().Be(registerModel.DisplayName);
        }

        [Theory]
        [InlineData("register_bad_email1@email.com", "password1", "user1")]
        [InlineData("register_bad_email2@email.com", "password2", "user2")]
        [InlineData("register_bad_email3@email.com", "password3", "user3")]
        public async Task Register_With_DuplicateEmail_Returns_BadRequest(string email,string password, string displayName)
        {
            // Arrange
            var registerModel = new RegisterModel(email, password, displayName);

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

        [Theory]
        [InlineData("login_bad_email1@email.com", "password1", "user1")]
        [InlineData("login_bad_email2@email.com", "password2", "user2")]
        [InlineData("login_bad_email3@email.com", "password3", "user3")]
        public async Task Login_With_UnconfirmedEmail_Returns_BadRequest(string email, string password, string displayName)
        {
            // Arrange
            var registerModel = new RegisterModel(email, password, displayName);
            var registerResult = await _client.PostAsJsonAsync("account/register", registerModel);
            registerResult.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var loginModel = new LoginModel(email, password);

            // Act
            var response = await _client.PostAsJsonAsync("account/login", loginModel);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        
        [Theory]
        // 이메일 인증 테스트가 어렵기 때문에 마스터 계정을 이용한다
        [InlineData("master@master.com", "master")]
        public async Task Login_With_ConfirmedEmail_Returns_Ok_With_Tokens(string email, string password)
        {
            // Arrange
            var loginModel = new LoginModel(email, password);

            // Act
            var response = await _client.PostAsJsonAsync("account/login", loginModel);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var loginResult = await response.Content.ReadFromJsonAsync<LoginResult>();
            loginResult.Should().NotBeNull();
            loginResult!.AccessToken.Should().NotBeNullOrWhiteSpace();
            loginResult!.RefreshToken.Should().NotBeNullOrWhiteSpace();

            _outputHelper.WriteLine($"AT: {loginResult.AccessToken}");
            _outputHelper.WriteLine($"RT: {loginResult.RefreshToken}");
        }

        [Theory]
        [InlineData("master@master.com", "master")]
        public async Task Refresh_Returns_Ok_With_ValidAccessToken(string email, string password)
        {
            // Arrange
            var loginModel = new LoginModel(email, password);
            var loginResponse = await _client.PostAsJsonAsync("account/login", loginModel);
            var loginResult = await loginResponse.Content.ReadFromJsonAsync<LoginResult>();
            var refreshModel = new RefreshModel(email, loginResult!.RefreshToken);

            // Act
            var refreshResponse = await _client.PostAsJsonAsync("account/refresh", refreshModel);

            // Assert
            refreshResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var refreshResult = await refreshResponse.Content.ReadFromJsonAsync<RefreshResult>();
            refreshResult.Should().NotBeNull();
            refreshResult!.AccessToken.Should().NotBeNullOrWhiteSpace();

            _outputHelper.WriteLine($"AT: {loginResult.AccessToken}");
        }

        [Theory]
        [InlineData("master@master.com", "master")]
        public async Task Refresh_With_InvalidRefreshToken_Returns_Badrequest(string email, string password)
        {
            // Arrange
            var loginModel = new LoginModel(email, password);
            var loginResponse = await _client.PostAsJsonAsync("account/login", loginModel);
            var loginResult = await loginResponse.Content.ReadFromJsonAsync<LoginResult>();
            var refreshModel = new RefreshModel(email, loginResult!.RefreshToken + "fail");

            // Act
            var refreshResponse = await _client.PostAsJsonAsync("account/refresh", refreshModel);

            // Assert
            refreshResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task SecurityTest_With_InvalidAccessToken_Returns_Unauthorized()
        {
            // Arrange
            var accessToken = Guid.NewGuid().ToString();
            var request = new HttpRequestMessage(HttpMethod.Post, "account/SecurityTest");
            request.SetBearerToken(accessToken);
            
            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
        }


        [Theory]
        [InlineData("master@master.com", "master")]
        public async Task SecurityTest_With_ValidAccessToken_Returns_Unauthorized(string email, string password)
        {
            // Arrange
            var loginModel = new LoginModel(email, password);
            var loginResponse = await _client.PostAsJsonAsync("account/login", loginModel);
            var loginResult = await loginResponse.Content.ReadFromJsonAsync<LoginResult>();

            var request = new HttpRequestMessage(HttpMethod.Post, "account/SecurityTest");
            request.SetBearerToken(loginResult!.AccessToken);

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }




    }
}
