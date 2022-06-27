using Drawer.Contract;
using Drawer.Contract.Authentication;
using Drawer.Contract.Common;
using Drawer.Contract.Organization;
using Drawer.Contract.UserInformation;
using Drawer.IntergrationTest.Seeds;
using Drawer.IntergrationTest.TheoryData;
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

namespace Drawer.IntergrationTest.Organization
{
    [Collection(ApiInstanceCollection.Default)]
    public class CompanyControllerTest
    {
        private readonly HttpClient _client;
        private readonly ITestOutputHelper _outputHelper;

        public CompanyControllerTest(ApiInstance apiInstance, ITestOutputHelper outputHelper)
        {
            _client = apiInstance.Client;
            _outputHelper = outputHelper;
        }

        [Fact]
        public async Task GetCompany_WithoutToken_Returns_Unauthorized()
        {
            // Arrange

            // Act
            var responseMessage = await _client.GetAsync(ApiRoutes.Company.GetCompany);

            // Assert
            responseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
        }

        [Theory]
        [ClassData(typeof(CompanyControllerTestData.User1Company))]
        public async Task OnlyOneCompany_CanBe_Created(string email, string password, string name, string phoneNumber, string name2, string phoneNumber2)
        {
            // Arrange
            var loginRequest = new LoginRequest(email, password);
            var loginResponseMessage = await _client.PostAsJsonAsync(ApiRoutes.Account.Login, loginRequest);
            var loginResponse = await loginResponseMessage.Content.ReadFromJsonAsync<LoginResponse>();

            var getUserRequestMessage = new HttpRequestMessage(HttpMethod.Get, ApiRoutes.User.GetUser);
            getUserRequestMessage.SetBearerToken(loginResponse!.AccessToken);
            var getUserResponseMessage = await _client.SendAsync(getUserRequestMessage);
            var getUserResponse = await getUserResponseMessage.Content!.ReadFromJsonAsync<GetUserResponse>();

            var creatCompanyRequest = new CreateCompanyRequest(name, phoneNumber);
            var createCompanyRequestMessage = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Company.CreateCompany);
            createCompanyRequestMessage.Content = JsonContent.Create(creatCompanyRequest);
            createCompanyRequestMessage.SetBearerToken(loginResponse!.AccessToken);

            // Act
            var createCompanyResponseMessage = await _client.SendAsync(createCompanyRequestMessage);

            var creatCompanyRequest2 = new CreateCompanyRequest(name2, phoneNumber2);
            var createCompanyRequestMessage2 = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Company.CreateCompany);
            createCompanyRequestMessage2.Content = JsonContent.Create(creatCompanyRequest2);
            createCompanyRequestMessage2.SetBearerToken(loginResponse!.AccessToken);
            var createCompanyResponseMessage2 = await _client.SendAsync(createCompanyRequestMessage2);

            // Assert
            createCompanyResponseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var createCompanyResponse = await createCompanyResponseMessage.Content.ReadFromJsonAsync<CreateCompanyResponse>();
            createCompanyResponse.Should().NotBeNull();
            createCompanyResponse!.Name.Should().Be(creatCompanyRequest.Name);
            createCompanyResponse!.PhoneNumber.Should().Be(creatCompanyRequest.PhoneNumber);
            createCompanyResponse!.OwnerId.Should().Be(getUserResponse!.UserId);

