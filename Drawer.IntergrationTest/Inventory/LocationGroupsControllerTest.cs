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
    public class LocationGroupsControllerTest
    {
        private readonly HttpClient _client;
        private readonly ITestOutputHelper _outputHelper;

        public LocationGroupsControllerTest(ApiInstance apiInstance, ITestOutputHelper outputHelper)
        {
            _client = apiInstance.Client;
            _outputHelper = outputHelper;
        }

        async Task<long> CreateParentGroup()
        {
            var requestContent = new LocationGroupAddCommandModel()
            {
                Name = Guid.NewGuid().ToString(),
            };
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.LocationGroups.Add);
            requestMessage.Content = JsonContent.Create(requestContent);
            var ResponseMessage = await _client.SendWithMasterAuthentication(requestMessage);
            var locationId = await ResponseMessage.Content.ReadFromJsonAsync<long>();
            return locationId;
        }

        [Fact]
        public async Task CreateLocationGroup_Returns_Ok_With_Content()
        {
            // Arrange
            var parentGroupId = await CreateParentGroup();
            var requestContent = new LocationGroupAddCommandModel()
            {
                ParentGroupId = parentGroupId,
                Name = Guid.NewGuid().ToString(),
                Note = Guid.NewGuid().ToString(),
            };
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.LocationGroups.Add);
            requestMessage.Content = JsonContent.Create(requestContent);

            // Act
            var responseMessage = await _client.SendWithMasterAuthentication(requestMessage);

            // Assert
            responseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var locationId = await responseMessage.Content.ReadFromJsonAsync<long>();
            locationId.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task BatchCreateLocationGroup_Returns_Ok_With_Content()
        {
            var parentGroupId = await CreateParentGroup();
            // Arrange
            var requestContent = new List<LocationGroupAddCommandModel>()
            {
                new LocationGroupAddCommandModel()
                {
                    ParentGroupId = parentGroupId,
                    Name = Guid.NewGuid().ToString(),
                    Note = Guid.NewGuid().ToString(),
                },
                new LocationGroupAddCommandModel()
                {
                    ParentGroupId = parentGroupId,
                    Name = Guid.NewGuid().ToString(),
                    Note = Guid.NewGuid().ToString(),
                }
            };
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.LocationGroups.BatchAdd);
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
        public async Task GetLocationGroup_Returns_Ok_With_CreatedLocationGroup()
        {
            // Arrange
            var parentGroupId = await CreateParentGroup();
            var requestContent = new LocationGroupAddCommandModel()
            {
                ParentGroupId = parentGroupId,
                Name = Guid.NewGuid().ToString(),
                Note = Guid.NewGuid().ToString(),
            };
            var createRequestMessage = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.LocationGroups.Add);
            createRequestMessage.Content = JsonContent.Create(requestContent);
            var createResponseMessage = await _client.SendWithMasterAuthentication(createRequestMessage);
            var locationId = await createResponseMessage.Content.ReadFromJsonAsync<long>();

            // Act
            var getRequestMessage = new HttpRequestMessage(HttpMethod.Get,
                ApiRoutes.LocationGroups.Get.Replace("{id}", $"{locationId}"));
            var getResponseMessage = await _client.SendWithMasterAuthentication(getRequestMessage);

            // Assert
            getResponseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var location = await getResponseMessage.Content.ReadFromJsonAsync<LocationGroupQueryModel>() ?? null!;
            location.Should().NotBeNull();
            location.Id.Should().Be(locationId);
            location.ParentGroupId.Should().Be(requestContent.ParentGroupId);
            location.Name.Should().Be(requestContent.Name);
            location.Note.Should().Be(requestContent.Note);
        }

        [Fact]
        public async Task GetLocationGroups_Returns_Ok_With_CreatedLocationGroups()
        {
            // Arrange
            var parentGroupId1 = await CreateParentGroup();
            var requestContent1 = new LocationGroupAddCommandModel()
            {
                ParentGroupId = parentGroupId1,
                Name = Guid.NewGuid().ToString(),
                Note = Guid.NewGuid().ToString(),
            };
            var createRequestMessage1 = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.LocationGroups.Add);
            createRequestMessage1.Content = JsonContent.Create(requestContent1);
            var createResponseMessage1 = await _client.SendWithMasterAuthentication(createRequestMessage1);

            var parentGroupId2 = await CreateParentGroup();
            var requestContent2 = new LocationGroupAddCommandModel()
            {
                ParentGroupId = parentGroupId2,
                Name = Guid.NewGuid().ToString(),
                Note = Guid.NewGuid().ToString(),
            };
            var createRequestMessage2 = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.LocationGroups.Add);
            createRequestMessage2.Content = JsonContent.Create(requestContent2);
            var createResponseMessage2 = await _client.SendWithMasterAuthentication(createRequestMessage2);

            // Act
            var getLocationGroupsRequestMessage = new HttpRequestMessage(HttpMethod.Get, ApiRoutes.LocationGroups.GetList);
            var getLocationGroupsResponseMessage = await _client.SendWithMasterAuthentication(getLocationGroupsRequestMessage);

            // Assert
            getLocationGroupsResponseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var locationList = await getLocationGroupsResponseMessage.Content.ReadFromJsonAsync<List<LocationGroupQueryModel>>() ?? null!;
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
        public async Task UpdateLocationGroup_Returns_Ok()
        {
            // Arrange
            var parentGroupId = await CreateParentGroup();
            var createContent = new LocationGroupAddCommandModel()
            {
                ParentGroupId = parentGroupId,
                Name = Guid.NewGuid().ToString(),
                Note = Guid.NewGuid().ToString(),
            };
            var createRequest = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.LocationGroups.Add);
            createRequest.Content = JsonContent.Create(createContent);
            var createResponse = await _client.SendWithMasterAuthentication(createRequest);
            var locationId = await createResponse.Content.ReadFromJsonAsync<long>();

            // Act
            var updateContent = new LocationGroupUpdateCommandModel()
            {
                Name = Guid.NewGuid().ToString(),
                Note = Guid.NewGuid().ToString(),
            };
            var updateRequest = new HttpRequestMessage(HttpMethod.Put,
                ApiRoutes.LocationGroups.Update.Replace("{id}", $"{locationId}"));
            updateRequest.Content = JsonContent.Create(updateContent);
            var updateResponse = await _client.SendWithMasterAuthentication(updateRequest);

            // Assert
            updateResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var getRequest = new HttpRequestMessage(HttpMethod.Get,
                ApiRoutes.LocationGroups.Get.Replace("{id}", $"{locationId}"));
            var getResponse = await _client.SendWithMasterAuthentication(getRequest);
            getResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var location = await getResponse.Content.ReadFromJsonAsync<LocationGroupQueryModel?>() ?? null!;
            location.Should().NotBeNull();
            location.Id.Should().Be(locationId);
            location.Name.Should().Be(updateContent.Name);
            location.Note.Should().Be(updateContent.Note);
        }

        [Fact]
        public async Task DeleteLocationGroup_Returns_Ok()
        {
            // Arrange
            var parentGroupId = await CreateParentGroup();
            var createContent = new LocationGroupAddCommandModel()
            {
                ParentGroupId = parentGroupId,
                Name = Guid.NewGuid().ToString(),
                Note = Guid.NewGuid().ToString(),
            };
            var createRequest = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.LocationGroups.Add);
            createRequest.Content = JsonContent.Create(createContent);
            var createResponse = await _client.SendWithMasterAuthentication(createRequest);
            var locationId = await createResponse.Content.ReadFromJsonAsync<long>();

            // Act
            var deleteRequest = new HttpRequestMessage(HttpMethod.Delete,
                ApiRoutes.LocationGroups.Remove.Replace("{id}", $"{locationId}"));
            var deleteResponse = await _client.SendWithMasterAuthentication(deleteRequest);

            // Assert
            deleteResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var getRequest = new HttpRequestMessage(HttpMethod.Get,
                ApiRoutes.LocationGroups.Get.Replace("{id}", $"{locationId}"));
            var getResponse = await _client.SendWithMasterAuthentication(getRequest);
            getResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var location = await getResponse.Content.ReadFromJsonAsync<LocationGroupQueryModel?>();
            location.Should().BeNull();
        }

    }
}
