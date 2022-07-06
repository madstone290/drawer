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

        async Task<long> CreateWorkPlace()
        {
            var request = new CreateWorkPlaceRequest(Guid.NewGuid().ToString(), null);
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.WorkPlaces.Create);
            requestMessage.Content = JsonContent.Create(request);
            var ResponseMessage = await _client.SendAsyncWithMasterAuthentication(requestMessage);
            var Response = await ResponseMessage.Content.ReadFromJsonAsync<CreateWorkPlaceResponse>() ?? default!;
            return Response.Id;
        }

        [Theory]
        [InlineData("Z-1", "note1")]
        public async Task CreateZone_Returns_Ok_With_Content(string name, string note)
        {
            // Arrange
            var workPlaceId = await CreateWorkPlace();
            var request = new CreateZoneRequest(workPlaceId, name, note);
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Zones.Create);
            requestMessage.Content = JsonContent.Create(request);

            // Act
            var responseMessage = await _client.SendAsyncWithMasterAuthentication(requestMessage);

            // Assert
            responseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var response = await responseMessage.Content.ReadFromJsonAsync<CreateZoneResponse>() ?? default!;
            response.Should().NotBeNull();
        }

        [Theory]
        [InlineData("Z-1-1", "note1")]
        public async Task GetZone_Returns_Ok_With_CreatedZone(string name, string note)
        {
            // Arrange
            var workPlaceId = await CreateWorkPlace();
            var createRequest = new CreateZoneRequest(workPlaceId, name, note);
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
            getResponse.Note.Should().Be(createRequest.Note);
        }

        [Theory]
        [InlineData("Z-1-1", "note1", "Z-1-3", "note2")]
        public async Task GetZones_Returns_Ok_With_CreatedZones(string name1, string note1, string name2, string note2)
        {
            // Arrange
            var workPlaceId1 = await CreateWorkPlace();
            var createRequest1 = new CreateZoneRequest(workPlaceId1, name1, note1);
            var createRequestMessage1 = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Zones.Create);
            createRequestMessage1.Content = JsonContent.Create(createRequest1);
            var createResponseMessage1 = await _client.SendAsyncWithMasterAuthentication(createRequestMessage1);

            var workPlaceId2 = await CreateWorkPlace();
            var createRequest2 = new CreateZoneRequest(workPlaceId2, name2, note2);
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
            getZonesResponse.Zones.Should().Contain(x => x.WorkPlaceId == workPlaceId1 && x.Name == name1 && x.Note == note1);
            getZonesResponse.Zones.Should().Contain(x => x.WorkPlaceId == workPlaceId2 && x.Name == name2 && x.Note == note2);
        }

        [Theory]
        [InlineData("Z-3-4", "note1", "Z-23-1", "note2")]
        public async Task UpdateZone_Returns_Ok(string name1, string note1, string name2, string note2)
        {
            // Arrange
            var workPlaceId = await CreateWorkPlace();
            var createRequest = new CreateZoneRequest(workPlaceId, name1, note1);
            var createRequestMessage = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Zones.Create);
            createRequestMessage.Content = JsonContent.Create(createRequest);
            var createResponseMessage = await _client.SendAsyncWithMasterAuthentication(createRequestMessage);
            var createResponse = await createResponseMessage.Content.ReadFromJsonAsync<CreateZoneResponse>() ?? null!;

            // Act
            var updateRequest = new UpdateZoneRequest(name2, note2);
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
            getResponse.Note.Should().Be(updateRequest.Note);
        }

        [Theory]
        [InlineData("Z-34-4", "note")]
        public async Task DeleteZone_Returns_Ok(string name, string note)
        {
            // Arrange
            var workPlaceId = await CreateWorkPlace();
            var createRequest = new CreateZoneRequest(workPlaceId, name, note);
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
