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
    public class AuthControllerTest : IClassFixture<ApiInstance>
    {
        private readonly HttpClient _client;

        public AuthControllerTest(ApiInstance apiInstance)
        {
            _client = apiInstance.Client;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Register_Returns_Ok_With_RegisterInfo()
        {
            // Arrange
            string email = $"{Guid.NewGuid()}@test.com";
            string password = Guid.NewGuid().ToString();
            var registerModel = new RegisterModel(email, password, "TestUser");
            
            // Act
            var result = await _client.PostAsJsonAsync("auth/register", registerModel);

            // Assert
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var registerResultModel = await result.Content.ReadFromJsonAsync<RegisterResult>();
            registerResultModel.Should().NotBeNull();
            registerResultModel!.Email.Should().Be(registerModel.Email);
            registerResultModel!.DisplayName.Should().Be(registerModel.DisplayName);
        }

        [Fact]
        public async Task Register_DuplicateEmail_Returns_BadRequest()
        {
            // Arrange
            string email = $"{Guid.NewGuid()}@test.com";
            string password = Guid.NewGuid().ToString();
            var registerModel = new RegisterModel(email, password, "TestUser");

            // Act
            var firstResult = await _client.PostAsJsonAsync("auth/register", registerModel);
            var secondResult = await _client.PostAsJsonAsync("auth/register", registerModel);

            // Assert
            firstResult.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var registerResultModel = await firstResult.Content.ReadFromJsonAsync<RegisterResult>();
            registerResultModel.Should().NotBeNull();
            registerResultModel!.Email.Should().Be(registerModel.Email);
            registerResultModel!.DisplayName.Should().Be(registerModel.DisplayName);

            secondResult.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }
    }
}
