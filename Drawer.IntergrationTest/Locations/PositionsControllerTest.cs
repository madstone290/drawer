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
    public class PositionsControllerTest
    {
        private readonly HttpClient _client;
        private readonly ITestOutputHelper _outputHelper;

        public PositionsControllerTest(ApiInstance apiInstance, ITestOutputHelper outputHelper)
        {
            _client = apiInstance.Client;
            _outputHelper = outputHelper;
        }


        async Task<long> CreateZone()
        {
            var zoneRequest = new CreateZoneRequest(Guid.NewGuid().ToString(), null);
            var zoneRequestMessage = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Zones.Create);
            zoneRequestMessage.Content = JsonContent.Create(zoneRequest);
            var zoneResponseMessage = await _client.SendAsyncWithMasterAuthentication(zoneRequestMessage);
            var zoneResponse = await zoneResponseMessage.Content.ReadFromJsonAsync<CreateZoneResponse>() ?? default!;
            return zoneResponse.Id;
        }

        [Theory]
        [InlineData("P-1")]
        public async Task CreatePosition_Returns_Ok_With_Content(string name) 
        {
            // Arrange
            var zoneId = await CreateZone();
            var request = new CreatePositionRequest(zoneId, name);
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Positions.Create);
            requestMessage.Content = JsonContent.Create(request);

            // Act
            var responseMessage = await _client.SendAsyncWithMasterAuthentication(requestMessage);

            // Assert
            responseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var response = await responseMessage.Content.ReadFromJsonAsync<CreatePositionResponse>();
            response.Should().NotBeNull();
            response!.Name.Should().Be(request.Name);
        }

        [Theory]
        [InlineData("P-1-1")]
        public async Task GetPosition_Returns_Ok_With_CreatedPosition(string name)
        {
            // Arrange
            var zoneId = await CreateZone();
            var createRequest = new CreatePositionRequest(zoneId, name);
            var createRequestMessage = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Positions.Create);
            createRequestMessage.Content = JsonContent.Create(createRequest);
            var createResponseMessage = await _client.SendAsyncWithMasterAuthentication(createRequestMessage);
            var createResponse = await createResponseMessage.Content.ReadFromJsonAsync<CreatePositionResponse>() ?? null!;

            // Act
            var getRequestMessage = new HttpRequestMessage(HttpMethod.Get,
                ApiRoutes.Positions.Get.Replace("{id}", createResponse.Id.ToString()));
            var getResponseMessage = await _client.SendAsyncWithMasterAuthentication(getRequestMessage);

            // Assert
            getResponseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var getResponse = await getResponseMessage.Content.ReadFromJsonAsync<GetPositionResponse>() ?? null!;
            getResponse.Should().NotBeNull();
            getResponse.Id.Should().Be(createResponse.Id);
            getResponse.Name.Should().Be(createRequest.Name);
        }

        [Theory]
        [InlineData("P-1-1", "P-1-3")]
        public async Task GetPositions_Returns_Ok_With_CreatedPositions(string name1,string name2)
        {
            // Arrange
            var zoneId1 = await CreateZone();
            var createRequest1 = new CreatePositionRequest(zoneId1, name1);
            var createRequestMessage1 = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Positions.Create);
            createRequestMessage1.Content = JsonContent.Create(createRequest1);
            var createResponseMessage1 = await _client.SendAsyncWithMasterAuthentication(createRequestMessage1);

            var zoneId2 = await CreateZone();
            var createRequest2 = new CreatePositionRequest(zoneId2, name2);
            var createRequestMessage2 = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Positions.Create);
            createRequestMessage2.Content = JsonContent.Create(createRequest2);
            var createResponseMessage2 = await _client.SendAsyncWithMasterAuthentication(createRequestMessage2);

            // Act
            var getPositionsRequestMessage = new HttpRequestMessage(HttpMethod.Get, ApiRoutes.Positions.GetList);
            var getPositionsResponseMessage = await _client.SendAsyncWithMasterAuthentication(getPositionsRequestMessage);

            // Assert
            getPositionsResponseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var getPositionsResponse = await getPositionsResponseMessage.Content.ReadFromJsonAsync<GetPositionsResponse>() ?? null!;
            getPositionsResponse.Should().NotBeNull();
            getPositionsResponse.Positions.Should().NotBeNull();
            getPositionsResponse.Positions.Should().Contain(x=> x.Name == name1);
            getPositionsResponse.Positions.Should().Contain(x => x.Name == name2);
        }

        [Theory]
        [InlineData("P-3-4", "P-23-1")]
        public async Task UpdatePosition_Returns_Ok(string name, string name2)
        {
            // Arrange
            var zoneId1 = await CreateZone();
            var createRequest = new CreatePositionRequest(zoneId1, name);
            var createRequestMessage = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Positions.Create);
            createRequestMessage.Content = JsonContent.Create(createRequest);
            var createResponseMessage = await _client.SendAsyncWithMasterAuthentication(createRequestMessage);
            var createResponse = await createResponseMessage.Content.ReadFromJsonAsync<CreatePositionResponse>() ?? null!;

            // Act
            var zoneId2 = await CreateZone();
            var updateRequest = new CreatePositionRequest(zoneId2, name2);
            var updateRequestMessage = new HttpRequestMessage(HttpMethod.Put,
                ApiRoutes.Positions.Update.Replace("{id}", createResponse.Id.ToString()));
            updateRequestMessage.Content = JsonContent.Create(updateRequest);
            var updateResponseMessage = await _client.SendAsyncWithMasterAuthentication(updateRequestMessage);

            // Assert
            updateResponseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var getRequestMessage = new HttpRequestMessage(HttpMethod.Get, 
                ApiRoutes.Positions.Get.Replace("{id}", createResponse.Id.ToString()));
            var getResponseMessage = await _client.SendAsyncWithMasterAuthentication(getRequestMessage);
            getResponseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var getResponse = await getResponseMessage.Content.ReadFromJsonAsync<GetPositionResponse>() ?? null!;
            getResponse.Should().NotBeNull();
            getResponse.Id.Should().Be(createResponse.Id);
            getResponse.Name.Should().Be(updateRequest.Name);
        }

        [Theory]
        [InlineData("P-34-4")]
        public async Task DeletePosition_Returns_Ok(string name)
        {
            // Arrange
            var zoneId = await CreateZone();
            var createRequest = new CreatePositionRequest(zoneId, name);
            var createRequestMessage = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Positions.Create);
            createRequestMessage.Content = JsonContent.Create(createRequest);
            var createResponseMessage = await _client.SendAsyncWithMasterAuthentication(createRequestMessage);
            var createResponse = await createResponseMessage.Content.ReadFromJsonAsync<CreatePositionResponse>() ?? null!;

            // Act
            var deleteRequestMessage = new HttpRequestMessage(HttpMethod.Delete,
                ApiRoutes.Positions.Delete.Replace("{id}", createResponse.Id.ToString()));
            var deleteResponseMessage = await _client.SendAsyncWithMasterAuthentication(deleteRequestMessage);

            // Assert
            deleteResponseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var getRequestMessage = new HttpRequestMessage(HttpMethod.Get,
                ApiRoutes.Positions.Get.Replace("{id}", createResponse.Id.ToString()));
            var getResponseMessage = await _client.SendAsyncWithMasterAuthentication(getRequestMessage);
            getResponseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        }

    }
}
