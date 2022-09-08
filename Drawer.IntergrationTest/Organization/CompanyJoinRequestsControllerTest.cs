using Drawer.Application.Services.Organization.CommandModels;
using Drawer.Application.Services.Organization.Commands;
using Drawer.Application.Services.Organization.QueryModels;
using Drawer.Application.Services.UserInformation.CommandModels;
using Drawer.Application.Services.UserInformation.QueryModels;
using Drawer.Shared;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Org.BouncyCastle.Crypto.Operators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Drawer.IntergrationTest.Organization
{
    [Collection(ApiInstanceCollection.Default)]
    public class CompanyJoinRequestsControllerTest
    {
        private readonly HttpClient _client;
        private readonly IServiceProvider _serviceProvider;
        private readonly ITestOutputHelper _outputHelper;

        public CompanyJoinRequestsControllerTest(ApiInstance apiInstance, ITestOutputHelper outputHelper)
        {
            _client = apiInstance.Client;
            _serviceProvider = apiInstance.ServiceProvider;
            _outputHelper = outputHelper;
        }

        public class TestUser
        {
            public string Email { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
            public string Name { get; set; } = string.Empty;
        }

        private async Task<TestUser> CreateOwner()
        {
            var owner = await CreateUser();
            await CreateCompany(owner);

            return owner;
        }

        private async Task<TestUser> CreateUser()
        {
            var scope = _serviceProvider.CreateScope();
            var testUser = new TestUser()
            {
                Email = $"{Guid.NewGuid()}@mymail.com",
                Name = $"{Guid.NewGuid()}",
                Password = $"aA1!{Guid.NewGuid()}"
            };

            await scope.AddUserAsync(testUser.Email,testUser.Name, testUser.Password);
            
            return testUser;
        }

        private async Task CreateCompany(TestUser user)
        {
            var companyDto = new CompanyCommandModel()
            {
                Name = Guid.NewGuid().ToString(),
                PhoneNumber = Guid.NewGuid().ToString()
            };
            var request = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Company.Add)
            {
                Content = JsonContent.Create(companyDto)
            };
            var response = await _client.SendWithToken(user.Email, user.Password, request);
        }

        [Fact]
        public async void AddRequest_Return_Ok()
        {
            // Arrange
            var owner = await CreateOwner();
            var user = await CreateUser();

            // Act
            var joinRequestDto = new JoinRequestAddCommandModel()
            {
                OwnerEmail = owner.Email
            };
            var request = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.JoinRequests.Add)
            {
                Content = JsonContent.Create(joinRequestDto)
            };
            var response = await _client.SendWithToken(user.Email, user.Password, request);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var joinRequestId = await response.Content.ReadFromJsonAsync<long>();
            joinRequestId.Should().BeGreaterThan(0);
        }

        [Fact]
        public async void AcceptRequest_CreateMember()
        {
            // Arrange
            var owner = await CreateOwner();
            var user = await CreateUser();

            var joinRequestDto = new JoinRequestAddCommandModel()
            {
                OwnerEmail = owner.Email
            };
            var addRequest = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.JoinRequests.Add)
            {
                Content = JsonContent.Create(joinRequestDto)
            };

            var addRresponse = await _client.SendWithToken(user.Email, user.Password, addRequest);
            var joinRequestId = await addRresponse.Content.ReadFromJsonAsync<long>();

            // Act
            var joinRequestHandleDto = new JoinRequestHandleCommandModel()
            {
                IsAccepted = true,
            };

            var handleRequest = new HttpRequestMessage(HttpMethod.Put, ApiRoutes.JoinRequests.Handle.Replace("{id}", $"{joinRequestId}"))
            {
                Content = JsonContent.Create(joinRequestHandleDto)
            };
            var handleResponse = await _client.SendWithToken(owner.Email, owner.Password, handleRequest);

            // Assert
            handleResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var loginResponse = await _client.LoginAsync(user.Email, user.Password);
            loginResponse.IsCompanyMember.Should().BeTrue();
        }

        [Fact]
        public async void RefuseRequest_DoesNot_CreateMember()
        {
            // Arrange
            var owner = await CreateOwner();
            var user = await CreateUser();

            var joinRequestDto = new JoinRequestAddCommandModel()
            {
                OwnerEmail = owner.Email
            };
            var addRequest = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.JoinRequests.Add)
            {
                Content = JsonContent.Create(joinRequestDto)
            };

            var addRresponse = await _client.SendWithToken(user.Email, user.Password, addRequest);
            var joinRequestId = await addRresponse.Content.ReadFromJsonAsync<long>();

            // Act
            var joinRequestHandleDto = new JoinRequestHandleCommandModel()
            {
                IsAccepted = false,
            };

            var handleRequest = new HttpRequestMessage(HttpMethod.Put, ApiRoutes.JoinRequests.Handle.Replace("{id}", $"{joinRequestId}"))
            {
                Content = JsonContent.Create(joinRequestHandleDto)
            };
            var handleResponse = await _client.SendWithToken(owner.Email, owner.Password, handleRequest);

            // Assert
            handleResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var loginResponse = await _client.LoginAsync(user.Email, user.Password);
            loginResponse.IsCompanyMember.Should().BeFalse();
        }

    }
}