            createCompanyResponseMessage2.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            var createCompanyResponse2 = await createCompanyResponseMessage2.Content.ReadFromJsonAsync<ErrorResponse>();
            createCompanyResponse2.Should().NotBeNull();
            _outputHelper.WriteLine(createCompanyResponse2!.Message);
        }

        [Theory]
        [ClassData(typeof(CompanyControllerTestData.User2Company))]
        public async Task CreateCompany_Returns_Ok_With_CompanyInfo(string email, string password, string name, string phoneNumber)
        {
            // Arrange
            var loginRequest = new LoginRequest(email, password);
            var loginResponseMessage = await _client.PostAsJsonAsync(ApiRoutes.Account.Login, loginRequest);
            var loginResponse = await loginResponseMessage.Content.ReadFromJsonAsync<LoginResponse>();

            var getUserRequestMessage = new HttpRequestMessage(HttpMethod.Get, ApiRoutes.User.GetUser);
            getUserRequestMessage.SetBearerToken(loginResponse!.AccessToken);
            var getUserResponseMessage = await _client.SendAsync(getUserRequestMessage);
            var getUserResponse = await getUserResponseMessage.Content!.ReadFromJsonAsync<GetUserResponse>();

            var creatCompanyRequest = new CreateCompanyRequest(name, phoneNumber);
            var createCompanyRequestMessage = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Company.CreateCompany);
            createCompanyRequestMessage.Content = JsonContent.Create(creatCompanyRequest);
            createCompanyRequestMessage.SetBearerToken(loginResponse!.AccessToken);
            // Act
            var createCompanyResponseMessage = await _client.SendAsync(createCompanyRequestMessage);

            // Assert
            createCompanyResponseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var createCompanyResponse = await createCompanyResponseMessage.Content.ReadFromJsonAsync<CreateCompanyResponse>();
            createCompanyResponse.Should().NotBeNull();
            createCompanyResponse!.Name.Should().Be(creatCompanyRequest.Name);
            createCompanyResponse!.PhoneNumber.Should().Be(creatCompanyRequest.PhoneNumber);
            createCompanyResponse!.OwnerId.Should().Be(getUserResponse!.UserId);
        }

        [Theory]
        [ClassData(typeof(CompanyControllerTestData.User3Company))]
        public async Task GetCompany_Returns_Ok_With_CompanyInfo(string email, string password, string name, string phoneNumber)
        {
            // Arrange
            var loginRequest = new LoginRequest(email, password);
            var loginResponseMessage = await _client.PostAsJsonAsync(ApiRoutes.Account.Login, loginRequest);
            var loginResponse = await loginResponseMessage.Content.ReadFromJsonAsync<LoginResponse>();

            var creatCompanyRequest = new CreateCompanyRequest(name, phoneNumber);
            var createCompanyRequestMessage = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Company.CreateCompany);
            createCompanyRequestMessage.Content = JsonContent.Create(creatCompanyRequest);
            createCompanyRequestMessage.SetBearerToken(loginResponse!.AccessToken);

            // Act
            var createCompanyResponseMessage = await _client.SendAsync(createCompanyRequestMessage);
            createCompanyResponseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var createCompanyResponse = await createCompanyResponseMessage.Content.ReadFromJsonAsync<CreateCompanyResponse>();
            createCompanyResponse.Should().NotBeNull();

            // CompanyId클레임 획득을 위한 재로그인
            loginRequest = new LoginRequest(email, password);
            loginResponseMessage = await _client.PostAsJsonAsync(ApiRoutes.Account.Login, loginRequest);
            loginResponse = await loginResponseMessage.Content.ReadFromJsonAsync<LoginResponse>();

            var getCompanyRequestMessage = new HttpRequestMessage(HttpMethod.Get, ApiRoutes.Company.GetCompany);
            getCompanyRequestMessage.SetBearerToken(loginResponse!.AccessToken);
            var getCompanyResponseMessage = await _client.SendAsync(getCompanyRequestMessage);

            // Assert
            getCompanyResponseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var getCompanyResponse = await getCompanyResponseMessage.Content.ReadFromJsonAsync<GetCompanyResponse>();
            getCompanyResponse.Should().NotBeNull();
            getCompanyResponse!.Id.Should().Be(createCompanyResponse!.Id);
            getCompanyResponse!.Name.Should().Be(creatCompanyRequest.Name);
            getCompanyResponse!.PhoneNumber.Should().Be(creatCompanyRequest.PhoneNumber);
        }

        [Theory]
        [ClassData(typeof(CompanyControllerTestData.User4Company))]
        public async Task CreateCompany_Also_Creates_CompanyMember(string email, string password, string name, string phoneNumber)
        {
            // Arrange
            var loginRequest = new LoginRequest(email, password);
            var loginResponseMessage = await _client.PostAsJsonAsync(ApiRoutes.Account.Login, loginRequest);
            var loginResponse = await loginResponseMessage.Content.ReadFromJsonAsync<LoginResponse>();

            var getUserRequestMessage = new HttpRequestMessage(HttpMethod.Get, ApiRoutes.User.GetUser);
            getUserRequestMessage.SetBearerToken(loginResponse!.AccessToken);
            var getUserResponseMessage = await _client.SendAsync(getUserRequestMessage);
            var getUserResponse = await getUserResponseMessage.Content!.ReadFromJsonAsync<GetUserResponse>();

            var createCompanyRequest = new CreateCompanyRequest(name, phoneNumber);
            var createCompanyRequestMessage = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Company.CreateCompany);
            createCompanyRequestMessage.Content = JsonContent.Create(createCompanyRequest);
            createCompanyRequestMessage.SetBearerToken(loginResponse!.AccessToken);

            // Act
            var createCompanyResponseMessage = await _client.SendAsync(createCompanyRequestMessage);
            createCompanyResponseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var createCompanyResponse = await createCompanyResponseMessage.Content.ReadFromJsonAsync<CreateCompanyResponse>();

            // CompanyId클레임 획득을 위한 재로그인
            loginRequest = new LoginRequest(email, password);
            loginResponseMessage = await _client.PostAsJsonAsync(ApiRoutes.Account.Login, loginRequest);
            loginResponse = await loginResponseMessage.Content.ReadFromJsonAsync<LoginResponse>();

            var getCompanyMemberRequestMessage= new HttpRequestMessage(HttpMethod.Get, ApiRoutes.Company.GetCompanyMembers);
            getCompanyMemberRequestMessage.SetBearerToken(loginResponse!.AccessToken);
            var getCompanyMemberResponseMessage = await _client.SendAsync(getCompanyMemberRequestMessage);

            // Assert
            getCompanyMemberResponseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var getCompanyMemberResponse = await getCompanyMemberResponseMessage.Content.ReadFromJsonAsync<GetCompanyMembersResponse>();
            getCompanyMemberResponse!.Should().NotBeNull();
            getCompanyMemberResponse!.Members.Should().Contain(m => m.UserId == getUserResponse!.UserId);
        }
      
    }
}
