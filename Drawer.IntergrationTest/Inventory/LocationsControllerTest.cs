using Drawer.Application.Services.Inventory.CommandModels;
using Drawer.Application.Services.Inventory.QueryModels;
using Drawer.Shared;
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

namespace Drawer.IntergrationTest.Inventory
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

        async Task<long> CreateGroup()
        {
            var requestContent = new LocationGroupAddCommandModel()
            {
                Name = Guid.NewGuid().ToString(),
            };
            var request = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.LocationGroups.Add);
            request.Content = JsonContent.Create(requestContent);
            var response = await _client.SendWithMasterAuthentication(request);
            var groupId = await response.Content.ReadFromJsonAsync<long>();
            return groupId;
        }

        [Fact]
        public async Task CreateLocation_Returns_Ok_With_Content()
        {
            // Arrange
            var groupId = await CreateGroup();
            var requestContent = new LocationAddCommandModel()
            {
                GroupId = groupId,
                Name = Guid.NewGuid().ToString(),
                Note = Guid.NewGuid().ToString(),
            };
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Locations.Add);
            requestMessage.Content = JsonContent.Create(requestContent);

            // Act
            var responseMessage = await _client.SendWithMasterAuthentication(requestMessage);

            // Assert
            responseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var locationId = await responseMessage.Content.ReadFromJsonAsync<long>();
            locationId.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task BatchCreateLocation_Returns_Ok_With_Content()
        {
            var groupId = await CreateGroup();
            // Arrange
            var requestContent = new List<LocationAddCommandModel>()
            {
                new LocationAddCommandModel()
                {
                    GroupId = groupId,
                    Name = Guid.NewGuid().ToString(),
                    Note = Guid.NewGuid().ToString(),
                },
                new LocationAddCommandModel()
                {
                    GroupId = groupId,
                    Name = Guid.NewGuid().ToString(),
                    Note = Guid.NewGuid().ToString(),
                }
            };
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Locations.BatchAdd);
            requestMessage.Content = JsonContent.Create(requestContent);

            // Act
            var responseMessage = await _client.SendWithMasterAuthentication(requestMessage);

            // Assert
            responseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var locationIdList = await responseMessage.Content.ReadFromJsonAsync<List<long>>() ?? default!;
            locationIdList.Should().NotBeNull();
            locationIdList.Count.Should().Be(2);
        }

        [Fact]
        public async Task GetLocation_Returns_Ok_With_CreatedLocation()
        {
            // Arrange
            var groupId = await CreateGroup();
            var requestContent = new LocationAddCommandModel()
            {
                GroupId = groupId,
                Name = Guid.NewGuid().ToString(),
                Note = Guid.NewGuid().ToString(),
            };
            var createRequestMessage = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Locations.Add);
            createRequestMessage.Content = JsonContent.Create(requestContent);
            var createResponseMessage = await _client.SendWithMasterAuthentication(createRequestMessage);
            var locationId = await createResponseMessage.Content.ReadFromJsonAsync<long>();

            // Act
            var getRequestMessage = new HttpRequestMessage(HttpMethod.Get,
                ApiRoutes.Locations.Get.Replace("{id}", $"{locationId}"));
            var getResponseMessage = await _client.SendWithMasterAuthentication(getRequestMessage);

            // Assert
            getResponseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var location = await getResponseMessage.Content.ReadFromJsonAsync<LocationQueryModel>() ?? null!;
            location.Should().NotBeNull();
            location.Id.Should().Be(locationId);
            location.GroupId.Should().Be(requestContent.GroupId);
            location.Name.Should().Be(requestContent.Name);
            location.Note.Should().Be(requestContent.Note);
        }

        [Fact]
        public async Task GetLocations_Returns_Ok_With_CreatedLocations()
        {
            // Arrange
            var groupId1 = await CreateGroup();
            var requestContent1 = new LocationAddCommandModel()
            {
                GroupId = groupId1,
                Name = Guid.NewGuid().ToString(),
                Note = Guid.NewGuid().ToString(),
            };
            var createRequestMessage1 = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Locations.Add);
            createRequestMessage1.Content = JsonContent.Create(requestContent1);
            var createResponseMessage1 = await _client.SendWithMasterAuthentication(createRequestMessage1);

            var groupId2 = await CreateGroup();
            var requestContent2 = new LocationAddCommandModel()
            {
                GroupId = groupId2,
                Name = Guid.NewGuid().ToString(),
                Note = Guid.NewGuid().ToString(),
            };
            var createRequestMessage2 = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Locations.Add);
            createRequestMessage2.Content = JsonContent.Create(requestContent2);
            var createResponseMessage2 = await _client.SendWithMasterAuthentication(createRequestMessage2);

            // Act
            var getLocationsRequestMessage = new HttpRequestMessage(HttpMethod.Get, ApiRoutes.Locations.GetList);
            var getLocationsResponseMessage = await _client.SendWithMasterAuthentication(getLocationsRequestMessage);

            // Assert
            getLocationsResponseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var locationList = await getLocationsResponseMessage.Content.ReadFromJsonAsync<List<LocationQueryModel>>() ?? null!;
            locationList.Should().NotBeNull();
            locationList.Should().Contain(x =>
                x.GroupId == requestContent1.GroupId &&
                x.Name == requestContent1.Name &&
                x.Note == requestContent1.Note);
            locationList.Should().Contain(x =>
                x.GroupId == requestContent2.GroupId &&
                x.Name == requestContent2.Name &&
                x.Note == requestContent2.Note);
        }

        [Fact]
        public async Task UpdateLocation_Returns_Ok()
        {
            // Arrange
            var groupId = await CreateGroup();
            var createContent = new LocationAddCommandModel()
            {
                GroupId = groupId,
                Name = Guid.NewGuid().ToString(),
                Note = Guid.NewGuid().ToString(),
            };
            var createRequest = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Locations.Add);
            createRequest.Content = JsonContent.Create(createContent);
            var createResponse = await _client.SendWithMasterAuthentication(createRequest);
            var locationId = await createResponse.Content.ReadFromJsonAsync<long>();

            // Act
            var updateContent = new LocationUpdateCommandModel()
            {
                Name = Guid.NewGuid().ToString(),
                Note = Guid.NewGuid().ToString(),
            };
            var updateRequest = new HttpRequestMessage(HttpMethod.Put,
                ApiRoutes.Locations.Update.Replace("{id}", $"{locationId}"));
            updateRequest.Content = JsonContent.Create(updateContent);
            var updateResponse = await _client.SendWithMasterAuthentication(updateRequest);

            // Assert
            updateResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var getRequest = new HttpRequestMessage(HttpMethod.Get,
                ApiRoutes.Locations.Get.Replace("{id}", $"{locationId}"));
            var getResponse = await _client.SendWithMasterAuthentication(getRequest);
            getResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var location = await getResponse.Content.ReadFromJsonAsync<LocationQueryModel?>() ?? null!;
            location.Should().NotBeNull();
            location.Id.Should().Be(locationId);
            location.Name.Should().Be(updateContent.Name);
            location.Note.Should().Be(updateContent.Note);
        }

        [Fact]
        public async Task DeleteLocation_Returns_Ok()
        {
            // Arrange
            var groupId = await CreateGroup();
            var createContent = new LocationAddCommandModel()
            {
                GroupId = groupId,
                Name = Guid.NewGuid().ToString(),
                Note = Guid.NewGuid().ToString(),
            };
            var createRequest = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Locations.Add);
            createRequest.Content = JsonContent.Create(createContent);
            var createResponse = await _client.SendWithMasterAuthentication(createRequest);
            var locationId = await createResponse.Content.ReadFromJsonAsync<long>();

            // Act
            var deleteRequest = new HttpRequestMessage(HttpMethod.Delete,
                ApiRoutes.Locations.Remove.Replace("{id}", $"{locationId}"));
            var deleteResponse = await _client.SendWithMasterAuthentication(deleteRequest);

            // Assert
            deleteResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var getRequest = new HttpRequestMessage(HttpMethod.Get,
                ApiRoutes.Locations.Get.Replace("{id}", $"{locationId}"));
            var getResponse = await _client.SendWithMasterAuthentication(getRequest);
            getResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var location = await getResponse.Content.ReadFromJsonAsync<LocationQueryModel?>();
            location.Should().BeNull();
        }

    }
}
