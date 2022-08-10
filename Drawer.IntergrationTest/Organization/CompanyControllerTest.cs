using Drawer.Shared;
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
using Drawer.Shared.Contracts.Common;
using Drawer.Application.Services.UserInformation.QueryModels;
using Drawer.Application.Services.Organization.CommandModels;
using Drawer.Application.Services.Organization.QueryModels;
using Drawer.Application.Services.Authentication.CommandModels;

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
            var responseMessage = await _client.GetAsync(ApiRoutes.Company.Get);

            // Assert
            responseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
        }

        [Theory]
        [ClassData(typeof(CompanyControllerTestData.User1Company))]
        public async Task OnlyOneCompany_CanBe_Created(string email, string password, string name, string phoneNumber, string name2, string phoneNumber2)
        {
            // Arrange
            var loginRequest = new LoginCommandModel(email, password);
            var loginResponseMessage = await _client.PostAsJsonAsync(ApiRoutes.Account.Login, loginRequest);
            var loginResponse = await loginResponseMessage.Content.ReadFromJsonAsync<LoginResponseCommandModel>();

            var getUserRequestMessage = new HttpRequestMessage(HttpMethod.Get, ApiRoutes.User.Get);
            getUserRequestMessage.SetBearerToken(loginResponse!.AccessToken);
            var getUserResponseMessage = await _client.SendAsync(getUserRequestMessage);
            var getUserResponse = await getUserResponseMessage.Content!.ReadFromJsonAsync<UserInfoQueryModel>();

            var companyDto1 = new CompanyAddUpdateCommandModel()
            {
                Name = name,
                PhoneNumber = phoneNumber
            };
            var createCompanyRequestMessage = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Company.Create);
            createCompanyRequestMessage.Content = JsonContent.Create(companyDto1);
            createCompanyRequestMessage.SetBearerToken(loginResponse!.AccessToken);

            // Act
            var createCompanyResponseMessage = await _client.SendAsync(createCompanyRequestMessage);

            var companyDto2 = new CompanyAddUpdateCommandModel()
            {
                Name = name2,
                PhoneNumber = phoneNumber2
            };
            var createCompanyRequestMessage2 = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Company.Create);
            createCompanyRequestMessage2.Content = JsonContent.Create(companyDto2);
            createCompanyRequestMessage2.SetBearerToken(loginResponse!.AccessToken);
            var createCompanyResponseMessage2 = await _client.SendAsync(createCompanyRequestMessage2);

            // Assert
            createCompanyResponseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var companyId = await createCompanyResponseMessage.Content.ReadFromJsonAsync<string>();
            companyId.Should().NotBeNull();

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
            var loginRequest = new LoginCommandModel(email, password);
            var loginResponseMessage = await _client.PostAsJsonAsync(ApiRoutes.Account.Login, loginRequest);
            var loginResponse = await loginResponseMessage.Content.ReadFromJsonAsync<LoginResponseCommandModel>();

            var getUserRequestMessage = new HttpRequestMessage(HttpMethod.Get, ApiRoutes.User.Get);
            getUserRequestMessage.SetBearerToken(loginResponse!.AccessToken);
            var getUserResponseMessage = await _client.SendAsync(getUserRequestMessage);
            var userInfo = await getUserResponseMessage.Content!.ReadFromJsonAsync<UserInfoQueryModel>();

            var companyDto = new CompanyAddUpdateCommandModel()
            {
                Name = name,
                PhoneNumber = phoneNumber
            };
            var companyRequest = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Company.Create);
            companyRequest.Content = JsonContent.Create(companyDto);
            companyRequest.SetBearerToken(loginResponse!.AccessToken);
            // Act
            var companyResponse = await _client.SendAsync(companyRequest);

            // Assert
            companyResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var companyId = await companyResponse.Content.ReadFromJsonAsync<string>();
            companyId.Should().NotBeNull();
        }

        [Theory]
        [ClassData(typeof(CompanyControllerTestData.User3Company))]
        public async Task GetCompany_Returns_Ok_With_CompanyInfo(string email, string password, string name, string phoneNumber)
        {
            // Arrange
            var loginRequest = new LoginCommandModel(email, password);
            var loginResponseMessage = await _client.PostAsJsonAsync(ApiRoutes.Account.Login, loginRequest);
            var loginResponse = await loginResponseMessage.Content.ReadFromJsonAsync<LoginResponseCommandModel>();

            var companyDto = new CompanyAddUpdateCommandModel()
            {
                Name = name,
                PhoneNumber = phoneNumber
            }; 
            var createCompanyRequestMessage = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Company.Create);
            createCompanyRequestMessage.Content = JsonContent.Create(companyDto);
            createCompanyRequestMessage.SetBearerToken(loginResponse!.AccessToken);

            // Act
            var createCompanyResponseMessage = await _client.SendAsync(createCompanyRequestMessage);
            createCompanyResponseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var companyId = await createCompanyResponseMessage.Content.ReadFromJsonAsync<string>();
            companyId.Should().NotBeNull();

            // CompanyId클레임 획득을 위한 재로그인
            loginRequest = new LoginCommandModel(email, password);
            loginResponseMessage = await _client.PostAsJsonAsync(ApiRoutes.Account.Login, loginRequest);
            loginResponse = await loginResponseMessage.Content.ReadFromJsonAsync<LoginResponseCommandModel>();

            var getCompanyRequestMessage = new HttpRequestMessage(HttpMethod.Get, ApiRoutes.Company.Get);
            getCompanyRequestMessage.SetBearerToken(loginResponse!.AccessToken);
            var getResponse = await _client.SendAsync(getCompanyRequestMessage);

            // Assert
            getResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var company = await getResponse.Content.ReadFromJsonAsync<CompanyQueryModel?>() ?? default!;
            company.Should().NotBeNull();
            company.Id.Should().Be(companyId);
            company.Name.Should().Be(companyDto.Name);
            company.PhoneNumber.Should().Be(companyDto.PhoneNumber);
        }

        [Theory]
        [ClassData(typeof(CompanyControllerTestData.User4Company))]
        public async Task CreateCompany_Also_Creates_CompanyMember(string email, string password, string name, string phoneNumber)
        {
            // Arrange
            var loginRequest = new LoginCommandModel(email, password);
            var loginResponseMessage = await _client.PostAsJsonAsync(ApiRoutes.Account.Login, loginRequest);
            var loginResponse = await loginResponseMessage.Content.ReadFromJsonAsync<LoginResponseCommandModel>();

            var getUserRequestMessage = new HttpRequestMessage(HttpMethod.Get, ApiRoutes.User.Get);
            getUserRequestMessage.SetBearerToken(loginResponse!.AccessToken);
            var userResponse = await _client.SendAsync(getUserRequestMessage);
            var userInfo = await userResponse.Content.ReadFromJsonAsync<UserInfoQueryModel>() ?? default;

            var companyDto = new CompanyAddUpdateCommandModel()
            {
                Name = name,
                PhoneNumber = phoneNumber
            };
            var createCompanyRequestMessage = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Company.Create);
            createCompanyRequestMessage.Content = JsonContent.Create(companyDto);
            createCompanyRequestMessage.SetBearerToken(loginResponse!.AccessToken);

            // Act
            var createCompanyResponseMessage = await _client.SendAsync(createCompanyRequestMessage);
            createCompanyResponseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var companyId = await createCompanyResponseMessage.Content.ReadFromJsonAsync<string>();

            // CompanyId클레임 획득을 위한 재로그인
            loginRequest = new LoginCommandModel(email, password);
            loginResponseMessage = await _client.PostAsJsonAsync(ApiRoutes.Account.Login, loginRequest);
            loginResponse = await loginResponseMessage.Content.ReadFromJsonAsync<LoginResponseCommandModel>();

            var getCompanyMemberRequestMessage= new HttpRequestMessage(HttpMethod.Get, ApiRoutes.Company.GetMembers);
            getCompanyMemberRequestMessage.SetBearerToken(loginResponse!.AccessToken);
            var getCompanyMemberResponseMessage = await _client.SendAsync(getCompanyMemberRequestMessage);

            // Assert
            getCompanyMemberResponseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var members = await getCompanyMemberResponseMessage.Content.ReadFromJsonAsync<List<CompanyMemberQueryModel>>();
            members.Should().NotBeNull();
            members.Should().Contain(m => m.UserId == userInfo.UserId);
        }
      
    }
}
