using Drawer.Contract;
using Drawer.Contract.BasicInfo;
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

namespace Drawer.IntergrationTest.BasicInfo
{
    [Collection(ApiInstanceCollection.Default)]
    public class LocationsControllerTest
    {
        private readonly HttpClient _client;
        private readonly ITestOutputHelper _outputHelper;

        public LocationsControllerTest(ApiInstance apiInstance, ITestOutputHelper outputHelper)
        {
            _client = apiInstance.Client;
            _outputHelper = outputHelper;
        }

        async Task<long> CreateUpperLocation()
        {
            var request = new CreateLocationRequest(null, Guid.NewGuid().ToString(), null);
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Locations.Create);
            requestMessage.Content = JsonContent.Create(request);
            var ResponseMessage = await _client.SendAsyncWithMasterAuthentication(requestMessage);
            var Response = await ResponseMessage.Content.ReadFromJsonAsync<CreateLocationResponse>() ?? default!;
            return Response.Id;
        }

        [Theory]
        [InlineData("위치-생성-1", "note1")]
        public async Task CreateLocation_Returns_Ok_With_Content(string name, string note)
        {
            // Arrange
            var upperLocationId = await CreateUpperLocation();
            var request = new CreateLocationRequest(upperLocationId, name, note);
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Locations.Create);
            requestMessage.Content = JsonContent.Create(request);

            // Act
            var responseMessage = await _client.SendAsyncWithMasterAuthentication(requestMessage);

