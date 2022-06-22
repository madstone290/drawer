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

    /**
     * 사용자 시드데이터로 데이터 변경 테스트를 진행할 경우
     * 시드데이터와 불일치하는 문제가 있으므로 업데이트 테스트를 나누어서 진행한다
     * */
    [Collection(ApiInstanceCollection.Default)]
    public class UserControllerUpdateTest 
    {
        private readonly HttpClient _client;
        private readonly ITestOutputHelper _outputHelper;

        public UserControllerUpdateTest(ApiInstance apiInstance, ITestOutputHelper outputHelper)
        {
            _client = apiInstance.Client;
            _outputHelper = outputHelper;
        }
       
        [Theory]
        [ClassData(typeof(UserSeeds.EmailPassword))]
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
        [ClassData(typeof(UserSeeds.EmailPassword))]
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
        [ClassData(typeof(UserSeeds.EmailPassword))]
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
