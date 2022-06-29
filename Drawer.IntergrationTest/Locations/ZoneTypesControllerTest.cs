using Drawer.Contract;
using Drawer.Contract.Locations;
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

namespace Drawer.IntergrationTest.Locations
{
    [Collection(ApiInstanceCollection.Default)]
    public class ZoneTypesControllerTest
    {
        private readonly HttpClient _client;
        private readonly ITestOutputHelper _outputHelper;

        public ZoneTypesControllerTest(ApiInstance apiInstance, ITestOutputHelper outputHelper)
        {
            _client = apiInstance.Client;
            _outputHelper = outputHelper;
        }

        [Theory]
        [InlineData("ZT-1")]
        public async Task CreateZoneType_Returns_Ok_With_Content(string name) 
        {
            // Arrange
            var request = new CreateZoneTypeRequest(name);
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.ZoneTypes.Create);
            requestMessage.Content = JsonContent.Create(request);

            // Act
            var responseMessage = await _client.SendAsyncWithMasterAuthentication(requestMessage);

            // Assert
            responseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var response = await responseMessage.Content.ReadFromJsonAsync<CreateZoneTypeResponse>();
            response.Should().NotBeNull();
            response!.Name.Should().Be(request.Name);
        }

        [Theory]
        [InlineData("ZT-1-1")]
        public async Task GetZoneType_Returns_Ok_With_CreatedZoneType(string name)
        {
            // Arrange
            var createRequest = new CreateZoneTypeRequest(name);
            var createRequestMessage = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.ZoneTypes.Create);
            createRequestMessage.Content = JsonContent.Create(createRequest);
            var createResponseMessage = await _client.SendAsyncWithMasterAuthentication(createRequestMessage);
            var createResponse = await createResponseMessage.Content.ReadFromJsonAsync<CreateZoneTypeResponse>() ?? null!;

            // Act
            var getRequestMessage = new HttpRequestMessage(HttpMethod.Get,
                ApiRoutes.ZoneTypes.Get.Replace("{id}", createResponse.Id.ToString()));
            var getResponseMessage = await _client.SendAsyncWithMasterAuthentication(getRequestMessage);

            // Assert
            getResponseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var getResponse = await getResponseMessage.Content.ReadFromJsonAsync<GetZoneTypeResponse>() ?? null!;
            getResponse.Should().NotBeNull();
            getResponse.Id.Should().Be(createResponse.Id);
            getResponse.Name.Should().Be(createRequest.Name);
        }

        [Theory]
        [InlineData("ZT-1-1", "ZT-1-3")]
        public async Task GetZoneTypes_Returns_Ok_With_CreatedZoneTypes(string name1,string name2)
        {
            // Arrange
            var createRequest1 = new CreateZoneTypeRequest(name1);
            var createRequestMessage1 = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.ZoneTypes.Create);
            createRequestMessage1.Content = JsonContent.Create(createRequest1);
            var createResponseMessage1 = await _client.SendAsyncWithMasterAuthentication(createRequestMessage1);

            var createRequest2 = new CreateZoneTypeRequest(name2);
            var createRequestMessage2 = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.ZoneTypes.Create);
            createRequestMessage2.Content = JsonContent.Create(createRequest2);
            var createResponseMessage2 = await _client.SendAsyncWithMasterAuthentication(createRequestMessage2);

            // Act
            var getZoneTypesRequestMessage = new HttpRequestMessage(HttpMethod.Get, ApiRoutes.ZoneTypes.GetList);
            var getZoneTypesResponseMessage = await _client.SendAsyncWithMasterAuthentication(getZoneTypesRequestMessage);

            // Assert
            getZoneTypesResponseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var getZoneTypesResponse = await getZoneTypesResponseMessage.Content.ReadFromJsonAsync<GetZoneTypesResponse>() ?? null!;
            getZoneTypesResponse.Should().NotBeNull();
            getZoneTypesResponse.ZoneTypes.Should().NotBeNull();
            getZoneTypesResponse.ZoneTypes.Should().Contain(x=> x.Name == name1);
            getZoneTypesResponse.ZoneTypes.Should().Contain(x => x.Name == name2);
        }

        [Theory]
        [InlineData("ZT-3-4", "ZT-23-1")]
        public async Task UpdateZoneType_Returns_Ok(string name, string name2)
        {
            // Arrange
            var createRequest = new CreateZoneTypeRequest(name);
            var createRequestMessage = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.ZoneTypes.Create);
            createRequestMessage.Content = JsonContent.Create(createRequest);
            var createResponseMessage = await _client.SendAsyncWithMasterAuthentication(createRequestMessage);
            var createResponse = await createResponseMessage.Content.ReadFromJsonAsync<CreateZoneTypeResponse>() ?? null!;

            // Act
            var updateRequest = new CreateZoneTypeRequest(name2);
            var updateRequestMessage = new HttpRequestMessage(HttpMethod.Put,
                ApiRoutes.ZoneTypes.Update.Replace("{id}", createResponse.Id.ToString()));
            updateRequestMessage.Content = JsonContent.Create(updateRequest);
            var updateResponseMessage = await _client.SendAsyncWithMasterAuthentication(updateRequestMessage);

            // Assert
            updateResponseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var getRequestMessage = new HttpRequestMessage(HttpMethod.Get, 
                ApiRoutes.ZoneTypes.Get.Replace("{id}", createResponse.Id.ToString()));
            var getResponseMessage = await _client.SendAsyncWithMasterAuthentication(getRequestMessage);
            getResponseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var getResponse = await getResponseMessage.Content.ReadFromJsonAsync<GetZoneTypeResponse>() ?? null!;
            getResponse.Should().NotBeNull();
            getResponse.Id.Should().Be(createResponse.Id);
            getResponse.Name.Should().Be(updateRequest.Name);
        }

        [Theory]
        [InlineData("ZT-34-4")]
        public async Task DeleteZoneType_Returns_Ok(string name)
        {
            // Arrange
            var createRequest = new CreateZoneTypeRequest(name);
            var createRequestMessage = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.ZoneTypes.Create);
            createRequestMessage.Content = JsonContent.Create(createRequest);
            var createResponseMessage = await _client.SendAsyncWithMasterAuthentication(createRequestMessage);
            var createResponse = await createResponseMessage.Content.ReadFromJsonAsync<CreateZoneTypeResponse>() ?? null!;

            // Act
            var deleteRequestMessage = new HttpRequestMessage(HttpMethod.Delete,
                ApiRoutes.ZoneTypes.Delete.Replace("{id}", createResponse.Id.ToString()));
            var deleteResponseMessage = await _client.SendAsyncWithMasterAuthentication(deleteRequestMessage);

            // Assert
            deleteResponseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var getRequestMessage = new HttpRequestMessage(HttpMethod.Get,
                ApiRoutes.ZoneTypes.Get.Replace("{id}", createResponse.Id.ToString()));
            var getResponseMessage = await _client.SendAsyncWithMasterAuthentication(getRequestMessage);
            getResponseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        }

    }
}
