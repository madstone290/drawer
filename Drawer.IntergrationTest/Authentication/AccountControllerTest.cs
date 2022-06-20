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
            var registerRequest = new RegisterRequest(email, password, displayName);
            
            // Act
            var responseMessage = await _client.PostAsJsonAsync("account/register", registerRequest);

            // Assert
            responseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var registerResponse = await responseMessage.Content.ReadFromJsonAsync<RegisterResponse>();
            registerResponse.Should().NotBeNull();
            registerResponse!.Email.Should().Be(registerRequest.Email);
            registerResponse!.DisplayName.Should().Be(registerRequest.DisplayName);
        }

        [Theory]
        [InlineData("register_bad_email1@email.com", "password1", "user1")]
        [InlineData("register_bad_email2@email.com", "password2", "user2")]
        [InlineData("register_bad_email3@email.com", "password3", "user3")]
        public async Task Register_With_DuplicateEmail_Returns_BadRequest(string email,string password, string displayName)
        {
            // Arrange
            var registerRequest = new RegisterRequest(email, password, displayName);

            // Act
            var firstResponseMessage = await _client.PostAsJsonAsync("account/register", registerRequest);
            var secondResponseMessage = await _client.PostAsJsonAsync("account/register", registerRequest);
            
            // Assert
            firstResponseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var registerResponse = await firstResponseMessage.Content.ReadFromJsonAsync<RegisterResponse>();
            registerResponse.Should().NotBeNull();
            registerResponse!.Email.Should().Be(registerRequest.Email);
            registerResponse!.DisplayName.Should().Be(registerRequest.DisplayName);

            secondResponseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Theory]
        [InlineData("login_bad_email1@email.com", "password1", "user1")]
        [InlineData("login_bad_email2@email.com", "password2", "user2")]
        [InlineData("login_bad_email3@email.com", "password3", "user3")]
        public async Task Login_With_UnconfirmedEmail_Returns_BadRequest(string email, string password, string displayName)
        {
            // Arrange
            var registerRequest = new RegisterRequest(email, password, displayName);
            var registerResponseMessage = await _client.PostAsJsonAsync("account/register", registerRequest);
            registerResponseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var loginRequest = new LoginRequest(email, password);

            // Act
            var loginResponseMessage = await _client.PostAsJsonAsync("account/login", loginRequest);

            // Assert
            loginResponseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        
        [Theory]
        // 이메일 인증 테스트가 어렵기 때문에 마스터 계정을 이용한다
        [InlineData("master@master.com", "master")]
        public async Task Login_With_ConfirmedEmail_Returns_Ok_With_Tokens(string email, string password)
        {
            // Arrange
            var loginRequest = new LoginRequest(email, password);

            // Act
            var loginResponseMessage = await _client.PostAsJsonAsync("account/login", loginRequest);

            // Assert
            loginResponseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var loginResponse = await loginResponseMessage.Content.ReadFromJsonAsync<LoginResponse>();
            loginResponse.Should().NotBeNull();
            loginResponse!.AccessToken.Should().NotBeNullOrWhiteSpace();
            loginResponse!.RefreshToken.Should().NotBeNullOrWhiteSpace();

            _outputHelper.WriteLine($"AT: {loginResponse.AccessToken}");
            _outputHelper.WriteLine($"RT: {loginResponse.RefreshToken}");
        }

        [Theory]
        [InlineData("master@master.com", "master")]
        public async Task Refresh_Returns_Ok_With_ValidAccessToken(string email, string password)
        {
            // Arrange
            var loginRequest = new LoginRequest(email, password);
            var loginResponseMessage = await _client.PostAsJsonAsync("account/login", loginRequest);
            var loginResponse = await loginResponseMessage.Content.ReadFromJsonAsync<LoginResponse>();
            var refreshRequest = new RefreshRequest(email, loginResponse!.RefreshToken);

            // Act
            var refreshResponseMessage = await _client.PostAsJsonAsync("account/refresh", refreshRequest);

            // Assert
            refreshResponseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var refreshResponse = await refreshResponseMessage.Content.ReadFromJsonAsync<RefreshResponse>();
            refreshResponse.Should().NotBeNull();
            refreshResponse!.AccessToken.Should().NotBeNullOrWhiteSpace();

            _outputHelper.WriteLine($"AT: {loginResponse.AccessToken}");
        }

        [Theory]
        [InlineData("master@master.com", "master")]
        public async Task Refresh_With_InvalidRefreshToken_Returns_Badrequest(string email, string password)
        {
            // Arrange
            var loginRequest = new LoginRequest(email, password);
            var loginResponseMessage = await _client.PostAsJsonAsync("account/login", loginRequest);
            var loginResponse = await loginResponseMessage.Content.ReadFromJsonAsync<LoginResponse>();
            var refreshRequest = new RefreshRequest(email, loginResponse!.RefreshToken + "fail");

            // Act
            var refreshResponseMessage = await _client.PostAsJsonAsync("account/refresh", refreshRequest);

            // Assert
            refreshResponseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task SecurityTest_With_InvalidAccessToken_Returns_Unauthorized()
        {
            // Arrange
            var accessToken = Guid.NewGuid().ToString();
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, "account/SecurityTest");
            requestMessage.SetBearerToken(accessToken);
            
            // Act
            var responseMessage = await _client.SendAsync(requestMessage);

            // Assert
            responseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
        }


        [Theory]
        [InlineData("master@master.com", "master")]
        public async Task SecurityTest_With_ValidAccessToken_Returns_Unauthorized(string email, string password)
        {
            // Arrange
            var loginRequest = new LoginRequest(email, password);
            var loginResponseMessage = await _client.PostAsJsonAsync("account/login", loginRequest);
            var loginResponse = await loginResponseMessage.Content.ReadFromJsonAsync<LoginResponse>();

            var request = new HttpRequestMessage(HttpMethod.Post, "account/SecurityTest");
            request.SetBearerToken(loginResponse!.AccessToken);

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }




    }
}