            // Assert
            responseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var response = await responseMessage.Content.ReadFromJsonAsync<CreateLocationResponse>() ?? default!;
            response.Should().NotBeNull();
        }

        [Theory]
        [InlineData(
            "위치-배치-1", "위치입니다", 
            "위치-배치-1", "위치입니다"
        )]
        public async Task BatchCreateLocation_Returns_Ok_With_Content(
            string name1, string note1,
            string name2, string note2)
        {
            var upperLocationId = await CreateUpperLocation();
            // Arrange
            var request = new BatchCreateLocationRequest(new List<BatchCreateLocationRequest.Location>()
            {
                new BatchCreateLocationRequest.Location(upperLocationId, name1, note1),
                new BatchCreateLocationRequest.Location(upperLocationId, name2, note2),
            });
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Locations.BatchCreate);
            requestMessage.Content = JsonContent.Create(request);

            // Act
            var responseMessage = await _client.SendAsyncWithMasterAuthentication(requestMessage);

            // Assert
            responseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var response = await responseMessage.Content.ReadFromJsonAsync<BatchCreateLocationResponse>() ?? default!;
            response.Should().NotBeNull();
            response.IdList.Count.Should().Be(2);
        }

        [Theory]
        [InlineData("위치-ID조회-1", "note1")]
        public async Task GetLocation_Returns_Ok_With_CreatedLocation(string name, string note)
        {
            // Arrange
            var upperLocationId = await CreateUpperLocation();
            var createRequest = new CreateLocationRequest(upperLocationId, name, note);
            var createRequestMessage = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Locations.Create);
            createRequestMessage.Content = JsonContent.Create(createRequest);
            var createResponseMessage = await _client.SendAsyncWithMasterAuthentication(createRequestMessage);
            var createResponse = await createResponseMessage.Content.ReadFromJsonAsync<CreateLocationResponse>() ?? null!;

            // Act
            var getRequestMessage = new HttpRequestMessage(HttpMethod.Get,
                ApiRoutes.Locations.Get.Replace("{id}", createResponse.Id.ToString()));
            var getResponseMessage = await _client.SendAsyncWithMasterAuthentication(getRequestMessage);

            // Assert
            getResponseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var getResponse = await getResponseMessage.Content.ReadFromJsonAsync<GetLocationResponse>() ?? null!;
            getResponse.Should().NotBeNull();
            getResponse.Id.Should().Be(createResponse.Id);
            getResponse.UpperLocationId.Should().Be(createRequest.UpperLocationId);
            getResponse.Name.Should().Be(createRequest.Name);
            getResponse.Note.Should().Be(createRequest.Note);
        }

        [Theory]
        [InlineData("위치-조회-1", "note1", "위치-조회-2", "note2")]
        public async Task GetLocations_Returns_Ok_With_CreatedLocations(string name1, string note1, string name2, string note2)
        {
            // Arrange
            var upperLocationId1 = await CreateUpperLocation();
            var createRequest1 = new CreateLocationRequest(upperLocationId1, name1, note1);
            var createRequestMessage1 = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Locations.Create);
            createRequestMessage1.Content = JsonContent.Create(createRequest1);
            var createResponseMessage1 = await _client.SendAsyncWithMasterAuthentication(createRequestMessage1);

            var upperLocationId2 = await CreateUpperLocation();
            var createRequest2 = new CreateLocationRequest(upperLocationId2, name2, note2);
            var createRequestMessage2 = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Locations.Create);
            createRequestMessage2.Content = JsonContent.Create(createRequest2);
            var createResponseMessage2 = await _client.SendAsyncWithMasterAuthentication(createRequestMessage2);

            // Act
            var getLocationsRequestMessage = new HttpRequestMessage(HttpMethod.Get, ApiRoutes.Locations.GetList);
            var getLocationsResponseMessage = await _client.SendAsyncWithMasterAuthentication(getLocationsRequestMessage);

            // Assert
            getLocationsResponseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var getLocationsResponse = await getLocationsResponseMessage.Content.ReadFromJsonAsync<GetLocationsResponse>() ?? null!;
            getLocationsResponse.Should().NotBeNull();
            getLocationsResponse.Locations.Should().NotBeNull();
            getLocationsResponse.Locations.Should()
                .Contain(x => x.UpperLocationId == upperLocationId1 && x.Name == name1 && x.Note == note1);
            getLocationsResponse.Locations.Should()
                .Contain(x => x.UpperLocationId == upperLocationId2 && x.Name == name2 && x.Note == note2);
        }

        [Theory]
        [InlineData("위치-수정-전", "note1", "위치-수정-후", "note2")]
        public async Task UpdateLocation_Returns_Ok(string name1, string note1, string name2, string note2)
        {
            // Arrange
            var upperLocationId = await CreateUpperLocation();
            var createRequest = new CreateLocationRequest(upperLocationId, name1, note1);
            var createRequestMessage = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Locations.Create);
            createRequestMessage.Content = JsonContent.Create(createRequest);
            var createResponseMessage = await _client.SendAsyncWithMasterAuthentication(createRequestMessage);
            var createResponse = await createResponseMessage.Content.ReadFromJsonAsync<CreateLocationResponse>() ?? null!;

            // Act
            var updateRequest = new UpdateLocationRequest(name2, note2);
            var updateRequestMessage = new HttpRequestMessage(HttpMethod.Put,
                ApiRoutes.Locations.Update.Replace("{id}", createResponse.Id.ToString()));
            updateRequestMessage.Content = JsonContent.Create(updateRequest);
            var updateResponseMessage = await _client.SendAsyncWithMasterAuthentication(updateRequestMessage);

            // Assert
            updateResponseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var getRequestMessage = new HttpRequestMessage(HttpMethod.Get,
                ApiRoutes.Locations.Get.Replace("{id}", createResponse.Id.ToString()));
            var getResponseMessage = await _client.SendAsyncWithMasterAuthentication(getRequestMessage);
            getResponseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var getResponse = await getResponseMessage.Content.ReadFromJsonAsync<GetLocationResponse>() ?? null!;
            getResponse.Should().NotBeNull();
            getResponse.Id.Should().Be(createResponse.Id);
            getResponse.Name.Should().Be(updateRequest.Name);
            getResponse.Note.Should().Be(updateRequest.Note);
        }

        [Theory]
        [InlineData("위치-삭제", "note")]
        public async Task DeleteLocation_Returns_Ok(string name, string note)
        {
            // Arrange
            var upperLocationId = await CreateUpperLocation();
            var createRequest = new CreateLocationRequest(upperLocationId, name, note);
            var createRequestMessage = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Locations.Create);
            createRequestMessage.Content = JsonContent.Create(createRequest);
            var createResponseMessage = await _client.SendAsyncWithMasterAuthentication(createRequestMessage);
            var createResponse = await createResponseMessage.Content.ReadFromJsonAsync<CreateLocationResponse>() ?? null!;

            // Act
            var deleteRequestMessage = new HttpRequestMessage(HttpMethod.Delete,
                ApiRoutes.Locations.Delete.Replace("{id}", createResponse.Id.ToString()));
            var deleteResponseMessage = await _client.SendAsyncWithMasterAuthentication(deleteRequestMessage);

            // Assert
            deleteResponseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var getRequestMessage = new HttpRequestMessage(HttpMethod.Get,
                ApiRoutes.Locations.Get.Replace("{id}", createResponse.Id.ToString()));
            var getResponseMessage = await _client.SendAsyncWithMasterAuthentication(getRequestMessage);
            getResponseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        }

    }
}
