﻿using Drawer.Shared;
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
using Drawer.Application.Services.Authentication.CommandModels;

namespace Drawer.IntergrationTest.Authentication
{
    [Collection(ApiInstanceCollection.Default)]
    public class AccountControllerTest 
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
        public async Task Register_Returns_Ok_With_RegisterInfo(string email, string password, string displayName)
        {
            // Arrange
            var registerRequest = new RegisterCommandModel(email, password, displayName);
            
            // Act
            var responseMessage = await _client.PostAsJsonAsync(ApiRoutes.Account.Register, registerRequest);

            // Assert
            responseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Theory]
        [InlineData("register_bad_email1@gmail.com", "password1", "user1")]
        public async Task Register_With_DuplicateEmail_Returns_BadRequest(string email,string password, string displayName)
        {
            // Arrange
            var registerRequest = new RegisterCommandModel(email, password, displayName);

            // Act
            var firstResponseMessage = await _client.PostAsJsonAsync(ApiRoutes.Account.Register, registerRequest);
            var secondResponseMessage = await _client.PostAsJsonAsync(ApiRoutes.Account.Register, registerRequest);
            
            // Assert
            firstResponseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Theory]
        [InlineData("login_bad_email1@email.com", "password1", "user1")]
        public async Task Login_With_UnconfirmedEmail_Returns_BadRequest(string email, string password, string displayName)
        {
            // Arrange
            var registerRequest = new RegisterCommandModel(email, password, displayName);
            var registerResponseMessage = await _client.PostAsJsonAsync(ApiRoutes.Account.Register, registerRequest);
            registerResponseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var loginRequest = new LoginCommandModel(email, password);

            // Act
            var loginResponseMessage = await _client.PostAsJsonAsync(ApiRoutes.Account.Login, loginRequest);

            // Assert
            loginResponseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        
        [Theory]
        // 이메일 인증 테스트가 어렵기 때문에 마스터 계정을 이용한다
        [ClassData(typeof(UserSeeds.LoginUser))]
        public async Task Login_With_ConfirmedEmail_Returns_Ok_With_Tokens(string email, string password)
        {
            // Arrange
            var loginRequest = new LoginCommandModel(email, password);

            // Act
            var loginResponseMessage = await _client.PostAsJsonAsync(ApiRoutes.Account.Login, loginRequest);

            // Assert
            loginResponseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var loginResponse = await loginResponseMessage.Content.ReadFromJsonAsync<LoginResponseCommandModel>();
            loginResponse.Should().NotBeNull();
            loginResponse!.AccessToken.Should().NotBeNullOrWhiteSpace();
            loginResponse!.RefreshToken.Should().NotBeNullOrWhiteSpace();

            _outputHelper.WriteLine($"AT: {loginResponse.AccessToken}");
            _outputHelper.WriteLine($"RT: {loginResponse.RefreshToken}");
        }

        [Theory]
        [ClassData(typeof(UserSeeds.RefreshUser_1))]
        public async Task Refresh_Returns_Ok_With_ValidAccessToken(string email, string password)
        {
            // Arrange
            var loginRequest = new LoginCommandModel(email, password);
            var loginResponseMessage = await _client.PostAsJsonAsync(ApiRoutes.Account.Login, loginRequest);
            var loginResponse = await loginResponseMessage.Content.ReadFromJsonAsync<LoginResponseCommandModel>();
            var refreshRequest = new RefreshCommandModel(email, loginResponse!.RefreshToken);

            // Act
            var refreshResponseMessage = await _client.PostAsJsonAsync(ApiRoutes.Account.Refresh, refreshRequest);

            // Assert
            refreshResponseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var accessToken = await refreshResponseMessage.Content.ReadFromJsonAsync<string>() ?? default!;
            accessToken.Should().NotBeNull();
            accessToken.Should().NotBeNullOrWhiteSpace();

            _outputHelper.WriteLine($"AT: {loginResponse.AccessToken}");
        }

        [Theory]
        [ClassData(typeof(UserSeeds.RefreshUser_2))]
        public async Task Refresh_With_InvalidRefreshToken_Returns_Badrequest(string email, string password)
        {
            // Arrange
            var loginRequest = new LoginCommandModel(email, password);
            var loginResponseMessage = await _client.PostAsJsonAsync(ApiRoutes.Account.Login, loginRequest);
            var loginResponse = await loginResponseMessage.Content.ReadFromJsonAsync<LoginResponseCommandModel>();
            var refreshRequest = new RefreshCommandModel(email, loginResponse!.RefreshToken + "fail");

            // Act
            var refreshResponseMessage = await _client.PostAsJsonAsync(ApiRoutes.Account.Refresh, refreshRequest);

            // Assert
            refreshResponseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task SecurityTest_With_InvalidAccessToken_Returns_Unauthorized()
        {
            // Arrange
            var accessToken = Guid.NewGuid().ToString();
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Account.SecurityTest);
            requestMessage.SetBearerToken(accessToken);
            
            // Act
            var responseMessage = await _client.SendAsync(requestMessage);

            // Assert
            responseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
        }

        [Theory]
        [ClassData(typeof(UserSeeds.LoginUser))]
        public async Task SecurityTest_With_ValidAccessToken_Returns_Unauthorized(string email, string password)
        {
            // Arrange
            var loginRequest = new LoginCommandModel(email, password);
            var loginResponseMessage = await _client.PostAsJsonAsync(ApiRoutes.Account.Login, loginRequest);
            var loginResponse = await loginResponseMessage.Content.ReadFromJsonAsync<LoginResponseCommandModel>();

            var request = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Account.SecurityTest);
            request.SetBearerToken(loginResponse!.AccessToken);

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }




    }
}
