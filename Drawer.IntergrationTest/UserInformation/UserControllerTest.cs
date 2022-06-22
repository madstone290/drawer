using Drawer.Contract;
using Drawer.Contract.Authentication;
using Drawer.Contract.UserInformation;
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
        [ClassData(typeof(UserSeeds.EmailPasswordDisplayName))]
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

    }
}
