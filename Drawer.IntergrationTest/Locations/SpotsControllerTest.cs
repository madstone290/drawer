﻿using Drawer.Contract;
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
    public class SpotsControllerTest
    {
        private readonly HttpClient _client;
        private readonly ITestOutputHelper _outputHelper;

        public SpotsControllerTest(ApiInstance apiInstance, ITestOutputHelper outputHelper)
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

        async Task<long> CreateZone()
        {
            var workPlaceId = await CreateWorkPlace();
            var zoneRequest = new CreateZoneRequest(workPlaceId, Guid.NewGuid().ToString(), null);
            var zoneRequestMessage = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Zones.Create);
            zoneRequestMessage.Content = JsonContent.Create(zoneRequest);
            var zoneResponseMessage = await _client.SendAsyncWithMasterAuthentication(zoneRequestMessage);
            var zoneResponse = await zoneResponseMessage.Content.ReadFromJsonAsync<CreateZoneResponse>() ?? default!;
            return zoneResponse.Id;
        }

        [Theory]
        [InlineData("P-1", "note1")]
        public async Task CreatePosition_Returns_Ok_With_Content(string name, string note) 
        {
            // Arrange
            var zoneId = await CreateZone();
            var request = new CreateSpotRequest(zoneId, name, note);
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Spots.Create);
            requestMessage.Content = JsonContent.Create(request);

            // Act
            var responseMessage = await _client.SendAsyncWithMasterAuthentication(requestMessage);

            // Assert
            responseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var response = await responseMessage.Content.ReadFromJsonAsync<CreateSpotResponse>();
            response.Should().NotBeNull();
        }

        [Theory]
        [InlineData("P-1-1", "note1")]
        public async Task GetPosition_Returns_Ok_With_CreatedPosition(string name, string note)
        {
            // Arrange
            var zoneId = await CreateZone();
            var createRequest = new CreateSpotRequest(zoneId, name, note);
            var createRequestMessage = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Spots.Create);
            createRequestMessage.Content = JsonContent.Create(createRequest);
            var createResponseMessage = await _client.SendAsyncWithMasterAuthentication(createRequestMessage);
            var createResponse = await createResponseMessage.Content.ReadFromJsonAsync<CreateSpotResponse>() ?? null!;

            // Act
            var getRequestMessage = new HttpRequestMessage(HttpMethod.Get,
                ApiRoutes.Spots.Get.Replace("{id}", createResponse.Id.ToString()));
            var getResponseMessage = await _client.SendAsyncWithMasterAuthentication(getRequestMessage);

            // Assert
            getResponseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var getResponse = await getResponseMessage.Content.ReadFromJsonAsync<GetSpotResponse>() ?? null!;
            getResponse.Should().NotBeNull();
            getResponse.Id.Should().Be(createResponse.Id);
            getResponse.Name.Should().Be(createRequest.Name);
            getResponse.Note.Should().Be(createRequest.Note);
        }

        [Theory]
        [InlineData("P-1-1", "note1", "P-1-3", "note2")]
        public async Task GetPositions_Returns_Ok_With_CreatedPositions(string name1, string note1, string name2, string note2)
        {
            // Arrange
            var zoneId1 = await CreateZone();
            var createRequest1 = new CreateSpotRequest(zoneId1, name1, note1);
            var createRequestMessage1 = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Spots.Create);
            createRequestMessage1.Content = JsonContent.Create(createRequest1);
            var createResponseMessage1 = await _client.SendAsyncWithMasterAuthentication(createRequestMessage1);

            var zoneId2 = await CreateZone();
            var createRequest2 = new CreateSpotRequest(zoneId2, name2, note2);
            var createRequestMessage2 = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Spots.Create);
            createRequestMessage2.Content = JsonContent.Create(createRequest2);
            var createResponseMessage2 = await _client.SendAsyncWithMasterAuthentication(createRequestMessage2);

            // Act
            var getPositionsRequestMessage = new HttpRequestMessage(HttpMethod.Get, ApiRoutes.Spots.GetList);
            var getPositionsResponseMessage = await _client.SendAsyncWithMasterAuthentication(getPositionsRequestMessage);

            // Assert
            getPositionsResponseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var getPositionsResponse = await getPositionsResponseMessage.Content.ReadFromJsonAsync<GetSpotsResponse>() ?? null!;
            getPositionsResponse.Should().NotBeNull();
            getPositionsResponse.Spots.Should().NotBeNull();
            getPositionsResponse.Spots.Should().Contain(x=> x.Name == name1 && x.Note == note1);
            getPositionsResponse.Spots.Should().Contain(x => x.Name == name2 && x.Note == note2);
        }

        [Theory]
        [InlineData("P-3-4","note1", "P-23-1", "note2")]
        public async Task UpdatePosition_Returns_Ok(string name1, string note1, string name2, string note2)
        {
            // Arrange
            var zoneId1 = await CreateZone();
            var createRequest = new CreateSpotRequest(zoneId1, name1, note1);
            var createRequestMessage = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Spots.Create);
            createRequestMessage.Content = JsonContent.Create(createRequest);
            var createResponseMessage = await _client.SendAsyncWithMasterAuthentication(createRequestMessage);
            var createResponse = await createResponseMessage.Content.ReadFromJsonAsync<CreateSpotResponse>() ?? null!;

            // Act
            var zoneId2 = await CreateZone();
            var updateRequest = new CreateSpotRequest(zoneId2, name2, note2);
            var updateRequestMessage = new HttpRequestMessage(HttpMethod.Put,
                ApiRoutes.Spots.Update.Replace("{id}", createResponse.Id.ToString()));
            updateRequestMessage.Content = JsonContent.Create(updateRequest);
            var updateResponseMessage = await _client.SendAsyncWithMasterAuthentication(updateRequestMessage);

            // Assert
            updateResponseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var getRequestMessage = new HttpRequestMessage(HttpMethod.Get, 
                ApiRoutes.Spots.Get.Replace("{id}", createResponse.Id.ToString()));
            var getResponseMessage = await _client.SendAsyncWithMasterAuthentication(getRequestMessage);
            getResponseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var getResponse = await getResponseMessage.Content.ReadFromJsonAsync<GetSpotResponse>() ?? null!;
            getResponse.Should().NotBeNull();
            getResponse.Id.Should().Be(createResponse.Id);
            getResponse.Name.Should().Be(updateRequest.Name);
            getResponse.Note.Should().Be(updateRequest.Note);
        }

        [Theory]
        [InlineData("P-34-4", "note1")]
        public async Task DeletePosition_Returns_Ok(string name, string note)
        {
            // Arrange
            var zoneId = await CreateZone();
            var createRequest = new CreateSpotRequest(zoneId, name, note);
            var createRequestMessage = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Spots.Create);
            createRequestMessage.Content = JsonContent.Create(createRequest);
            var createResponseMessage = await _client.SendAsyncWithMasterAuthentication(createRequestMessage);
            var createResponse = await createResponseMessage.Content.ReadFromJsonAsync<CreateSpotResponse>() ?? null!;

            // Act
            var deleteRequestMessage = new HttpRequestMessage(HttpMethod.Delete,
                ApiRoutes.Spots.Delete.Replace("{id}", createResponse.Id.ToString()));
            var deleteResponseMessage = await _client.SendAsyncWithMasterAuthentication(deleteRequestMessage);

            // Assert
            deleteResponseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var getRequestMessage = new HttpRequestMessage(HttpMethod.Get,
                ApiRoutes.Spots.Get.Replace("{id}", createResponse.Id.ToString()));
            var getResponseMessage = await _client.SendAsyncWithMasterAuthentication(getRequestMessage);
            getResponseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        }

    }
}
