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
    public class ZonesControllerTest
    {
        private readonly HttpClient _client;
        private readonly ITestOutputHelper _outputHelper;

        public ZonesControllerTest(ApiInstance apiInstance, ITestOutputHelper outputHelper)
        {
            _client = apiInstance.Client;
            _outputHelper = outputHelper;
        }

        async Task<long> CreateZoneType()
        {
            var zoneTypeRequest = new CreateZoneTypeRequest(Guid.NewGuid().ToString());
            var zoneTypeRequestMessage = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.ZoneTypes.Create);
            zoneTypeRequestMessage.Content = JsonContent.Create(zoneTypeRequest);
            var zoneTypeResponseMessage = await _client.SendAsyncWithMasterAuthentication(zoneTypeRequestMessage);
            var zoneTypeResponse = await zoneTypeResponseMessage.Content.ReadFromJsonAsync<CreateZoneTypeResponse>() ?? default!;
            return zoneTypeResponse.Id;
        }

        [Theory]
        [InlineData("Z-1")]
        public async Task CreateZone_Returns_Ok_With_Content(string name) 
        {
            // Arrange
            var zoneTypeId = await CreateZoneType();
            var request = new CreateZoneRequest(name, zoneTypeId);
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Zones.Create);
            requestMessage.Content = JsonContent.Create(request);

            // Act
            var responseMessage = await _client.SendAsyncWithMasterAuthentication(requestMessage);

            // Assert
            responseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var response = await responseMessage.Content.ReadFromJsonAsync<CreateZoneResponse>() ?? default!;
            response.Should().NotBeNull();
            response.Name.Should().Be(request.Name);
            response.ZoneTypeId.Should().Be(request.ZoneTypeId);
        }

        [Theory]
        [InlineData("Z-1-1")]
        public async Task GetZone_Returns_Ok_With_CreatedZone(string name)
        {
            // Arrange
            var zoneTypeId = await CreateZoneType();
            var createRequest = new CreateZoneRequest(name, zoneTypeId);
            var createRequestMessage = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Zones.Create);
            createRequestMessage.Content = JsonContent.Create(createRequest);
            var createResponseMessage = await _client.SendAsyncWithMasterAuthentication(createRequestMessage);
            var createResponse = await createResponseMessage.Content.ReadFromJsonAsync<CreateZoneResponse>() ?? null!;

            // Act
            var getRequestMessage = new HttpRequestMessage(HttpMethod.Get,
                ApiRoutes.Zones.Get.Replace("{id}", createResponse.Id.ToString()));
            var getResponseMessage = await _client.SendAsyncWithMasterAuthentication(getRequestMessage);

            // Assert
            getResponseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var getResponse = await getResponseMessage.Content.ReadFromJsonAsync<GetZoneResponse>() ?? null!;
            getResponse.Should().NotBeNull();
            getResponse.Id.Should().Be(createResponse.Id);
            getResponse.Name.Should().Be(createRequest.Name);
        }

        [Theory]
        [InlineData("Z-1-1", "Z-1-3")]
        public async Task GetZones_Returns_Ok_With_CreatedZones(string name1,string name2)
        {
            // Arrange
            var zoneTypeId1 = await CreateZoneType();
            var createRequest1 = new CreateZoneRequest(name1, zoneTypeId1);
            var createRequestMessage1 = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Zones.Create);
            createRequestMessage1.Content = JsonContent.Create(createRequest1);
            var createResponseMessage1 = await _client.SendAsyncWithMasterAuthentication(createRequestMessage1);

            var zoneTypeId2 = await CreateZoneType();
            var createRequest2 = new CreateZoneRequest(name2, zoneTypeId2);
            var createRequestMessage2 = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Zones.Create);
            createRequestMessage2.Content = JsonContent.Create(createRequest2);
            var createResponseMessage2 = await _client.SendAsyncWithMasterAuthentication(createRequestMessage2);

            // Act
            var getZonesRequestMessage = new HttpRequestMessage(HttpMethod.Get, ApiRoutes.Zones.GetList);
            var getZonesResponseMessage = await _client.SendAsyncWithMasterAuthentication(getZonesRequestMessage);

            // Assert
            getZonesResponseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var getZonesResponse = await getZonesResponseMessage.Content.ReadFromJsonAsync<GetZonesResponse>() ?? null!;
            getZonesResponse.Should().NotBeNull();
            getZonesResponse.Zones.Should().NotBeNull();
            getZonesResponse.Zones.Should().Contain(x=> x.Name == name1 && x.ZoneTypeId == zoneTypeId1);
            getZonesResponse.Zones.Should().Contain(x => x.Name == name2 && x.ZoneTypeId == zoneTypeId2);
        }

        [Theory]
        [InlineData("Z-3-4", "Z-23-1")]
        public async Task UpdateZone_Returns_Ok(string name, string name2)
        {
            // Arrange
            var zoneTypeId1 = await CreateZoneType();
            var createRequest = new CreateZoneRequest(name, zoneTypeId1);
            var createRequestMessage = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Zones.Create);
            createRequestMessage.Content = JsonContent.Create(createRequest);
            var createResponseMessage = await _client.SendAsyncWithMasterAuthentication(createRequestMessage);
            var createResponse = await createResponseMessage.Content.ReadFromJsonAsync<CreateZoneResponse>() ?? null!;

            // Act
            var zoneTypeId2 = await CreateZoneType();
            var updateRequest = new CreateZoneRequest(name2, zoneTypeId2);
            var updateRequestMessage = new HttpRequestMessage(HttpMethod.Put,
                ApiRoutes.Zones.Update.Replace("{id}", createResponse.Id.ToString()));
            updateRequestMessage.Content = JsonContent.Create(updateRequest);
            var updateResponseMessage = await _client.SendAsyncWithMasterAuthentication(updateRequestMessage);

            // Assert
            updateResponseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var getRequestMessage = new HttpRequestMessage(HttpMethod.Get, 
                ApiRoutes.Zones.Get.Replace("{id}", createResponse.Id.ToString()));
            var getResponseMessage = await _client.SendAsyncWithMasterAuthentication(getRequestMessage);
            getResponseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var getResponse = await getResponseMessage.Content.ReadFromJsonAsync<GetZoneResponse>() ?? null!;
            getResponse.Should().NotBeNull();
            getResponse.Id.Should().Be(createResponse.Id);
            getResponse.Name.Should().Be(updateRequest.Name);
            getResponse.ZoneTypeId.Should().Be(updateRequest.ZoneTypeId);
        }

        [Theory]
        [InlineData("Z-34-4")]
        public async Task DeleteZone_Returns_Ok(string name)
        {
            // Arrange
            var zoneTypeId = await CreateZoneType();
            var createRequest = new CreateZoneRequest(name, zoneTypeId);
            var createRequestMessage = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Zones.Create);
            createRequestMessage.Content = JsonContent.Create(createRequest);
            var createResponseMessage = await _client.SendAsyncWithMasterAuthentication(createRequestMessage);
            var createResponse = await createResponseMessage.Content.ReadFromJsonAsync<CreateZoneResponse>() ?? null!;

            // Act
            var deleteRequestMessage = new HttpRequestMessage(HttpMethod.Delete,
                ApiRoutes.Zones.Delete.Replace("{id}", createResponse.Id.ToString()));
            var deleteResponseMessage = await _client.SendAsyncWithMasterAuthentication(deleteRequestMessage);

            // Assert
            deleteResponseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var getRequestMessage = new HttpRequestMessage(HttpMethod.Get,
                ApiRoutes.Zones.Get.Replace("{id}", createResponse.Id.ToString()));
            var getResponseMessage = await _client.SendAsyncWithMasterAuthentication(getRequestMessage);
            getResponseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        }

    }
}
