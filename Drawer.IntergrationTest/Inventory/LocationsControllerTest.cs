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

        async Task<long> CreateParentGroup()
        {
            var requestContent = new LocationAddCommandModel()
            {
                Name = Guid.NewGuid().ToString(),
                IsGroup = true
            };
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Locations.Create);
            requestMessage.Content = JsonContent.Create(requestContent);
            var ResponseMessage = await _client.SendAsyncWithMasterAuthentication(requestMessage);
            var locationId = await ResponseMessage.Content.ReadFromJsonAsync<long>();
            return locationId;
        }

        [Fact]
        public async Task CreateLocation_Returns_Ok_With_Content()
        {
            // Arrange
            var parentGroupId = await CreateParentGroup();
            var requestContent = new LocationAddCommandModel()
            {
                ParentGroupId = parentGroupId,
                Name = Guid.NewGuid().ToString(),
                Note = Guid.NewGuid().ToString(),
                IsGroup = false
            };
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Locations.Create);
            requestMessage.Content = JsonContent.Create(requestContent);

            // Act
            var responseMessage = await _client.SendAsyncWithMasterAuthentication(requestMessage);

            // Assert
            responseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var locationId = await responseMessage.Content.ReadFromJsonAsync<long>();
            locationId.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task BatchCreateLocation_Returns_Ok_With_Content()
        {
            var parentGroupId = await CreateParentGroup();
            // Arrange
            var requestContent = new List<LocationAddCommandModel>()
            {
                new LocationAddCommandModel()
                {
                    ParentGroupId = parentGroupId,
                    Name = Guid.NewGuid().ToString(),
                    Note = Guid.NewGuid().ToString(),
                    IsGroup = false
                },
                new LocationAddCommandModel()
                {
                    ParentGroupId = parentGroupId,
                    Name = Guid.NewGuid().ToString(),
                    Note = Guid.NewGuid().ToString(),
                    IsGroup = false
                }
            };
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Locations.BatchCreate);
            requestMessage.Content = JsonContent.Create(requestContent);

            // Act
            var responseMessage = await _client.SendAsyncWithMasterAuthentication(requestMessage);

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
            var parentGroupId = await CreateParentGroup();
            var requestContent = new LocationAddCommandModel()
            {
                ParentGroupId = parentGroupId,
                Name = Guid.NewGuid().ToString(),
                Note = Guid.NewGuid().ToString(),
                IsGroup = false
            };
            var createRequestMessage = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Locations.Create);
            createRequestMessage.Content = JsonContent.Create(requestContent);
            var createResponseMessage = await _client.SendAsyncWithMasterAuthentication(createRequestMessage);
            var locationId = await createResponseMessage.Content.ReadFromJsonAsync<long>();

            // Act
            var getRequestMessage = new HttpRequestMessage(HttpMethod.Get,
                ApiRoutes.Locations.Get.Replace("{id}", $"{locationId}"));
            var getResponseMessage = await _client.SendAsyncWithMasterAuthentication(getRequestMessage);

            // Assert
            getResponseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var location = await getResponseMessage.Content.ReadFromJsonAsync<LocationQueryModel>() ?? null!;
            location.Should().NotBeNull();
            location.Id.Should().Be(locationId);
            location.ParentGroupId.Should().Be(requestContent.ParentGroupId);
            location.Name.Should().Be(requestContent.Name);
            location.Note.Should().Be(requestContent.Note);
        }

        [Fact]
        public async Task GetLocations_Returns_Ok_With_CreatedLocations()
        {
            // Arrange
            var parentGroupId1 = await CreateParentGroup();
            var requestContent1 = new LocationAddCommandModel()
            {
                ParentGroupId = parentGroupId1,
                Name = Guid.NewGuid().ToString(),
                Note = Guid.NewGuid().ToString(),
                IsGroup = false
            };
            var createRequestMessage1 = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Locations.Create);
            createRequestMessage1.Content = JsonContent.Create(requestContent1);
            var createResponseMessage1 = await _client.SendAsyncWithMasterAuthentication(createRequestMessage1);

            var parentGroupId2 = await CreateParentGroup();
            var requestContent2 = new LocationAddCommandModel()
            {
                ParentGroupId = parentGroupId2,
                Name = Guid.NewGuid().ToString(),
                Note = Guid.NewGuid().ToString(),
                IsGroup = false
            };
            var createRequestMessage2 = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Locations.Create);
            createRequestMessage2.Content = JsonContent.Create(requestContent2);
            var createResponseMessage2 = await _client.SendAsyncWithMasterAuthentication(createRequestMessage2);

            // Act
            var getLocationsRequestMessage = new HttpRequestMessage(HttpMethod.Get, ApiRoutes.Locations.GetList);
            var getLocationsResponseMessage = await _client.SendAsyncWithMasterAuthentication(getLocationsRequestMessage);

            // Assert
            getLocationsResponseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var locationList = await getLocationsResponseMessage.Content.ReadFromJsonAsync<List<LocationQueryModel>>() ?? null!;
            locationList.Should().NotBeNull();
            locationList.Should().Contain(x =>
                x.ParentGroupId == requestContent1.ParentGroupId &&
                x.Name == requestContent1.Name &&
                x.Note == requestContent1.Note);
            locationList.Should().Contain(x =>
                x.ParentGroupId == requestContent2.ParentGroupId &&
                x.Name == requestContent2.Name &&
                x.Note == requestContent2.Note);
        }

        [Fact]
        public async Task UpdateLocation_Returns_Ok()
        {
            // Arrange
            var parentGroupId = await CreateParentGroup();
            var createContent = new LocationAddCommandModel()
            {
                ParentGroupId = parentGroupId,
                Name = Guid.NewGuid().ToString(),
                Note = Guid.NewGuid().ToString(),
                IsGroup = false
            };
            var createRequest = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Locations.Create);
            createRequest.Content = JsonContent.Create(createContent);
            var createResponse = await _client.SendAsyncWithMasterAuthentication(createRequest);
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
            var updateResponse = await _client.SendAsyncWithMasterAuthentication(updateRequest);

            // Assert
            updateResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var getRequest = new HttpRequestMessage(HttpMethod.Get,
                ApiRoutes.Locations.Get.Replace("{id}", $"{locationId}"));
            var getResponse = await _client.SendAsyncWithMasterAuthentication(getRequest);
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
            var parentGroupId = await CreateParentGroup();
            var createContent = new LocationAddCommandModel()
            {
                ParentGroupId = parentGroupId,
                Name = Guid.NewGuid().ToString(),
                Note = Guid.NewGuid().ToString(),
                IsGroup = false
            };
            var createRequest = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Locations.Create);
            createRequest.Content = JsonContent.Create(createContent);
            var createResponse = await _client.SendAsyncWithMasterAuthentication(createRequest);
            var locationId = await createResponse.Content.ReadFromJsonAsync<long>();

            // Act
            var deleteRequest = new HttpRequestMessage(HttpMethod.Delete,
                ApiRoutes.Locations.Delete.Replace("{id}", $"{locationId}"));
            var deleteResponse = await _client.SendAsyncWithMasterAuthentication(deleteRequest);

            // Assert
            deleteResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var getRequest = new HttpRequestMessage(HttpMethod.Get,
                ApiRoutes.Locations.Get.Replace("{id}", $"{locationId}"));
            var getResponse = await _client.SendAsyncWithMasterAuthentication(getRequest);
            getResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var location = await getResponse.Content.ReadFromJsonAsync<LocationQueryModel?>();
            location.Should().BeNull();
        }

    }
}
